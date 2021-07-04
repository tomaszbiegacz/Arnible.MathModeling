using System;
using Arnible.Assertions;
using Arnible.Linq;
using Arnible.Linq.Algebra;
using Arnible.MathModeling.Algebra;
using Arnible.MathModeling.Geometry;
using Xunit;

namespace Arnible.MathModeling.Analysis.Optimization.SingleStep.Test.Strategy
{
  public record GoldenSecantStrategy : IFunctionValueOptimizationStrategy
  {
    private readonly ISimpleLogger _logger;
    private readonly INumberRangeDomain _argumentsDomain;
    private readonly ISingleStepOptimization _optimization;
    private readonly double? _extendedUniformSearchDisablingRatio = null;
    
    public bool WideSearch { get; init; } = false;
    public bool UniformSearchDirection { get; init; } = false;
    
    public Number? MinimumValue { get; init; }

    /// <summary>
    /// The smaller it is, the smaller is computation complexity per one loop
    /// </summary>
    public double? ExtendedUniformSearchDirectionDisablingRatio
    {
      get => _extendedUniformSearchDisablingRatio;
      init
      {
        if(value is not null)
        {
          value.Value.AssertIsBetween(0, 1);  
          _extendedUniformSearchDisablingRatio = value;
        }
      }
    }
    
    /// <summary>
    /// The bigger the number the smaller amount of axis will be used in extended search
    /// </summary>
    public Number ExtendedUniformSearchDirectionMinimumGradientRatio { get; init; } = 0.3; 

    public bool AnyMethod => WideSearch || UniformSearchDirection || _extendedUniformSearchDisablingRatio.HasValue;
    
    public GoldenSecantStrategy(in Number minArgument, in Number maxArgument, ISimpleLogger logger)
    {
      _logger = logger;
      _argumentsDomain = new NumberRangeDomain(minimum: in minArgument, maximum: in maxArgument);
      _optimization = new GoldenSecantSmoothlyConstrainedMinimum(_logger);
    }

    public FunctionImprovementStatistics FindImprovedArguments(ref FunctionMinimumImprovement solution)
    {
      AnyMethod.AssertIsTrue();
      
      IFunctionValueAnalysis valueAnalysis = solution.Function;
      Number currentValue = solution.Value;
      
      Span<Number> parameters = stackalloc Number[solution.ParametersCount];
      solution.Parameters.CopyTo(parameters);
      _logger.Write("Parameters: ", parameters).Write(" value: ", currentValue).NewLine();
      
      Span<Number> gradient = stackalloc Number[parameters.Length];
      solution.Function.GradientByArguments(solution.Parameters, in gradient);
      gradient.IsZero().AssertIsFalse();
      _logger.Write("Gradient: ", gradient).NewLine();

      Span<Number> optimalPoint = stackalloc Number[gradient.Length];
      Span<Number> filteredGradient = stackalloc Number[gradient.Length];
      FunctionImprovementStatistics statistics = new();
      
      if(WideSearch)
      {
        gradient.CopyTo(filteredGradient);
        _logger
          .Write("---").NewLine()
          .Write(">> WideSearch from ", parameters).Write(" value: ", currentValue).NewLine();
        ImproveWithGradient(
          valueAnalysis, parameters, in currentValue, in filteredGradient, 
          in optimalPoint, out Number value, out uint complexity);
        statistics.Complexity += complexity;
        if(solution.ConsiderSolution(in value, complexity, "WideSearch"))
          optimalPoint.CopyTo(solution.Parameters);
      }
      
      if(UniformSearchDirection)
      {
        if(gradient.CopyToIf((in Number v) => v < 0, defaultValue: 0, in filteredGradient))
        {
          _logger
            .Write("---").NewLine()
            .Write(">> NegativeDirectionSearch from ", parameters).Write(" value: ", currentValue).NewLine();
          ImproveWithGradient(
            valueAnalysis, parameters, in currentValue, in filteredGradient, 
            in optimalPoint, out Number value, out uint complexity);
          statistics.Complexity += complexity;
          if(solution.ConsiderSolution(in value, complexity, "NegativeDirectionSearch"))
            optimalPoint.CopyTo(solution.Parameters);
        }
      
        if(gradient.CopyToIf((in Number v) => v > 0, defaultValue: 0, in filteredGradient))
        {
          _logger
            .Write("---").NewLine()
            .Write(">> PositiveDirectionSearch from ", parameters).Write(" value: ", currentValue).NewLine();
          ImproveWithGradient(
            valueAnalysis, parameters, in currentValue, in filteredGradient, 
            in optimalPoint, out Number value, out uint complexity);
          statistics.Complexity += complexity;
          if(solution.ConsiderSolution(in value, complexity, "PositiveDirectionSearch"))
            optimalPoint.CopyTo(solution.Parameters);
        }  
      }
      
      if(solution.HasConjugateDirections)
      {
        solution.GetConjugateDirection(in filteredGradient);
        _logger
          .Write("---").NewLine()
          .Write(">> ConjugateDirection from ", parameters).Write(" value: ", currentValue).NewLine();
        ImproveInSearchDirection(
          valueAnalysis, parameters, filteredGradient, 
          in optimalPoint, out Number value, out uint complexity);
        statistics.Complexity += complexity;
        if(solution.ConsiderSolution(in value, complexity, "ConjugateDirection"))
          optimalPoint.CopyTo(solution.Parameters);
      }
      
      if(_extendedUniformSearchDisablingRatio.HasValue)
      {
        Number improvementRatio = currentValue == 0 ? solution.Value.Abs() : ((currentValue - solution.Value) / currentValue).Abs();
        if(_extendedUniformSearchDisablingRatio > improvementRatio)
        {
          ushort maxDerivativeAt = gradient.WithMaximumAt();
          if(gradient[maxDerivativeAt] > 0)
          {
            _logger
              .Write("---").NewLine()
              .Write(">> Positive ExtendedUniformSearchDirection from ", parameters)
              .Write(", value: ", currentValue)
              .Write(", gradient: ", gradient)
              .NewLine();
            GetExtendedUniformSearchDirection(valueAnalysis, parameters, maxDerivativeAt, gradient, in filteredGradient);
            ImproveInSearchDirection(
              valueAnalysis, parameters, filteredGradient, 
              in optimalPoint, out Number value, out uint complexity);
            statistics.Complexity += complexity;
            statistics.WithExtendedSearch = true;
            if(solution.ConsiderSolution(in value, complexity, "PositiveExtendedUniformSearchDirection"))
              optimalPoint.CopyTo(solution.Parameters);
          }

          ushort minimumDerivativeAt = gradient.WithMinimumAt();
          if(gradient[minimumDerivativeAt] < 0)
          {
            _logger
              .Write("---").NewLine()
              .Write(">> Negative ExtendedUniformSearchDirection from ", parameters)
              .Write(", value: ", currentValue)
              .Write(", gradient: ", gradient)
              .NewLine();
            GetExtendedUniformSearchDirection(valueAnalysis, parameters, minimumDerivativeAt, gradient, in filteredGradient);
            ImproveInSearchDirection(
              valueAnalysis, parameters, filteredGradient, 
              in optimalPoint, out Number value, out uint complexity);
            statistics.Complexity += complexity;
            statistics.WithExtendedSearch = true;
            if(solution.ConsiderSolution(in value, complexity, "NegativeExtendedUniformSearchDirection"))
              optimalPoint.CopyTo(solution.Parameters);
          }
        }
        else
        {
          _logger.Write("Skipped ExtendedUniformSearchDirection because improvementRatio is ", improvementRatio).NewLine();
        }
      }
      
      solution.FinaliseCurrentDirectionSearch(parameters);
      
      return statistics;
    }
    
    private void GetExtendedUniformSearchDirection(
      IFunctionValueAnalysis valueAnalysis,
      in ReadOnlySpan<Number> currentParameters,
      ushort directionCorePos,
      in Span<Number> gradient,
      in Span<Number> result)
    {
      Sign descentDirection = gradient[directionCorePos] > 0 ? Sign.Negative : Sign.Positive;

      Span<bool> searchDirection = stackalloc bool[result.Length];
      searchDirection.Clear();
      searchDirection[directionCorePos] = true;
      
      Span<Number> resultBuffer = stackalloc Number[result.Length];
      Number minimumDerivativeStep = GetDerivativeForUniformDirection(
        valueAnalysis, in currentParameters,
        searchDirection, descentDirection,
        in resultBuffer) * ExtendedUniformSearchDirectionMinimumGradientRatio;
      
      _logger
        .Write("Finding uniform direction search starting from axis ", directionCorePos)
        .Write(", direction ", descentDirection == Sign.Positive ? "positive" : "negative")
        .Write(" and minimum gradient: ", minimumDerivativeStep)
        .NewLine();
      
      bool doSearch = true;
      while(doSearch && !searchDirection.All())
      {
        int minimumDerivativeAt = -1;
        Number minimumDerivative = minimumDerivativeStep;
        for(ushort pos = 0; pos < searchDirection.Length; ++pos)
        {
          if(!searchDirection[pos])
          {
            // let's check it out
            searchDirection[pos] = true;
            Number currentDerivative = GetDerivativeForUniformDirection(
              valueAnalysis, in currentParameters,
              searchDirection, descentDirection,
              in resultBuffer);
            searchDirection[pos] = false;
            
            _logger
              .Write("- Checking direction ", pos)
              .Write(" got derivative ", currentDerivative)
              .NewLine();
            if(currentDerivative < minimumDerivative)
            {
              minimumDerivativeAt = pos;
              minimumDerivative = currentDerivative; 
            }
          }
        }
        
        if(minimumDerivativeAt == -1)
        {
          _logger.Write("Uniform direction search is complete.");
          WriteSearchDirection(searchDirection, descentDirection).NewLine();
          doSearch = false;
        }
        else
        {
          searchDirection[minimumDerivativeAt] = true;
          
          _logger.Write("Accepting change at axis ", minimumDerivativeAt);
          WriteSearchDirection(searchDirection, descentDirection).NewLine();
        }
      }
      
      FillUniformDirection(searchDirection, descentDirection, in result);
    }
    
    private ISimpleLogger WriteSearchDirection(in ReadOnlySpan<bool> searchDirection, Sign descentDirection)
    {
      _logger.Write(" got [");
      ReadOnlySpan<char> sign = descentDirection == Sign.Positive ? "+" : "-";
      ReadOnlySpan<char> separator = "";
      for(ushort i=0; i<searchDirection.Length; ++i)
      {
        if(searchDirection[i])
          _logger.Write(separator, sign);
        else
          _logger.Write(separator, "0");
        separator = ",";
      }
      return _logger.Write("]");
    }
    
    private static void FillUniformDirection(
      in ReadOnlySpan<bool> directions,
      Sign directionSign,
      in Span<Number> result)
    {
      Number uniformRatio = CartesianCoordinate.GetIdentityVectorRatio(directions.Count((in bool v) => v));
      if(directionSign == Sign.Negative)
        uniformRatio = -1 * uniformRatio;
      
      for(ushort i=0; i<result.Length; ++i)
      {
        if(directions[i])
          result[i] = uniformRatio;
        else
          result[i] = 0;
      }
    }
    
    private static Number GetDerivativeForUniformDirection(
      IFunctionValueAnalysis valueAnalysis,
      in ReadOnlySpan<Number> currentParameters,
      in ReadOnlySpan<bool> directions,
      Sign directionSign,
      in Span<Number> directionDerivativeRatios)
    {
      FillUniformDirection(in directions, directionSign, in directionDerivativeRatios);
      return valueAnalysis.GetValueWithDerivativeByArgumentsChangeDirection(
        arguments: in currentParameters,
        directionDerivativeRatios: directionDerivativeRatios).First;
    }

    private void ImproveWithGradient(
      IFunctionValueAnalysis valueAnalysis,
      in ReadOnlySpan<Number> currentParameters,
      in Number currentValue,
      in Span<Number> gradient,
      in Span<Number> potentialSolution,
      out Number value,
      out uint complexity)
    {
      _logger.Write("gradient: ", gradient).NewLine();
      gradient.MultiplySelfBy(-1);
      
      Span<Number> directionDerivativeRatios = stackalloc Number[currentParameters.Length];
      gradient.GetDirectionDerivativeRatios(in directionDerivativeRatios);
      ImproveInSearchDirection(
        valueAnalysis, in currentParameters, directionDerivativeRatios, 
        in potentialSolution, out value, out complexity);
      
      if(MinimumValue is not null)
      {
        _logger
          .Write("**Minimum search for ", MinimumValue.Value).NewLine();
        Number scalingFactor = currentValue - MinimumValue.Value;
        
        Span<Number> parameters = stackalloc Number[gradient.Length];
        gradient.CopyTo(parameters);
        parameters.MultiplySelfBy(scalingFactor);
        parameters.AddToSelf(currentParameters);
        
        if(_argumentsDomain.IsValid(in parameters))
        {
          Number valueForTest = valueAnalysis.GetValue(parameters);
          _logger
            .Write("PotentialSolution: ", parameters)
            .Write(", value: ", valueForTest)
            .NewLine();
        
          complexity += 1;
          if(valueForTest < value)
          {
            value = valueForTest;
            parameters.CopyTo(potentialSolution);
          }  
        }
        else
        {
          _logger.Write("point: ", parameters).Write(" outside of the domain.").NewLine();
        }
      }
    }
    
    private void ImproveInSearchDirection(
      IFunctionValueAnalysis valueAnalysis,
      in ReadOnlySpan<Number> currentParameters,
      in ReadOnlySpan<Number> directionDerivativeRatios,
      in Span<Number> potentialSolution,
      out Number value,
      out uint complexity)
    {
      _logger.Write("minimum search direction: ", directionDerivativeRatios).NewLine();
      
      Number? maxScalingFactor = _argumentsDomain.GetMaximumValidTranslationRatio(
        value: in currentParameters,
        delta: directionDerivativeRatios);
      if (maxScalingFactor == null || maxScalingFactor <= 0)
      {
        throw new InvalidOperationException("translationDirection is empty");
      }
      _logger.Write("maxScalingFactor: ", maxScalingFactor.Value).NewLine();

      FunctionValueAnalysisForDirection function = new(
        functionToAnalyse: valueAnalysis,
        startPosition: in currentParameters,
        directionDerivativeRatios: directionDerivativeRatios);
      
      NumberFunctionPointWithDerivative startPoint = function.ValueWithDerivative(0);
      if(startPoint.First >= 0)
      {
        _logger.Write("Got not negative first derivative, ignoring: ", startPoint.First).NewLine();
        
        currentParameters.CopyTo(potentialSolution);
        value = startPoint.Y;
        complexity = 0;
      }
      else
      {
        Number x = _optimization.MoveNext(
          in function, 
          startPoint: function.ValueWithDerivative(0), 
          borderX: maxScalingFactor.Value,
          complexity: out complexity
          ).X;
        _logger
          .Write("chosen scaling: ", x)
          .Write(" with complexity: ", complexity)
          .NewLine();
      
        function.GetPosition(in x, in potentialSolution);
        value = valueAnalysis.GetValue(potentialSolution);
        _logger
          .Write("PotentialSolution: ", potentialSolution)
          .Write(", value: ", value)
          .NewLine();  
      }
    }
  }
}
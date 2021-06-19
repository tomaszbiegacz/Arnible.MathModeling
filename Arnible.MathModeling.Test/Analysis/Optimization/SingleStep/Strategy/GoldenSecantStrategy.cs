using System;
using Arnible.Assertions;
using Arnible.Linq;
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
    
    public bool WideSearch { get; init; } = false;
    public bool UniformSearchDirection { get; init; } = false;
    public bool ExtendedUniformSearchDirection { get; init; } = false;
    public bool AnyMethod => WideSearch || UniformSearchDirection || ExtendedUniformSearchDirection;
    
    public GoldenSecantStrategy(in Number minArgument, in Number maxArgument, ISimpleLogger logger)
    {
      _logger = logger;
      _argumentsDomain = new NumberRangeDomain(minimum: in minArgument, maximum: in maxArgument);
      _optimization = new GoldenSecantSmoothlyConstrainedMinimum(_logger);
    }

    public void FindImprovedArguments(in FunctionMinimumImprovement solution)
    {
      AnyMethod.AssertIsTrue();
      
      IFunctionValueAnalysis valueAnalysis = solution.Function;
      
      Span<Number> parameters = stackalloc Number[solution.Parameters.Length];
      solution.Parameters.CopyTo(parameters);
      _logger.Write("Parameters: ", parameters).NewLine();
      
      Span<Number> gradient = stackalloc Number[parameters.Length];
      solution.Function.GradientByArguments(solution.Parameters, in gradient);
      gradient.IsZero().AssertIsFalse();
      _logger.Write("Gradient: ", gradient).NewLine();

      Span<Number> optimalPoint = stackalloc Number[gradient.Length];
      Span<Number> filteredGradient = stackalloc Number[gradient.Length];
      
      if(WideSearch)
      {
        _logger.Write("---").NewLine().Write(">> WideSearch from ", parameters).NewLine();
        gradient.CopyTo(filteredGradient);
        ImproveWithGradient(valueAnalysis, parameters, in filteredGradient, in optimalPoint);
        solution.ConsiderSolution(optimalPoint);
      }
      
      if(UniformSearchDirection)
      {
        if(gradient.CopyTo((in Number v) => v < 0, defaultValue: 0, in filteredGradient))
        {
          _logger.Write("---").NewLine().Write(">> NegativeDirectionSearch from ", parameters).NewLine();
          ImproveWithGradient(valueAnalysis, parameters, filteredGradient, in optimalPoint);
          solution.ConsiderSolution(optimalPoint);
        }
      
        if(gradient.CopyTo((in Number v) => v > 0, defaultValue: 0, in filteredGradient))
        {
          _logger.Write("---").NewLine().Write(">> PositiveDirectionSearch from ", parameters).NewLine();
          ImproveWithGradient(valueAnalysis, parameters, filteredGradient, in optimalPoint);
          solution.ConsiderSolution(optimalPoint);
        }  
      }
      
      if(ExtendedUniformSearchDirection)
      {
        ushort maxDerivativeAt = gradient.WithMaximumAt();
        if(gradient[maxDerivativeAt] > 0)
        {
          _logger.Write("---").NewLine().Write(">> Positive ExtendedUniformSearchDirection from ", parameters).NewLine();
          GetExtendedUniformSearchDirection(valueAnalysis, parameters, maxDerivativeAt, gradient, in filteredGradient);
          ImproveInSearchDirection(valueAnalysis, parameters, filteredGradient, in optimalPoint);
          solution.ConsiderSolution(optimalPoint);  
        }
        
        ushort minimumDerivativeAt = gradient.WithMinimumAt();
        if(gradient[minimumDerivativeAt] < 0)
        {
          _logger.Write("---").NewLine().Write(">> Negative ExtendedUniformSearchDirection from ", parameters).NewLine();
          GetExtendedUniformSearchDirection(valueAnalysis, parameters, minimumDerivativeAt, gradient, in filteredGradient);
          ImproveInSearchDirection(valueAnalysis, parameters, filteredGradient, in optimalPoint);
          solution.ConsiderSolution(optimalPoint);  
        }
      }
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
      
      bool doSearch = true;
      Span<Number> resultBuffer = stackalloc Number[result.Length];
      while(doSearch && !searchDirection.All())
      {
        int minimumDerivativeAt = -1;
        Number minimumDerivative = 0;
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
            
            if(currentDerivative < minimumDerivative)
            {
              minimumDerivativeAt = pos;
              minimumDerivative = currentDerivative; 
            }
          }
        }
        
        if(minimumDerivativeAt == -1)
        {
          // that's it
          doSearch = false;
        }
        else
        {
          searchDirection[minimumDerivativeAt] = true;
        }
      }
      
      FillUniformDirection(searchDirection, descentDirection, in result);
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
      in Span<Number> gradient,
      in Span<Number> potentialSolution)
    {
      _logger.Write("gradient: ", gradient).NewLine();
      gradient.MultiplySelf(-1);

      Span<Number> directionDerivativeRatios = stackalloc Number[currentParameters.Length];
      gradient.GetDirectionDerivativeRatios(in directionDerivativeRatios);
      ImproveInSearchDirection(valueAnalysis, in currentParameters, directionDerivativeRatios, in potentialSolution);
    }
    
    private void ImproveInSearchDirection(
      IFunctionValueAnalysis valueAnalysis,
      in ReadOnlySpan<Number> currentParameters,
      in ReadOnlySpan<Number> directionDerivativeRatios,
      in Span<Number> potentialSolution)
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
        Assert.Equal((double)startPoint.First, 0, 3);
        _logger.Write("XXX Warning: rounding error"); 
      }
      else
      {
        Number x = _optimization.MoveNext(
          in function, 
          startPoint: function.ValueWithDerivative(0), 
          borderX: maxScalingFactor.Value).X;
        _logger.Write("chosen scaling: ", x).NewLine();
      
        function.GetPosition(in x, in potentialSolution);
        _logger
          .Write("PotentialSolution: ", potentialSolution)
          .Write(", value: ", valueAnalysis.GetValue(potentialSolution))
          .NewLine();  
      }
    }
  }
}
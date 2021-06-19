using System;
using Arnible.Assertions;
using Arnible.Linq;
using Arnible.MathModeling.Algebra;
using Arnible.MathModeling.Geometry;

namespace Arnible.MathModeling.Analysis.Optimization.SingleStep.Test.Strategy
{
  public record GoldenSecantStrategy : IFunctionValueOptimizationStrategy
  {
    private readonly ISimpleLogger _logger;
    private readonly INumberRangeDomain _argumentsDomain;
    private readonly ISingleStepOptimization _optimization;
    
    public bool WideSearch { get; init; } = false;
    public bool UniformSearchDirection { get; init; } = false;
    public bool AnyMethod => WideSearch || UniformSearchDirection;
    
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
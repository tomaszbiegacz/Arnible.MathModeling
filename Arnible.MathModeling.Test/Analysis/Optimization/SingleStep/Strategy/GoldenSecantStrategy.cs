using System;
using Arnible.MathModeling.Algebra;
using Arnible.MathModeling.Geometry;

namespace Arnible.MathModeling.Analysis.Optimization.SingleStep.Test.Strategy
{
  public record GoldenSecantStrategy : IFunctionValueOptimizationStrategy
  {
    private readonly ISimpleLogger _logger;
    private readonly INumberRangeDomain _argumentsDomain;
    private readonly ISingleStepOptimization _optimization;
    
    public bool WideSearch { get; init; } = true;
    
    public GoldenSecantStrategy(in Number minArgument, in Number maxArgument, ISimpleLogger logger)
    {
      _logger = logger;
      _argumentsDomain = new NumberRangeDomain(minimum: in minArgument, maximum: in maxArgument);
      _optimization = new GoldenSecantSmoothlyConstrainedMinimum(_logger);
    }

    public void FindImprovedArguments(in FunctionMinimumImprovement solution)
    {
      IFunctionValueAnalysis valueAnalysis = solution.Function;
      ReadOnlySpan<Number> parameters = solution.Parameters;
      _logger.Write("Parameters: ", parameters).NewLine();
      
      Span<Number> gradient = stackalloc Number[parameters.Length];
      solution.Function.GradientByArguments(solution.Parameters, in gradient);
      _logger.Write("Gradient: ", gradient).NewLine();
      
      Span<Number> optimalPoint = stackalloc Number[gradient.Length];
      Span<Number> filteredGradient = stackalloc Number[gradient.Length];
      
      if(WideSearch)
      {
        _logger.NewLine().Write("> WideSearch").NewLine();
        gradient.CopyTo(filteredGradient);
        ImproveWithGradient(
          valueAnalysis, in parameters, 
          in filteredGradient, in optimalPoint);
        solution.ConsiderSolution(optimalPoint);
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
      _logger.Write("minimum direction: ", directionDerivativeRatios).NewLine();
      
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
      _logger.Write("Chosen scaling: ", x).NewLine();
      
      function.GetPosition(in x, in potentialSolution);
      _logger
        .Write("potentialSolution: ", potentialSolution)
        .Write(", value: ", valueAnalysis.GetValue(potentialSolution))
        .NewLine();
    }
  }
}
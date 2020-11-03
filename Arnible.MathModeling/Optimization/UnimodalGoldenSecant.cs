using System;
using Arnible.MathModeling.Algebra;
using Arnible.MathModeling.Export;

namespace Arnible.MathModeling.Optimization
{
  public class UnimodalGoldenSecant : IUnimodalOptimization
  {
    private readonly IMathModelingLogger _logger;
    private readonly GoldenSectionConstrained? _goldenSection;
    private UnimodalSecant? _secant;

    public UnimodalGoldenSecant(
      INumberFunctionWithDerivative f,
      ValueWithDerivative1 a,
      ValueWithDerivative1 b,
      IMathModelingLogger logger)
    {
      _logger = logger;
      if (a.First.GetSign() != b.First.GetSign())
      {
        _logger.Log($"  Starting with secant");
        _goldenSection = null;
        _secant = new UnimodalSecant(f: f, a: a, b: b, logger);
      }
      else
      {
        _logger.Log($"  Starting with golden section");
        _goldenSection = new GoldenSectionConstrained(f: f, a: a, b: b, logger);
        _secant = null;
      }
    }

    private IUnimodalOptimization CurrentOptimization
    {
      get
      {
        if (_secant != null)
          return _secant;
        if (_goldenSection != null)
          return _goldenSection;
        
        throw new InvalidOperationException("Something went wrong");
      }
    }

    public Number X => CurrentOptimization.X;
    public Number Y => CurrentOptimization.Y;
    public Number Width => CurrentOptimization.Width;
    public bool IsOptimal => CurrentOptimization.IsOptimal;
    
    public bool MoveNext()
    {
      if (IsOptimal)
      {
        // we're done here
        return false;
      }

      if (_secant == null && _goldenSection != null)
      {
        if (_goldenSection.IsSecantOptimizerReady)
        {
          _logger.Log($"  Promoting search to secant");
          _secant = _goldenSection.GetSecantOptimizer();
        }
      }

      if (_secant != null)
      {
        return _secant.MoveNext();
      }
      if (_goldenSection != null)
      {
        return _goldenSection.MoveNext();
      }
      
      throw new InvalidOperationException("Something went wrong");
    }
  }
}
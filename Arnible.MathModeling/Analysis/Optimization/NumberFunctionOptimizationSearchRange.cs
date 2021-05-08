using System;
using Arnible.Assertions;

namespace Arnible.MathModeling.Analysis.Optimization
{
  public struct NumberFunctionOptimizationSearchRange
  {
    private NumberFunctionPointWithDerivative _a;
    private NumberFunctionPointWithDerivative _b;

    public NumberFunctionOptimizationSearchRange(
      in NumberFunctionPointWithDerivative a,
      in NumberFunctionPointWithDerivative b)
    {
      if ((double)a.X <= (double)b.X)
      {
        _a = a;
        _b = b;  
      }
      else
      {
        _a = b;
        _b = a;
      }
    }

    private void AssignRange(
      in NumberFunctionPointWithDerivative a,
      in NumberFunctionPointWithDerivative b)
    {
      if ((double)a.X <= (double)b.X)
      {
        _a = a;
        _b = b;  
      }
      else
      {
        _a = b;
        _b = a;
      }
    }

    public readonly NumberFunctionPointWithDerivative Start => _a;
    public readonly NumberFunctionPointWithDerivative End => _b;
    public readonly Number Width => _b.X - _a.X;
    public readonly bool IsOptimal => _a.X == _b.X;

    //
    // Value comparison
    //
    
    public NumberFunctionPointWithDerivative BorderSmaller
    {
      readonly get => (double)_a.Y <= (double)_b.Y ? _a : _b;
      set
      {
        if ((double)_a.Y <= (double)_b.Y)
        {
          // update _a
          value.Y.AssertIsLessEqualThan(_b.Y);
          AssignRange(in value, in _b);
        }
        else
        {
          // update _b
          value.Y.AssertIsLessEqualThan(_a.Y);
          AssignRange(in value, in _a);
        }
      }
    }
    public NumberFunctionPointWithDerivative BorderGreater
    {
      readonly get => (double)_a.Y <= (double)_b.Y ? _b : _a;
      set
      {
        if ((double)_a.Y <= (double)_b.Y)
        {
          // update _b
          value.Y.AssertIsGreaterEqualThan(_a.Y);
          AssignRange(in value, in _a);
        }
        else
        {
          // update _a
          value.Y.AssertIsGreaterEqualThan(_b.Y);
          AssignRange(in value, in _b);
        }
      }
    }
    
    //
    // Derivative comparison
    //

    public NumberFunctionPointWithDerivative BorderLowestDerivative
    {
      readonly get => (double)_a.First <= (double)_b.First ? _a : _b;
      set
      {
        if ((double)_a.First <= (double)_b.First)
        {
          // update _a
          value.First.AssertIsLessEqualThan(_b.First);
          AssignRange(in value, in _b);
        }
        else
        {
          // update _b
          value.First.AssertIsLessEqualThan(_a.First);
          AssignRange(in value, in _a);
        }
      }
    }
    public NumberFunctionPointWithDerivative BorderGreatestDerivative
    {
      readonly get => (double)_a.First <= (double)_b.First ? _b : _a;
      set
      {
        if ((double)_a.First <= (double)_b.First)
        {
          // update _b
          value.First.AssertIsGreaterEqualThan(_a.First);
          AssignRange(in value, in _a);
        }
        else
        {
          // update _a
          value.First.AssertIsGreaterEqualThan(_b.First);
          AssignRange(in value, in _b);
        }
      }
    }
    
    //
    // Utilities
    //
    
    public readonly void Log(
      ISimpleLogger logger,
      string message,
      in NumberFunctionPointWithDerivative c)
    {
      logger.Write("[");
      _a.Write(logger);
      logger.Write(", ");
      _b.Write(logger);
      logger.Write("] ", message, ", c:");
      c.Write(logger);
      logger.Write(Environment.NewLine);
    }
  }
}
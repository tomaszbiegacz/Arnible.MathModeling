﻿using Arnible.Assertions;
using Xunit;
using static Arnible.MathModeling.Algebra.Polynomials.Term;
using static Arnible.MathModeling.Algebra.Polynomials.MetaMath;

namespace Arnible.MathModeling.Algebra.Polynomials.Tests
{  
  public class PolynomialTautology
  {
    [Fact]
    public void Factoring_a2_minus_b2()
    {
      ((a - b) * (a + b)).AssertIsEqualTo(a.ToPower(2) - b.ToPower(2));
    }

    [Fact]
    public void Factoring_a3_minus_b3()
    {
      ((a - b) * (a.ToPower(2) + b.ToPower(2) + a * b)).AssertIsEqualTo(a.ToPower(3) - b.ToPower(3));
    }

    [Fact]
    public void Factoring_a3_plus_b3()
    {
      ((a + b) * (a.ToPower(2) + b.ToPower(2) - a * b)).AssertIsEqualTo(a.ToPower(3) + b.ToPower(3));
    }    

    [Fact]
    public void Trigonometric_one()
    {
      (Sin(a).ToPower(2) + Cos(a).ToPower(2)).DerivativeBy(a).AssertIsEqualTo(0);
    }
  }
}

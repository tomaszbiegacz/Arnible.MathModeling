# Arnible

[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](https://github.com/tomaszbiegacz/Arnible.MathModeling/blob/master/LICENSE)
[![CI-Linux](https://github.com/tomaszbiegacz/Arnible.MathModeling/actions/workflows/ci-linux.yml/badge.svg)](https://github.com/tomaszbiegacz/Arnible.MathModeling/actions/workflows/ci-linux.yml)
[![CI-Windows](https://github.com/tomaszbiegacz/Arnible.MathModeling/actions/workflows/ci-windows.yml/badge.svg)](https://github.com/tomaszbiegacz/Arnible.MathModeling/actions/workflows/ci-windows.yml)
[![Coverage Status](https://coveralls.io/repos/github/tomaszbiegacz/Arnible.MathModeling/badge.svg?branch=master)](https://coveralls.io/github/tomaszbiegacz/Arnible.MathModeling?branch=master)
[![CodeQL](https://github.com/tomaszbiegacz/Arnible.MathModeling/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/tomaszbiegacz/Arnible.MathModeling/actions/workflows/codeql-analysis.yml)

Tooltik manifesto:
* I should have support for LINQ operators over `Span<T>`, `ReadOnlySpan<T>`, `Memory<T>` and `ReadOnlyMemory<T>`.
  I need this to allow me writing quick and dirty prototypes over new collections in the same way as I do for `IEnumerable<T>`.
* LINQ should have clear API that will allow me to write defensive and self validating logic, like for example `SumDefensive` that will throw an error when being called over empty collection.
* Provide basic optimization algorithms for functions in multidimensional space.

Arnible toolkit is meant to address the above limitations by providing extensions over latest .Net framework.
The toolkit is split into libraries:
* [Arnible](./Arnible) with basic interfaces and utilities like logger interface or ReadOnlyArray value type.
* [Arnible.Assertions](./Arnible.Assertions) focusing on defensive programming support, like extensions method `.AssertIsEqualTo`.
* [Arnible.Export](./Arnible.Export) is boxing free exporting library focused on minimal memory and processing footprint needed for diagnostics.
* [Arnible.Linq](./Arnible.Linq) adds support for LINQ with defensive API like "SumDefensive" or "SumWithDefault" together with LINQ for combinatorics and ReadOnlySpan support.
* [Arnible.MathModeling](./Arnible.MathModeling) and [Arnible.MathModeling.Formal](./Arnible.MathModeling.Formal) contains various tools for numeric and symbolic math analysis.
* [Arnible.xunit](./Arnible.xunit) simplifies writing xunit tests for projects using Arnible toolkit.

Basic assumptions:
* Optimize code for `x86` architecture. Prefer horizontal over vertical scalling.
* Leverage `stackalloc` to avoid GC involvement and overall performance degradation.
* Avoid asynchronous operations as part of diagnostics at all cost.
* Avoid using `IEnumerable<T>` due to boxing and GC overhead.

## How to use it

I haven't reached yet version 1.0, work is still in progress.
For this reason the toolkit is not yet available as NuGet. There is simply too much risk of breaking backward compatibility with next revisions. Anyway I am open for negotiations.

If you want to use this toolkit simply get it locally and build it via `dotnet build` or run unit tests with `dotnet test`. Linux and Windows OS are supported.


## Arnible.MathModeling

Math modeling library is split into areas:
* [Number](./Arnible.MathModeling/Number.cs) value type and supporting it LINQ operations.
* [Algebra](./Arnible.MathModeling/Algebra) namespace with of various mathematical structures like group, rings and derived from them operators.
* [Analysis](./Arnible.MathModeling/Analysis) namespace with derivative value types and based on it optimization algorithms.
* [Geometry](./Arnible.MathModeling/Geometry) namespace with definition of cartesian and hyperspherical coordinate system.

`Arnible.MathModeling` is numeric math modeling library. 

## Arnible.MathModeling.Formal

Library reuses most of the code from `Arnible.MathModeling`, but replaces definition of `Number` to allow symbolic rather than numeric modeling.

To show it's usage assume that square error is defined as class
```C#
public class SquareError : IBinaryOperation<Number>
{    
    public Number Value(in Number x, in Number y)
    {
      return (x - y).ToPower(2);
    }

    public Derivative2Value DerivativeByX(in RectangularCoordinate p)
    {
      return new Derivative2Value(
        first: 2 * (p.X - p.Y),
        second: 2);
    }

    public Derivative2Value DerivativeByY(in RectangularCoordinate p)
    {
      return new Derivative2Value(
        first: -2 * (p.X - p.Y),
        second: 2);
    }

    public Derivative2Value DerivativeByR(in PolarCoordinate p)
    {
      return new Derivative2Value(
        first: 2 * p.R * (Cos(p.Φ) - Sin(p.Φ)).ToPower(2),
        second: 2 * (Cos(p.Φ) - Sin(p.Φ)).ToPower(2)
        );
    }     

    public Derivative2Value DerivativeByΦ(in PolarCoordinate p)
    {
      return new Derivative2Value(
        first: 2 * p.R.ToPower(2) * (Sin(p.Φ).ToPower(2) - Cos(p.Φ).ToPower(2)),
        second: 8 * p.R.ToPower(2) * Cos(p.Φ) * Sin(p.Φ)
        );
    }
}
```

Based on this class the following tests can be written to verify above formulas correctness
```C#
public class SquareErrorTests
{
    private readonly SquareError _error = new SquareError();

    static void AreDerivativesEqual(in Number value, in Number term, in Derivative1Value actual)
    {
      AreDerivativesEqual((PolynomialDivision)value, in term, in actual);
    }

    static void AreDerivativesEqual(in PolynomialDivision value, in Number term, in Derivative1Value actual)
    {
      PolynomialTerm termSingle = (PolynomialTerm)term;
      Number firstDerivative = value.DerivativeBy(termSingle);
      firstDerivative.AssertIsEqualTo(actual.First);
    }

    [Fact]
    public void Value()
    {
      _error.Value(x, y).AssertIsEqualTo((x - y).ToPower(2));
    }

    [Fact]
    public void DerivativeByX()
    {
      var p = new RectangularCoordinate(x, y);
      AreDerivativesEqual(_error.Value(x, y), x, _error.DerivativeByX(p));
    }
}
```

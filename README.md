# Arnible.MathModelling

Beside math modelling tools, this library extends LINQ operators for various math specific operations  like `Product` or `AggregateCombinations`.

## Lazy operations on derivatives

Library can be helpful when calculating first and second derivative over combinations of functions with known derivatives.

```C#
var d1 = new DerivativeValue(2, 3);
var d2 = new DerivativeValue(5, 7);
var d3 = new DerivativeValue(11, 13);
var d = DerivativeOperator.Multiply(d1, d2, d3);
Assert.Equal(2 * 5 * 11, d.First);
Assert.Equal(3 * 5 * 5 * 11 * 11 + 2 * 7 * 11 * 11 + 2 * 5 * 13, d.Second);
```

## Polynomials arithmetic

Usage for operations over polynomials can be shown via example

```C#
var v1 = new Polynomial('x', +1);
var v2 = new Polynomial('x', -1);

var v = v1 * v2;
Assert.Equal(new Polynomial(('x', 2), -1), v);
Assert.Equal(new PolynomialVariable(2) * 'x', v.DerivativeBy('x'));
Assert.Equal(0, v.DerivativeBy('y'));
```

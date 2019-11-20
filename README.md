# Arnible.MathModelling

Beside math modelling tools, this library extends LINQ operators for various math specific operations  like `Product` or `AggregateCombinations`.

## Lazy operations on derivatives

Library can be helpful when calculating first and second derivative over composition of functions with known derivatives.

```C#
var d1 = new DerivativeValue(2, 3);
var d2 = new DerivativeValue(5, 7);
var d3 = new DerivativeValue(11, 13);
var d = DerivativeOperator.Multiply(d1, d2, d3);
Assert.Equal(2 * 5 * 11, d.First);
Assert.Equal(3 * 5 * 5 * 11 * 11 + 2 * 7 * 11 * 11 + 2 * 5 * 13, d.Second);
```

## Polynomials arithmetic

Usage for basic operations over polynomials can be shown via example

```C#
PolynomialTerm x = 'x';
var poly = (x + 1) * (x - 1);

Assert.Equal(x * x - 1, poly);
Assert.Equal(2 * x, poly.DerivativeBy(x));
Assert.Equal(0, poly.DerivativeBy('y'));
```

Linq operations are also supported

```C#
Polynomial x = 'x';
Polynomial z = 'z';

Assert.Equal(z * (x * x - 1), (new[] { x - 1, x + 1, z }).Product());
```

The more interesting bit starts polynomials composition

```C#
PolynomialTerm x = 'x';
PolynomialTerm y = 'y';

var entry = 1 + x * x - y * y;
Assert.Equal(2 + 2 * y, entry.Composition(x, y + 1));
```

### Polynomials division

Division of polynomials can be seen by example

```C#
PolynomialTerm x = 'x';
PolynomialTerm y = 'y';

var expression = (y + x + 1) / (2 * x + 1);
Assert.Equal(-1 * (2 * y + 1) / (4 * x * x + 4 * x + 1), expression.DerivativeBy(x));
Assert.Equal(1 / (2 * x + 1), expression.DerivativeBy(y));

Assert.True(expression.DerivativeBy(y).DerivativeBy(y).IsZero);
```

and basic reduction

```C#
PolynomialTerm x = 'x';
PolynomialTerm y = 'y';
      
Assert.Equal(x + y, (x * x - y * y).ReduceBy(x - y));
```

### Roadmap

Todo:
* Polynomial Factorizer/Simplifier.

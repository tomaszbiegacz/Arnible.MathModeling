# Arnible.MathModelling

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

## Polynomials division

Division of polynomials can be seen by example

```C#
PolynomialTerm x = 'x';
PolynomialTerm y = 'y';

var expression = 1 + (y + x + 1) / (2 * x + 1);
Assert.Equal(-1 * (2 * y + 1) / (4 * x * x + 4 * x + 1), expression.DerivativeBy(x));
Assert.Equal(1 / (2 * x + 1), expression.DerivativeBy(y));

Assert.True(expression.DerivativeBy(y).DerivativeBy(y).IsZero);
```

Basic reduction

```C#
PolynomialTerm x = 'x';
PolynomialTerm y = 'y';
      
Assert.Equal(x + y, (x * x - y * y).ReduceBy(x - y));
```

or calculating reduction leftovers

```C#
PolynomialTerm x = 'x';

var toDivide = (x - 1) * (x - 1) * (x + 1) + 2 * x + 3;
Assert.Equal(2 * x + 3, toDivide % (x * x - 1));
``` 

## Polynomials composition

Example of polynomials composition

```C#
PolynomialTerm x = 'x';
PolynomialTerm y = 'y';

var entry = 1 + x * x - y * y;
Assert.Equal(2 + 2 * y, entry.Composition(x, y + 1));
```

and composition of polynomials division

```C#
PolynomialTerm x = 'x';
PolynomialTerm y = 'y';

PolynomialDivision entry = x / (x + 1);
Assert.Equal(y / (x + y), entry.Composition(x, y / x));
```

## Polar coordinates

Following basic support of trigonometric functions, polynomials can be also analysed in polar coordinates

```C#
using static Arnible.MathModeling.MetaMath;
using static Arnible.MathModeling.Term;

public class PolarCoordinatesTest
{
  [Fact]
  public void ErrorExpression()
  {
    // error expression in cartesian coordinates (x, y)
    var error = (c - x * y).ToPower(2);
      
    Assert.Equal(
      -2 * y * (c - x * y), 
      error.DerivativeBy(x));

    Assert.Equal(
      -2 * x * (c - x * y), 
      error.DerivativeBy(y));

    // error expression in polar coordinates (r, θ)
    var errorPolar = error.Composition(x, r * Cos(θ)).Composition(y, r * Sin(θ));

    Assert.Equal(
      (c - r.ToPower(2) * Sin(θ) * Cos(θ)).ToPower(2),
      errorPolar);

    Assert.Equal(
      -4 * r * Sin(θ) * Cos(θ) * (c - r.ToPower(2) * Sin(θ) * Cos(θ)), 
      errorPolar.DerivativeBy(r));

    Assert.Equal(
      -2 * r.ToPower(2) * (c - r.ToPower(2) * Sin(θ) * Cos(θ)) * (Cos(θ).ToPower(2) - Sin(θ).ToPower(2)),
      errorPolar.DerivativeBy(θ));
  }
}
```

## Roadmap

Todo:
* Polynomial Factorizer/Simplifier.

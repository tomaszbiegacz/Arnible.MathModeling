# Arnible.MathModeling/Analysis

Analysis namespace contains implementation of basic optimization algorithms:
* golden section method
* secant method

From one of the tests

```C#
[Fact]
public void Unimodal_Square_Optimum()
{
    var f = new SquareTestFunction();
    var a = f.ValueWithDerivative(-1);
    var b = f.ValueWithDerivative(2);
    
    var method = new GoldenSectionConstrained(f: f, a: a, b: b, Logger);
    method.X.AssertIsEqualTo(2);
    method.Y.AssertIsEqualTo(4);

    ushort i = OptimizationHelper.FindOptimal(method);
    
    method.X.AssertIsEqualTo(1);
    i.AssertIsEqualTo(26);
}
```

Implemented optimization methods are meant to allow moving in the optimimum direction with minimum cost.
They are meant to be part of other orchestrating algorithm.

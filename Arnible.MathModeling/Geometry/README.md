# Arnible.MathModeling/Geometry

Tools for convesion between cartesian and hyperspherical coordinates.
From one of the tests

```c#
[Fact]
public void CubeToSphericalTransformation_Known()
{
    var cc = new CartesianCoordinate(Math.Sqrt(2), Math.Sqrt(2), 2 * Math.Sqrt(3));

    const double φ = Math.PI / 4;   // x to r(xy)
    const double θ = Math.PI / 3;   // xy to r

    HypersphericalCoordinate hc = cc.ToSphericalView();
    hc.DimensionsCount.AssertIsEqualTo(3);
    hc.R.AssertIsEqualTo(4d);
    hc.Angles[0].AssertIsEqualTo(φ);
    hc.Angles[1].AssertIsEqualTo(θ);

    Derivative1Value[] derivatives = hc.ToCartesianView().DerivativeByR().ToArray();
    derivatives.Length.AssertIsEqualTo(3);
    derivatives[0].First.AssertIsEqualTo(Math.Sqrt(2) / 4);      // x
    derivatives[1].First.AssertIsEqualTo(Math.Sqrt(2) / 4);      // y
    derivatives[2].First.AssertIsEqualTo(Math.Sqrt(3) / 2);      // z

    ((CartesianCoordinate)hc.ToCartesianView()).AssertIsEqualTo(cc);
}
```

For API manual, please refer to tests.

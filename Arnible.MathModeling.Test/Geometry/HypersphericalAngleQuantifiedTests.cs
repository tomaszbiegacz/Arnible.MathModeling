using Arnible.MathModeling.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Arnible.MathModeling.Test.Geometry
{
  public class HypersphericalAngleQuantifiedTests
  {
    private static void AssertDirection(List<HypersphericalAngleQuantified> directions, params sbyte[] expected)
    {
      int pos = directions.IndexOf(d => expected.SequenceEqual(d.Angles));
      Assert.True(pos >= 0);

      directions.RemoveAt(pos);
    }

    private static HypersphericalAngleQuantified GetDirection(List<HypersphericalAngleQuantified> directions, params sbyte[] expected)
    {
      int pos = directions.IndexOf(d => expected.SequenceEqual(d.Angles));
      Assert.True(pos >= 0);

      return directions[pos];
    }

    [Fact]
    public void ToVector_Single()
    {
      var directions = HypersphericalAngleQuantified.GetNonLinearDirections(anglesCount: 1, resolution: 2).ToList();

      Assert.Equal(new HypersphericalAngleVector(-1 * Math.PI / 4), directions.Single(d => d.Angles.Single() == -1).ToVector());
      Assert.Equal(new HypersphericalAngleVector(0), directions.Single(d => d.Angles.Single() == 0).ToVector());
      Assert.Equal(new HypersphericalAngleVector(Math.PI / 4), directions.Single(d => d.Angles.Single() == 1).ToVector());
      Assert.Equal(new HypersphericalAngleVector(Math.PI / 2), directions.Single(d => d.Angles.Single() == 2).ToVector());
    }

    [Fact]
    public void GetNonLinearDirections_OneAngle()
    {
      var directions = HypersphericalAngleQuantified.GetNonLinearDirections(anglesCount: 1, resolution: 2).ToList();

      // axis directions 1d: 4 = 2*2
      AssertDirection(directions, 0);        //   0     + 0    x
      AssertDirection(directions, 2);        //  90     0 +    y

      // remaining directions 2d, 1 variation: 4 = 2^2
      AssertDirection(directions, -1);       //  -45    + -    xy
      AssertDirection(directions, 1);        //   45    + +    xy

      Assert.Empty(directions);
    }

    [Fact]
    public void GetNonLinearDirections_TwoAngles()
    {
      var directions = HypersphericalAngleQuantified.GetNonLinearDirections(anglesCount: 2, resolution: 2).ToList();

      // axis directions 1d: 6 = 3*2
      AssertDirection(directions, 0, 0);     //   0    0      + 0 0      x       - 0 0
      AssertDirection(directions, 2, 0);     //  90    0      0 + 0      y       0 - 0
      AssertDirection(directions, 0, 2);     //   0   90      0 0 +      z       0 0 -   

      // directions 2d, 1 variation: 12 = (2+1 = 3)*4         [ (2 from 3)*(remaining directions 2d) ]
      AssertDirection(directions, 1, 0);     //  45    0      + + 0      xy      - - 0
      AssertDirection(directions, -1, 0);    // -45    0      + - 0      xy      - + 0
      AssertDirection(directions, 0, 1);     //   0   45      + 0 +      xz      - 0 -
      AssertDirection(directions, 0, -1);    //   0  -45      + 0 -      xz      - 0 +
      AssertDirection(directions, 2, 1);     //  90   45      0 + +      yz      0 - -
      AssertDirection(directions, 2, -1);    //  90  -45      0 + -      yz      0 - +

      // remaining directions 3d, 2 variations: 8 = 2^3 
      AssertDirection(directions, -1, -1);   // -45 -45       + - -      xyz     - + +      
      AssertDirection(directions, 1, -1);    //  45 -45       + + -      xyz     - - +
      AssertDirection(directions, -1, 1);    // -45  45       + - +      xyz     - + -
      AssertDirection(directions, 1, 1);     //  45  45       + + +      xyz     - - -

      Assert.Empty(directions);
    }

    [Fact]
    public void GetNonLinearDirections_ThreeAngles()
    {
      var directions = HypersphericalAngleQuantified.GetNonLinearDirections(anglesCount: 3, resolution: 2).ToList();

      // axis directions 1d: 8 = 4*2
      AssertDirection(directions, 0, 0, 0);     //   0    0    0     + 0 0 0     x       - 0 0 0
      AssertDirection(directions, 2, 0, 0);     //  90    0    0     0 + 0 0     y       0 - 0 0
      AssertDirection(directions, 0, 2, 0);     //   0   90    0     0 0 + 0     z       0 0 - 0 
      AssertDirection(directions, 0, 0, 2);     //   0    0   90     0 0 0 +     a       0 0 0 - 

      // directions 2d, 1 variation: 24 = (3+2+1 = 6)*4              [ (2 from 4)*(remaining directions 2d) ]
      AssertDirection(directions, 1, 0, 0);     //  45    0   0      + + 0 0     xy      - - 0 0
      AssertDirection(directions, -1, 0, 0);    // -45    0   0      + - 0 0     xy      - + 0 0
      AssertDirection(directions, 0, 1, 0);     //   0   45   0      + 0 + 0     xz      - 0 - 0      
      AssertDirection(directions, 0, -1, 0);    //   0  -45   0      + 0 - 0     xz      - 0 + 0
      AssertDirection(directions, 0, 0, 1);     //   0    0  45      + 0 0 +     xa      - 0 0 -      
      AssertDirection(directions, 0, 0, -1);    //   0    0 -45      + 0 0 -     xa      - 0 0 +
      AssertDirection(directions, 2, 1, 0);     //  90   45   0      0 + + 0     yz      0 - - 0
      AssertDirection(directions, 2, -1, 0);    //  90  -45   0      0 + - 0     yz      0 - + 0
      AssertDirection(directions, 2, 0, 1);     //  90    0  45      0 + 0 +     ya      0 - 0 -
      AssertDirection(directions, 2, 0, -1);    //  90    0 -45      0 + 0 -     ya      0 - 0 +
      AssertDirection(directions, 0, 2, 1);     //   0   90  45      0 0 + +     za      0 0 - -  // right angle on Z cancels XY
      AssertDirection(directions, 0, 2, -1);    //   0   90 -45      0 0 + -     za      0 0 - +

      // directions 3d, 2 variation: 48 = (2+1+1)*8                  [ (3 from 4)*(remaining directions 3d) ]
      AssertDirection(directions, -1, -1, 0);   // -45  -45   0      + - - 0     xyz     - + + 0
      AssertDirection(directions, 1, -1, 0);    //  45  -45   0      + + - 0     xyz     - - + 0
      AssertDirection(directions, -1, 1, 0);    // -45   45   0      + - + 0     xyz     - + - 0
      AssertDirection(directions, 1, 1, 0);     //  45   45   0      + + + 0     xyz     - - - 0

      AssertDirection(directions, -1, 0, -1);   // -45    0 -45      + - 0 -     xya     - + 0 +
      AssertDirection(directions, 1, 0, -1);    //  45    0 -45      + + 0 -     xya     - - 0 +
      AssertDirection(directions, -1, 0, 1);    // -45    0  45      + - 0 +     xya     - + 0 -
      AssertDirection(directions, 1, 0, 1);     //  45    0  45      + + 0 +     xya     - - 0 -

      AssertDirection(directions, 0, -1, -1);   //   0  -45 -45      + 0 - -     xza     - 0 + +
      AssertDirection(directions, 0, -1, 1);    //   0  -45  45      + 0 - +     xza     - 0 + -
      AssertDirection(directions, 0, 1, -1);    //   0   45 -45      + 0 + -     xza     - 0 - +
      AssertDirection(directions, 0, 1, 1);     //   0   45  45      + 0 + +     xza     - 0 - -

      AssertDirection(directions, 2, -1, -1);   //  90  -45 -45      0 + - -     yza     0 - + +
      AssertDirection(directions, 2, -1, 1);    //  90  -45  45      0 + - +     yza     0 - + -
      AssertDirection(directions, 2, 1, -1);    //  90   45 -45      0 + + -     yza     0 - - +
      AssertDirection(directions, 2, 1, 1);     //  90   45  45      0 + + +     yza     0 - - -

      // remaining directions 4d, 3 variations: 16 = 2^4
      AssertDirection(directions, -1, -1, -1);   // -45 -45 -45      + - - -     xyza    - + + +    
      AssertDirection(directions, 1, -1, -1);    //  45 -45 -45      + + - -     xyza    - - + +
      AssertDirection(directions, -1, 1, -1);    // -45  45 -45      + - + -     xyza    - + - +
      AssertDirection(directions, 1, 1, -1);     //  45  45 -45      + + + -     xyza    - - - +    
      AssertDirection(directions, -1, -1, 1);    // -45 -45  45      + - - +     xyza    - + + -    
      AssertDirection(directions, 1, -1, 1);     //  45 -45  45      + + - +     xyza    - - + -
      AssertDirection(directions, -1, 1, 1);     // -45  45  45      + - + +     xyza    - + - -
      AssertDirection(directions, 1, 1, 1);      //  45  45  45      + + + +     xyza    - - - -    

      Assert.Empty(directions);
    }

    [Fact]
    public void UsedDirectionsCount_TwoAngles()
    {
      var directions = HypersphericalAngleQuantified.GetNonLinearDirections(anglesCount: 2, resolution: 2).ToList();

      Assert.Equal(1, GetDirection(directions, 0, 0).UsedCartesianDirectionsCount);
      Assert.Equal(1, GetDirection(directions, 2, 0).UsedCartesianDirectionsCount);
      Assert.Equal(1, GetDirection(directions, 0, 2).UsedCartesianDirectionsCount);
      Assert.Equal(1, GetDirection(directions, 0, 0).UsedCartesianDirectionsCount);

      Assert.Equal(2, GetDirection(directions, 1, 0).UsedCartesianDirectionsCount);
      Assert.Equal(2, GetDirection(directions, -1, 0).UsedCartesianDirectionsCount);
      Assert.Equal(2, GetDirection(directions, 0, 1).UsedCartesianDirectionsCount);

      Assert.Equal(3, GetDirection(directions, 1, 1).UsedCartesianDirectionsCount);
    }

    [Fact]
    public void UsedDirectionsCount_ThreeAngles()
    {
      var directions = HypersphericalAngleQuantified.GetNonLinearDirections(anglesCount: 3, resolution: 2).ToList();

      Assert.Equal(1, GetDirection(directions, 0, 0, 0).UsedCartesianDirectionsCount);
      Assert.Equal(1, GetDirection(directions, 2, 0, 0).UsedCartesianDirectionsCount);
      Assert.Equal(1, GetDirection(directions, 0, 2, 0).UsedCartesianDirectionsCount);
      Assert.Equal(1, GetDirection(directions, 0, 0, 2).UsedCartesianDirectionsCount);

      Assert.Equal(2, GetDirection(directions, 1, 0, 0).UsedCartesianDirectionsCount);
      Assert.Equal(2, GetDirection(directions, 0, 1, 0).UsedCartesianDirectionsCount);
      Assert.Equal(2, GetDirection(directions, 0, 2, 1).UsedCartesianDirectionsCount);

      Assert.Equal(3, GetDirection(directions, 0, 1, 1).UsedCartesianDirectionsCount);
      Assert.Equal(3, GetDirection(directions, 2, 1, 1).UsedCartesianDirectionsCount);
      Assert.Equal(3, GetDirection(directions, 1, 1, 0).UsedCartesianDirectionsCount);
      Assert.Equal(3, GetDirection(directions, 1, 0, 1).UsedCartesianDirectionsCount);

      Assert.Equal(4, GetDirection(directions, 1, 1, 1).UsedCartesianDirectionsCount);
    }
  }
}
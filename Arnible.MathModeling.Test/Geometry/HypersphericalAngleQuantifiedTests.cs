﻿using System;
using System.Collections.Generic;
using Xunit;
using static Arnible.MathModeling.xunit.AssertNumber;

namespace Arnible.MathModeling.Geometry.Test
{
  public class HypersphericalAngleQuantifiedTests
  {
    private static void AssertDirection(List<HypersphericalAngleQuantified> directions, params sbyte[] expected)
    {
      uint? pos = directions.IndexOf(d => expected.SequenceEqual(d.Angles));
      directions.RemoveAt((int)pos.Value);
    }

    private static HypersphericalAngleQuantified GetDirection(List<HypersphericalAngleQuantified> directions, params sbyte[] expected)
    {
      uint? pos = directions.IndexOf(d => expected.SequenceEqual(d.Angles));
      return directions[(int)pos.Value];
    }

    [Fact]
    public void ToVector_Single()
    {
      var directions = HypersphericalAngleQuantified.GetQuantifiedDirections(anglesCount: 1, resolution: 2).ToArray();

      AreEqual(new HypersphericalAngleVector(-1 * Math.PI / 4), directions.Where(d => d.Angles.Single() == -1).Single().ToAngleVector());
      AreEqual(new HypersphericalAngleVector(0), directions.Where(d => d.Angles.Single() == 0).Single().ToAngleVector());
      AreEqual(new HypersphericalAngleVector(Math.PI / 4), directions.Where(d => d.Angles.Single() == 1).Single().ToAngleVector());
      AreEqual(new HypersphericalAngleVector(Math.PI / 2), directions.Where(d => d.Angles.Single() == 2).Single().ToAngleVector());
    }

    [Fact]
    public void GetQuantifiedDirections_OneAngle()
    {
      var directions = new List<HypersphericalAngleQuantified>(HypersphericalAngleQuantified.GetQuantifiedDirections(anglesCount: 1, resolution: 2));

      // axis directions 1d: 4 = 2*2
      AssertDirection(directions, 0);        //   0     + 0    x
      AssertDirection(directions, 2);        //  90     0 +    y

      // remaining directions 2d, 1 variation: 4 = 2^2
      AssertDirection(directions, -1);       //  -45    + -    xy
      AssertDirection(directions, 1);        //   45    + +    xy

      IsEmpty(directions);
      
    }

    [Fact]
    public void GetAllDirectionChangePositive_OneAngle()
    {
      var directions = new List<HypersphericalAngleQuantified>(HypersphericalAngleQuantified.GetQuantifiedDirections(anglesCount: 1, resolution: 2));
      var allChanged = GetDirection(directions, 1);
      AreEqual(allChanged, HypersphericalAngleQuantified.GetAllDirectionChangePositive(anglesCount: 1));
    }

    [Fact]
    public void GetQuantifiedDirections_TwoAngles()
    {
      var directions = new List<HypersphericalAngleQuantified>(HypersphericalAngleQuantified.GetQuantifiedDirections(anglesCount: 2, resolution: 2));

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

      IsEmpty(directions);
    }

    [Fact]
    public void GetAllDirectionChangePositive_TwoAngle()
    {
      var directions = new List<HypersphericalAngleQuantified>(HypersphericalAngleQuantified.GetQuantifiedDirections(anglesCount: 2, resolution: 2));
      var allChanged = GetDirection(directions, 1, 1);
      AreEqual(allChanged, HypersphericalAngleQuantified.GetAllDirectionChangePositive(anglesCount: 2));
    }

    [Fact]
    public void GetQuantifiedDirections_ThreeAngles()
    {
      var directions = new List<HypersphericalAngleQuantified>(HypersphericalAngleQuantified.GetQuantifiedDirections(anglesCount: 3, resolution: 2));

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

      IsEmpty(directions);
    }

    [Fact]
    public void UsedDirectionsCount_TwoAngles()
    {
      var directions = new List<HypersphericalAngleQuantified>(HypersphericalAngleQuantified.GetQuantifiedDirections(anglesCount: 2, resolution: 2));

      AreEqual(1, GetDirection(directions, 0, 0).UsedCartesianDirectionsCount);
      AreEqual(1, GetDirection(directions, 2, 0).UsedCartesianDirectionsCount);
      AreEqual(1, GetDirection(directions, 0, 2).UsedCartesianDirectionsCount);
      AreEqual(1, GetDirection(directions, 0, 0).UsedCartesianDirectionsCount);

      AreEqual(2, GetDirection(directions, 1, 0).UsedCartesianDirectionsCount);
      AreEqual(2, GetDirection(directions, -1, 0).UsedCartesianDirectionsCount);
      AreEqual(2, GetDirection(directions, 0, 1).UsedCartesianDirectionsCount);

      AreEqual(3, GetDirection(directions, 1, 1).UsedCartesianDirectionsCount);
    }

    [Fact]
    public void UsedDirectionsCount_ThreeAngles()
    {
      var directions = new List<HypersphericalAngleQuantified>(HypersphericalAngleQuantified.GetQuantifiedDirections(anglesCount: 3, resolution: 2));

      AreEqual(1, GetDirection(directions, 0, 0, 0).UsedCartesianDirectionsCount);
      AreEqual(1, GetDirection(directions, 2, 0, 0).UsedCartesianDirectionsCount);
      AreEqual(1, GetDirection(directions, 0, 2, 0).UsedCartesianDirectionsCount);
      AreEqual(1, GetDirection(directions, 0, 0, 2).UsedCartesianDirectionsCount);

      AreEqual(2, GetDirection(directions, 1, 0, 0).UsedCartesianDirectionsCount);
      AreEqual(2, GetDirection(directions, 0, 1, 0).UsedCartesianDirectionsCount);
      AreEqual(2, GetDirection(directions, 0, 2, 1).UsedCartesianDirectionsCount);

      AreEqual(3, GetDirection(directions, 0, 1, 1).UsedCartesianDirectionsCount);
      AreEqual(3, GetDirection(directions, 2, 1, 1).UsedCartesianDirectionsCount);
      AreEqual(3, GetDirection(directions, 1, 1, 0).UsedCartesianDirectionsCount);
      AreEqual(3, GetDirection(directions, 1, 0, 1).UsedCartesianDirectionsCount);

      AreEqual(4, GetDirection(directions, 1, 1, 1).UsedCartesianDirectionsCount);
    }

    [Fact]
    public void GetQuantifiedDirectionsNotOrthogonal_OneAngle()
    {
      var directions = new List<HypersphericalAngleQuantified>(HypersphericalAngleQuantified.GetQuantifiedDirectionsNotOrthogonal(anglesCount: 1, resolution: 2));

      // remaining directions 2d, 1 variation: 4 = 2^2
      AssertDirection(directions, -1);       //  -45    + -    xy
      AssertDirection(directions, 1);        //   45    + +    xy

      IsEmpty(directions);
    }
  }
}
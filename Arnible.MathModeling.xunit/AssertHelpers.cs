using System;
using System.Collections.Generic;
using Arnible.MathModeling.Algebra;
using Arnible.MathModeling.Geometry;

namespace Arnible.MathModeling.xunit
{
  public static class AssertHelpers
  {
    public static void AreEquals<T>(
      IEnumerable<T> expected,
      in ValueArray<T> actual) where T : struct, IEquatable<T>
    {
      AssertNumber.AreEquals(expected, actual.GetInternalEnumerable());
    }

    public static void IsEmpty<T>(in ValueArray<T> value) where T : struct, IEquatable<T>
    {
      AssertNumber.IsEmpty(value.GetInternalEnumerable());
    }

    public static void AreEquals(
      IEnumerable<double> expected,
      in NumberVector actual)
    {
      AssertNumber.AreEquals(expected, actual.GetInternalEnumerable());
    }

    public static void AreEquals(
      NumberVector expected,
      in NumberVector actual)
    {
      AssertNumber.AreEquals(expected.GetInternalEnumerable(), actual.GetInternalEnumerable());
    }

    //
    // Algebra
    //

    public static void AreEquals(
      IEnumerable<Number> expected,
      in NumberTranslationVector actual)
    {
      AssertNumber.AreEquals(expected, actual.GetInternalEnumerable());
    }

    //
    // Geometry
    //

    public static void AreEquals(
      IEnumerable<Number> expected,
      in HypersphericalAngleVector actual)
    {
      AssertNumber.AreEquals(expected, actual.GetInternalEnumerable());
    }

    public static void AreEquals(
      IEnumerable<Number> expected,
      in HypersphericalAngleTranslationVector actual)
    {
      AssertNumber.AreEquals(expected, actual.GetInternalEnumerable());
    }
  }
}
using System;

namespace Arnible.MathModeling.Algebra
{
  public readonly struct TranslationVectorScaling : IComparable<TranslationVectorScaling>
  {
    public NumberTranslationVector Translation { get; }
    
    public Number ScalingFactor { get; }

    public TranslationVectorScaling(in NumberTranslationVector translation, in Number scalingFactor)
    {
      if (scalingFactor < 0 || scalingFactor > 1)
      {
        throw new ArgumentException($"scalingFactor: {scalingFactor.ToString()}");
      }

      Translation = translation * scalingFactor;
      ScalingFactor = scalingFactor;
    }

    public TranslationVectorScaling(in NumberTranslationVector translation)
    {
      Translation = translation;
      ScalingFactor = 1;
    }
    
    //
    // IComparable<TranslationVectorScaling>
    //

    public int CompareTo(TranslationVectorScaling other)
    {
      int bScaling = ScalingFactor.CompareTo(other.ScalingFactor);
      if (bScaling != 0)
      {
        return 1 - bScaling;  // the greater scaling factor the better
      }

      return Translation.CompareTo(other.Translation);
    }
    
    //
    // Properties
    //

    public bool IsEmpty => Translation == default;

    public bool IsScaled => !IsEmpty && ScalingFactor < 1;
  }
}
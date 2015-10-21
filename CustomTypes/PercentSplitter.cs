using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

namespace UnityUtils.CustomTypes
{
  [Serializable]
  public struct Percent<T>
  {
    public T Object;
    public int Value;
  }

  [Serializable]
  public struct PercentSplitter<T>
  {
    public Percent<T>[] Percents;

    public Dictionary<T, int> Values
    {
      get { return Percents.ToDictionary(x => x.Object, x => x.Value); }
    }
  }

  [System.AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
  public sealed class PercentSplitterAttribute : PropertyAttribute
  {
    public readonly Type ObjectType;
    public readonly int Split;
    public readonly int Min;
    public readonly int Max;

    public PercentSplitterAttribute(Type objectType, int split, int min = 0, int max = 100)
    {
      ObjectType = objectType;
      Split = split;
      Min = min;
      Max = max;
    }
  }

}

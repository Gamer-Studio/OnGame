using System;
using UnityEngine;
using UnityEngine.Events;

namespace OnGame.Utils
{
  [Serializable]
  public class RangedStat
  {
    public UnityEvent<int, int> onChanged;

    [SerializeField] [GetSet("Value")] private int value;
    [SerializeField] private Stat<int> max;

#if UNITY_EDITOR
    [SerializeField] [GetSet("Max")] private int maxValue;
#endif

    public RangedStat(int maxValue, int value, StatOperator<int> maxOper = null)
    {
      Max = maxValue;
      Value = value;
      MaxOper = maxOper;
    }

    public RangedStat(int maxValue) : this(maxValue, maxValue)
    {
    }

    public int Value
    {
      get => value;
      set
      {
        this.value = Math.Max(0, Math.Min(value, max));
        onChanged?.Invoke(this.value, max);
      }
    }

    public int Max
    {
      get => max;
      set => max.baseValue = value;
    }

    public StatOperator<int> MaxOper
    {
      get => max.oper;
      set => max.oper = value;
    }
  }
}
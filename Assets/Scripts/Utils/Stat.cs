using System;

namespace OnGame.Utils
{
  [Serializable]
  public class Stat
  {
    public float baseValue;

    public float Value
    {
      get => baseValue;
      set => baseValue = value;
    }
  }
}
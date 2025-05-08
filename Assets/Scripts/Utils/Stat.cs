using System;

namespace OnGame.Utils
{
  public delegate T StatOperator<T>(T origin);
  [Serializable]
  public class Stat<T>
  {
    public T baseValue;
    public T Value => oper(baseValue);

    public StatOperator<T> oper;
    
    public Stat(T baseValue, StatOperator<T> oper)
    {
      this.baseValue = baseValue;
      this.oper = oper;
    }
    
    public static implicit operator T(Stat<T> stat) => stat.Value;
  }
}
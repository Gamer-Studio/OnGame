using System;
using UnityEngine;

namespace OnGame.Worlds
{
  [Serializable]
  public class Stage
  {
    [SerializeField]
    private ThemePack theme;
    
    public ThemePack Theme => theme;
    public Stage(ThemePack theme)
    {
      this.theme = theme;
    }
  }
}
using System;

namespace OnGame.Contents.Items
{
  public class Item
  {
    public ItemData data;
    public ItemType type;
  }

  [Flags]
  public enum ItemType
  {
    Default = 0,
    Active = 1 << 0,
    Passive = 1 << 1,
    All = Active | Passive
  }
}
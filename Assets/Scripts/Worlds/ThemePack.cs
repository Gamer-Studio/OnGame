using OnGame.Contents.Items;
using UnityEngine;

namespace OnGame.Worlds
{
  [CreateAssetMenu(fileName = "new ThemePack", menuName = "OnGame/Theme Pack", order = 0)]
  public class ThemePack : ScriptableObject
  {
    public Room[] roomTable;

    public Room bossRoom;
    // Serializable로 변경 예정
    public Item[] itemTable;
  }
}
using OnGame.Contents.Items;
using UnityEngine;

namespace OnGame.Scenes.World
{
  public class Inventory : MonoBehaviour
  {
    // 배열 크기 절 대 변경하면 안됨
    [SerializeField] private Item[] items = new Item[5];
  }
}
using UnityEngine;

namespace OnGame.Contents.Items
{
  [CreateAssetMenu(fileName = "new ItemData", menuName = "Item/item", order = 0)]
  public class ItemData : ScriptableObject
  {
    public string description;
    public Sprite sprite;
    public int price;

    public virtual Item Create()
    {
      var result = new Item();

      return result;
    }
  }
}
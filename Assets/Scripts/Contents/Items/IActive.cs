namespace OnGame.Contents.Items
{
  public interface IActive
  {
    void Use();
    bool ConsumeOnUse { get; set; }
    
    // Part Management
    void AddPart<T>(T part) where T : Item, IPassive;
    // 혹시 모르니까 만들어둔 메소드
    void RemovePart<T>(T part) where T : Item, IPassive;
    Item[] GetParts();
  }
}
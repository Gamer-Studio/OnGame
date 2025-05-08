namespace OnGame.Contents.Items
{
  public interface IActive
  {
    void Use();
    bool ConsumeOnUse { get; set; }
  }
}
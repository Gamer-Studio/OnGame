namespace OnGame.Contents.Items
{
  public interface IPassive
  {
    void Apply();
    bool TryGetValue <T>(string key, out T value);
  }
}
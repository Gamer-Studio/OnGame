using UnityEngine;

namespace OnGame.Scenes.World
{
  public class WorldManager : MonoBehaviour
  {
    public static WorldManager Instance { get; private set; }

    private void Awake()
    {
      if (Instance == null)
        Instance = this;
      else
        Destroy(gameObject);
    }
  }
}
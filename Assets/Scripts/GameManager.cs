using UnityEngine;

namespace OnGame
{
  public class GameManager : MonoBehaviour
  {
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
      if (Instance == null)
      {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Load();
      }
      else
      {
        Destroy(gameObject);
      }
    }

    public void Load()
    {
    }
  }
}
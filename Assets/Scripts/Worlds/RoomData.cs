using System;
using UnityEngine;

namespace OnGame.Worlds
{
  [CreateAssetMenu(fileName = "new RoomData", menuName = "OnGame/Stage Data", order = 0)]
  public class RoomData : ScriptableObject
  {
    public RoomType type;
    public GameObject originMap;
    public SpawnData[] spawnTable;
  }

  [Serializable]
  public class SpawnData
  {
    public GameObject entityPrefab;
    public int min, max;
    [Range(0, 100)]
    public int probability;
  }

  public enum RoomType
  {
    Battle,
    Event,
    Shop,
    Reward
  }
}
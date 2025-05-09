using UnityEngine;
using UnityEngine.Tilemaps;

namespace OnGame.Prefabs.RoomBlueprints
{
  public class RoomBlueprint : MonoBehaviour
  {
    // 여러 레이어에 그릴 경우 레이어 추가해주신 다음 넣어주시면 됩니다.
    public Tilemap[] floors;
    public Tilemap[] structures;
    // 방에 고정적 위치에 소환되는 엔티티
    public GameObject[] entities;
  }
}
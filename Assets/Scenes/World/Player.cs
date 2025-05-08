using OnGame.Prefabs.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OnGame.Scenes.World
{
  public class Player : Entity
  {
    public Vector2 direction;
    public float speed;
    private void Start()
    {
    }
    
    private void OnMove(InputValue value)
    {
    }
  }
}
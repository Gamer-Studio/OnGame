using Cinemachine;
using OnGame.Prefabs.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OnGame.Scenes.World
{
  public class Player : Entity
  {
    public Vector2 direction;
    public float speed = 10;
    public float cameraSpeed = 10;
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private new CinemachineVirtualCamera camera;

    private void Update()
    {
      var scroll = Input.GetAxis("Mouse ScrollWheel");

      if (scroll != 0)
        camera.m_Lens.OrthographicSize = Mathf.Clamp(
          camera.m_Lens.OrthographicSize + scroll > 0 ? +cameraSpeed : -cameraSpeed
          , 10, 100);
    }

    private void FixedUpdate()
    {
      body.AddForce(direction * speed, ForceMode2D.Impulse);
    }

    private void OnMove(InputValue value)
    {
      direction = value.Get<Vector2>();
    }
  }
}
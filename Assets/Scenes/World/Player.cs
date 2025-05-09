using System;
using Cinemachine;
using OnGame.Prefabs.Entities;
using OnGame.Utils;
using UnityEngine;
using UnityEngine.InputSystem;
using OnGame.Utils.PlayerState;

namespace OnGame.Scenes.World
{
  public class Player : Character
  {
    // Component Fields
    [SerializeField] private new CinemachineVirtualCamera camera;
    
    public float CameraSpeed = 10;
    public PlayerStates CurrentState { get; private set; } = PlayerStates.Idle;
    
    // State Machine
    [SerializeField] private State<Player>[] states;
    public StateMachine<Player> StateMachine { get; private set; }

    protected override void Awake()
    {
      base.Awake();
      
      // Sets Player States
      SetUp();
    }

    private void OnMove(InputValue value)
    { 
      movementDirection = value.Get<Vector2>();
    }
    
    private void OnZoom(InputValue value)
    {
      var scroll = value.Get<float>();

      if (scroll != 0)
        camera.m_Lens.OrthographicSize = Mathf.Clamp(
          camera.m_Lens.OrthographicSize + scroll > 0 ? +CameraSpeed : -CameraSpeed, 
          10, 100);
    }
    
    #region Basic Action Rules
    private void SetUp()
    {
      states = new State<Player>[Enum.GetValues(typeof(PlayerStates)).Length];
      for (int i = 0; i < states.Length; i++)
      {
        states[i] = GetState((PlayerStates)i);
      }
      StateMachine = new StateMachine<Player>();
      StateMachine.SetUp(this, states[(int)PlayerStates.Idle]);
    }

    private State<Player> GetState(PlayerStates _state)
    {
      return _state switch
      {
        PlayerStates.Idle => new IdleState(),
        PlayerStates.Move => new MoveState(),
        PlayerStates.Dash => new DashState(),
        PlayerStates.Guard => new GuardState(),
        PlayerStates.Dead => new DeadState(),
        _ => null
      };
    }

    public void ChangeState(PlayerStates _newState)
    {
      CurrentState = _newState;
      StateMachine.ChangeState(states[(int)_newState]);
    }
    #endregion
  }
}
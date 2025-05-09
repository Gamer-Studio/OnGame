using OnGame.Scenes.World;
using UnityEngine;

namespace OnGame.Utils
{
    namespace PlayerState
    {
        public class IdleState : State<Player>
            {
                public override void Enter(Player source)
                {
                    Debug.Log("Changed to idle state");
                    source.Animator.SetBool(source.IsMove, false);
                }
        
                public override void Execute(Player source)
                {
                    
                    if (source.RigidBody.velocity.magnitude <= 0.5f)
                    {
                        source.ChangeState(PlayerStates.Move);
                    }
                }
        
                public override void Exit(Player source)
                {
                    source.Animator.SetBool(source.IsMove, true);
                }
            }
        
            public class MoveState : State<Player>
            {
                public override void Enter(Player source)
                {
                    Debug.Log("Changed to move state");
                    source.Animator.SetBool(source.IsMove, true);
                }
        
                public override void Execute(Player source)
                {
                    if (source.RigidBody.velocity.magnitude > 0.5f)
                    {
                        source.ChangeState(PlayerStates.Idle);
                    }
                }
        
                public override void Exit(Player source)
                {
                    source.Animator.SetBool(source.IsMove, false);
                }
            }
        
            public class DashState : State<Player>
            {
                public override void Enter(Player source)
                {
                    Debug.Log("Changed to dash state");
                }
        
                public override void Execute(Player source)
                {
                    
                }
        
                public override void Exit(Player source)
                {
                    
                }
            }
        
            public class GuardState : State<Player>
            {
                private float originalSpeed;
                private StatOperator<float> guardOperator;
                
                public override void Enter(Player source)
                {
                    Debug.Log("Changed to guard state");
                    originalSpeed = source.Speed;
                    source.Speed /= 2;
                    guardOperator = (x) => x * 5f;
                    source.IsDashing = true;
                    source.DefenseOpers.Add(guardOperator);
                }
        
                public override void Execute(Player source)
                {
                    
                }
        
                public override void Exit(Player source)
                {
                    source.Speed = originalSpeed;
                    source.DefenseOpers.Remove(guardOperator);
                }
            }
        
            public class DeadState : State<Player>
            {
                public override void Enter(Player source)
                {
                    
                }
        
                public override void Execute(Player source)
                {
                    
                }
        
                public override void Exit(Player source)
                {
                    
                }
            }
    }
    
}
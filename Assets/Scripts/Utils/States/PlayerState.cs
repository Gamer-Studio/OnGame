using OnGame.Prefabs.Entities;
using UnityEngine;

namespace OnGame.Utils.States.PlayerState
{
    public class IdleState : State<Character>
    {
        public override void Enter(Character source)
        {
            Debug.Log("Changed to idle state");
            source.Animator.SetBool(source.IsMove, false);
        }

        public override void Execute(Character source)
        {
            if (source.RigidBody.velocity.magnitude >= 0.5f) source.ChangeState(PlayerStates.Move);
        }

        public override void Exit(Character source)
        {
            source.Animator.SetBool(source.IsMove, true);
        }
    }

    public class MoveState : State<Character>
    {
        public override void Enter(Character source)
        {
            Debug.Log("Changed to move state");
            source.Animator.SetBool(source.IsMove, true);
        }

        public override void Execute(Character source)
        {
            if (source.RigidBody.velocity.magnitude < 0.5f) source.ChangeState(PlayerStates.Idle);
        }

        public override void Exit(Character source)
        {
            source.Animator.SetBool(source.IsMove, false);
        }
    }

    public class DashState : State<Character>
    {
        public override void Enter(Character source)
        {
            Debug.Log("Changed to dash state");
        }

        public override void Execute(Character source)
        {
        }

        public override void Exit(Character source)
        {
        }
    }

    public class GuardState : State<Character>
    {
        private StatOperator<float> guardOperator;
        private float originalSpeed;

        public override void Enter(Character source)
        {
            Debug.Log("Changed to guard state");
            originalSpeed = source.Speed;
            source.Speed /= 2;
            guardOperator = x => x + x * 0.8f;
            source.DefenseOpers.Add(guardOperator);
        }

        public override void Execute(Character source)
        {
        }

        public override void Exit(Character source)
        {
            source.Speed = originalSpeed;
            source.DefenseOpers.Remove(guardOperator);
        }
    }

    public class DeadState : State<Character>
    {
        public override void Enter(Character source)
        {
        }

        public override void Execute(Character source)
        {
        }

        public override void Exit(Character source)
        {
        }
    }
}
using System.Collections;
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
            if (source.RigidBody.velocity.magnitude >= 0.1f) source.ChangeState(PlayerStates.Move);
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
            if (source.RigidBody.velocity.magnitude < 0.1f) source.ChangeState(PlayerStates.Idle);
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
            source.IsDashing = true;
        }

        public override void Execute(Character source)
        {
            source.StartCoroutine(WaitUntilDashEnd(source));
        }

        public override void Exit(Character source)
        {
            source.IsDashing = false;
        }

        IEnumerator WaitUntilDashEnd(Character source)
        {
            yield return new WaitForSeconds(0.2f);
            source.ChangeState(PlayerStates.Move);
        }
    }

    public class GuardState : State<Character>
    {
        private StatOperator<float> guardOperator;
        private float originalForce;
        private float originalDrag;

        public override void Enter(Character source)
        {
            Debug.Log("Changed to guard state");
            originalForce = source.MoveForce;
            originalDrag = source.Drag;

            source.RigidBody.drag = originalDrag * 2f;
            source.MoveForce *= 0.3f;
            
            guardOperator = x => x + x * 0.8f;
            source.DefenseOpers.Add(guardOperator);
        }

        public override void Execute(Character source)
        {
            if(source.Mana.Value <= 0) source.ChangeState(PlayerStates.Idle);
            
            // TODO: Mana가 일정량 소모되도록 코드 작성 필요
        }

        public override void Exit(Character source)
        {
            source.MoveForce = originalForce;
            source.RigidBody.drag = originalDrag;
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
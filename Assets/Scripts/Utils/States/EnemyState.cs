using OnGame.Prefabs.Entities;
using UnityEngine;

namespace OnGame.Utils.States.EnemyState
{
    public class PatrolState : State<EnemyCharacter>
    {
        public override void Enter(EnemyCharacter source)
        {
            Debug.Log("Changed to patrol state");
        }

        public override void Execute(EnemyCharacter source)
        {
        }

        public override void Exit(EnemyCharacter source)
        {
        }
    }

    public class ChaseState : State<EnemyCharacter>
    {
        public override void Enter(EnemyCharacter source)
        {
            Debug.Log("Changed to chase state");
        }

        public override void Execute(EnemyCharacter source)
        {
        }

        public override void Exit(EnemyCharacter source)
        {
        }
    }

    public class AttackState : State<EnemyCharacter>
    {
        public override void Enter(EnemyCharacter source)
        {
            Debug.Log("Changed to attack state");
        }

        public override void Execute(EnemyCharacter source)
        {
        }

        public override void Exit(EnemyCharacter source)
        {
        }
    }

    public class DeadState : State<EnemyCharacter>
    {
        public override void Enter(EnemyCharacter source)
        {
            Debug.Log("Changed to dead state");
        }

        public override void Execute(EnemyCharacter source)
        {
        }

        public override void Exit(EnemyCharacter source)
        {
        }
    }
}
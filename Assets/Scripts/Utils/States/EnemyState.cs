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
            if (source.Target == null) return;
            if (DistanceToTarget(source.transform.position, source.Target.position) <= source.MaxDistanceToTarget)
            {
                source.ChangeState(EnemyStates.Chase);
            }
        }

        public override void Exit(EnemyCharacter source)
        {
        }

        private float DistanceToTarget(Vector3 origin, Vector3 target)
        {
            return Vector3.Distance(origin, target);
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
            if (DistanceToTarget(source.transform.position, source.Target.position) > source.MaxDistanceToTarget)
            {
                source.ChangeState(EnemyStates.Patrol);
            }
            else if (DistanceToTarget(source.transform.position, source.Target.position) <= source.AttackRange)
            {
                source.ChangeState(EnemyStates.Attack);
            }
        }

        public override void Exit(EnemyCharacter source)
        {
        }
        
        private float DistanceToTarget(Vector3 origin, Vector3 target)
        {
            return Vector3.Distance(origin, target);
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
            source.IsAlive = true;
        }
    }
}
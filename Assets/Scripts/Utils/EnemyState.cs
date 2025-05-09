using OnGame.Prefabs.Entities;
using UnityEngine;

namespace OnGame.Utils
{
    namespace EnemyState
    {
        public class PatrolState : State<Enemy>
        {
            public override void Enter(Enemy source)
            { 
                Debug.Log("Changed to patrol state");
            }
        
            public override void Execute(Enemy source) 
            {
                        
            }
            public override void Exit(Enemy source) 
            {
                        
            }
        }
        
        public class ChaseState : State<Enemy>
        { 
            public override void Enter(Enemy source)
            { 
                Debug.Log("Changed to chase state");
            }
        
            public override void Execute(Enemy source)
            {
                
            }
        
            public override void Exit(Enemy source)
            {
                        
            }
        }
        
        public class AttackState : State<Enemy>
        { 
            public override void Enter(Enemy source)
            { 
                Debug.Log("Changed to attack state");
            }
        
            public override void Execute(Enemy source)
            {
                        
            }
        
            public override void Exit(Enemy source)
            {
                        
            }
        }
        
        public class DeadState : State<Enemy>
        {
            public override void Enter(Enemy source)
            { 
                Debug.Log("Changed to dead state");
            }
        
            public override void Execute(Enemy source)
            {
                        
            }
        
            public override void Exit(Enemy source)
            {
                        
            }
        }
    }
}
using System.Collections.Generic;
using OnGame.Utils;
using UnityEngine;

namespace OnGame.Prefabs.Entities
{
    public class Entity : MonoBehaviour
    {
        // Config Fields
        [Header("Configs")] 
        [Range(1f, 100f)] [SerializeField] protected float speed = 20f;
        [Range(0f, 20f)] [SerializeField] protected float drag = 10f;
        [Range(1f, 100f)] [SerializeField] protected float moveForce = 30f;

        // Stat. Fields
        [Header("Stats")] 
        [SerializeField] protected Stat<float> attackStat;
        [SerializeField] protected Stat<float> defenseStat;
        [SerializeField] protected RangedStat health;
        [SerializeField] protected RangedStat mana;
        [SerializeField] protected RangedStat experience;
        public Stat<float> CriticalMultiplier;
        public Stat<float> CriticalPossibility;
        public List<StatOperator<float>> AttackOpers = new();
        public List<StatOperator<float>> DefenseOpers = new();
        
        // Component Fields
        [Header("RigidBody Components")]
        [SerializeField] protected Rigidbody2D rigidBody;

        // Properties
        public Rigidbody2D RigidBody => rigidBody;
        public float Speed { get => speed; set => speed = value; }
        public float Drag => drag;
        public float MoveForce => moveForce;

        protected virtual void Update()
        {
            HandleAction();
        }

        public virtual void Init()
        {
            attackStat = new Stat<float>(10f, x =>
            {
                var originalValue = x;
                foreach(var oper in AttackOpers) originalValue = oper(originalValue);
                return originalValue;
            });
            defenseStat = new Stat<float>(5f, x =>
            {
                var originalValue = x;
                foreach (var oper in DefenseOpers) originalValue = oper(originalValue);
                return originalValue;
            });

            health = new RangedStat(100, 100);
            mana = new RangedStat(50, 50);
            experience = new RangedStat(100, 0);
            CriticalMultiplier = new Stat<float>(1.5f, null);
            CriticalPossibility = new Stat<float>(0.1f, null);
        }

        protected virtual void HandleAction()
        {
        }
    }
}
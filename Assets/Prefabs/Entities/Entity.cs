using System.Collections.Generic;
using OnGame.Utils;
using UnityEngine;

namespace OnGame.Prefabs.Entities
{
    public class Entity : MonoBehaviour
    {
        // Config Fields
        [Header("Configs")] 
        [Range(1f, 100f)] [SerializeField] protected float speed = 5f;
        [Range(0f, 10f)] [SerializeField] protected float drag;

        // Stat. Fields
        [Header("Stats")] 
        [SerializeField] protected Stat<float> attackStat;
        [SerializeField] protected Stat<float> defenseStat;
        [SerializeField] protected RangedStat health;
        [SerializeField] protected RangedStat mana;
        [SerializeField] protected RangedStat experience;
        public Stat<float> CriticalMultiplier;
        public Stat<float> CriticalPossibility;
        public List<StatOperator<float>> DefenseOpers = new();
        
        // Component Fields
        [SerializeField] protected Rigidbody2D rigidBody;

        // Properties
        public Rigidbody2D RigidBody => rigidBody;

        public float Speed
        {
            get => speed;
            set => speed = value;
        }

        public Stat<float> DefendStatProp => defenseStat;

        protected virtual void Update()
        {
            HandleAction();
        }

        protected virtual void Init()
        {
            attackStat = new Stat<float>(10f, null);
            defenseStat = new Stat<float>(5f, x =>
            {
                var originalValue = x;
                foreach (var oper in DefenseOpers) originalValue = oper(originalValue);
                return originalValue;
            });

            health = new RangedStat(100);
            mana = new RangedStat(50);
            CriticalMultiplier = new Stat<float>(1.5f, null);
            CriticalPossibility = new Stat<float>(0.1f, null);
            experience = new RangedStat(100, 0);
        }

        protected virtual void HandleAction()
        {
        }

        protected virtual void Rotate(Vector2 direction)
        {
        }
    }
}
using System;
using System.Collections.Generic;
using OnGame.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace OnGame.Prefabs.Entities
{
    public class Entity : MonoBehaviour
    {
        // Config Fields
        [Header("Configs")] 
        [Range(1f, 100f)] [SerializeField] protected float speed = 5f;
        [Range(0f, 10f)] [SerializeField] protected float drag = 0f;

        // Component Fields
        [Header("Components")]
        [SerializeField] protected Rigidbody2D rigidBody;

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
        

        // Physics Fields
        [Header("Physics")]
        [SerializeField] protected Vector2 lookAtDirection = Vector2.zero;
        [SerializeField] protected Vector2 movementDirection = Vector2.zero;

        // Properties
        public Rigidbody2D RigidBody => rigidBody;
        public Vector2 MovementDirection => movementDirection;
        public Vector2 LookAtDirection => lookAtDirection;
        public float Speed { get => speed; set => speed = value; }
        public Stat<float> DefendStatProp => defenseStat;

        protected virtual void Awake()
        {
            rigidBody = Helper.GetComponent_Helper<Rigidbody2D>(gameObject);
        }

        protected virtual void Update()
        {
            HandleAction();
        }

        protected virtual void FixedUpdate()
        {
            Movement(movementDirection);
        }

        protected virtual void Init()
        {
            attackStat = new Stat<float>(10f, null);
            defenseStat = new Stat<float>(5f, (x) =>
            {
                var originalValue = x;
                foreach (var oper in DefenseOpers)
                {
                    originalValue = oper(originalValue);
                }
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

        protected virtual void Movement(Vector2 direction)
        {
            direction *= speed;
            rigidBody.velocity = direction;
        }

        protected virtual void Rotate(Vector2 direction) { }
    }
}
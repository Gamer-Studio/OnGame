using System;
using OnGame.Utils;
using UnityEngine;

namespace OnGame.Prefabs.Entities
{
    public class Entity : MonoBehaviour
    {
        [Range(1f, 100f)] [SerializeField] protected float speed = 5f;
        [Range(0f, 10f)] [SerializeField] protected float drag = 0f;

        protected Rigidbody2D rigidBody;
        
        protected Vector2 movementDirection = Vector2.zero;
        protected Vector2 lookAtDirection = Vector2.zero;
        protected bool isAttacking;
        protected bool isInteracting;
        
        private float timeSinceLastAttack = float.MaxValue;
        
        public Vector2 MovementDirection => movementDirection;
        public Vector2 LookAtDirection => lookAtDirection;
        public bool IsAttacking => isAttacking;
        public bool IsInteracting{ get=> isInteracting; set=> isInteracting = value; }

        protected virtual void Awake()
        {
          rigidBody = Helper.GetComponent_Helper<Rigidbody2D>(gameObject);
        }
        
        protected virtual void Start() { }

        protected virtual void Update()
        {
          HandleAction();
        }

        protected virtual void FixedUpdate()
        {
          Movement(movementDirection);
        }

        protected virtual void HandleAction()
        {
          
        }

        protected virtual void Movement(Vector2 direction)
        {
          direction *= speed;
          rigidBody.velocity = direction;
        }
        
        protected virtual void Rotate(Vector2 direction)
        {
            
        }
        
        protected virtual void Attack() { }

        public virtual void Die()
        {
          rigidBody.velocity = Vector2.zero;
        }
    }
}
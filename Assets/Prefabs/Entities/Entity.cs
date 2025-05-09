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
        
        public Vector2 MovementDirection => movementDirection;
        public Vector2 LookAtDirection => lookAtDirection;

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

        protected virtual void HandleAction() { }

        protected virtual void Movement(Vector2 direction)
        {
          direction *= speed;
          rigidBody.velocity = direction;
        }
        
        protected virtual void Rotate(Vector2 direction) { }
    }
}
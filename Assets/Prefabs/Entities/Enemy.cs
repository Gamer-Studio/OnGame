using UnityEngine;

namespace OnGame.Prefabs.Entities
{
    public class Enemy : MonoBehaviour
    {
        // Character Field
        [Header("Character Entity")]
        [SerializeField] private EnemyCharacter character;
        
        // Physics Fields
        [Header("Physics")] 
        [SerializeField] protected Vector2 lookAtDirection = Vector2.zero;
        [SerializeField] protected Vector2 movementDirection = Vector2.zero;
        
        private Rigidbody2D rigidBody;
        private float speed;
        private float drag;
        private float moveForce;
        
        // Properties
        public Vector2 MovementDirection => movementDirection;
        public Vector2 LookAtDirection => lookAtDirection;
        
        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// </summary>
        private void Start()
        {
            rigidBody = character.RigidBody;
            speed = character.Speed;
            drag = character.Drag;
            rigidBody.drag = drag;
            moveForce = character.MoveForce;
            
            character.Init();
        }
        
        /// <summary>
        /// Update is called every frame if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            Rotate(LookAtDirection);
        }
        
        /// <summary>
        /// Character Rotation Action
        /// </summary>
        /// <param name="direction"></param>
        private void Rotate(Vector2 direction)
        {
            var rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            var confDirection = rotZ switch
            {
                > 45f and < 135f => Direction.North,
                < -45f and > -135f => Direction.South,
                >= 135f or <= -135f => Direction.West,
                _ => Direction.East
            };

            // Movement Animation
            character.Animator.SetInteger(character.Angle, (int)confDirection);
        }
    }
}
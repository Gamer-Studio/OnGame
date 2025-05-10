using Cinemachine;
using OnGame.Prefabs.Entities;
using OnGame.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OnGame.Scenes.World
{
    public class Player : MonoBehaviour
    {
        // Character Field
        [Header("Character Entity")]
        [SerializeField] private Character character;

        // Component Fields
        [Header("Components")] [SerializeField]
        private CinemachineVirtualCamera cam;
        [Range(0.1f, 1f)] [SerializeField] private float zoomRate = 1f;
        [Range(0.1f, 2f)] [SerializeField] private float zoomSpeed = 1f;
        [Range(1f, 10f)] [SerializeField] private float minZoom = 1f;
        [Range(1f, 20f)] [SerializeField] private float maxZoom = 5f;
        
        // Physics Fields
        [Header("Physics")] 
        [SerializeField] protected Vector2 lookAtDirection = Vector2.zero;
        [SerializeField] protected Vector2 movementDirection = Vector2.zero;
        
        private Rigidbody2D rigidBody;
        private float speed;
        private float drag;
        private float moveForce;
        private float currentZoom;
        private float newZoom;
        private float scroll;

        // Properties
        public Vector2 MovementDirection => movementDirection;
        public Vector2 LookAtDirection => lookAtDirection;
        public Character Character => character;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// </summary>
        private void Start()
        {
            character.Init();
            
            rigidBody = character.RigidBody;
            speed = character.Speed;
            drag = character.Drag;
            rigidBody.drag = drag;
            moveForce = character.MoveForce;
            currentZoom = cam.m_Lens.OrthographicSize;
            newZoom = currentZoom;
        }

        /// <summary>
        /// Update is called every frame if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            Rotate(LookAtDirection);
            CalculateCamZoom();
        }

        private void FixedUpdate()
        {
            Movement(MovementDirection);
        }

        /// <summary>
        /// Change camera zoom with lerp
        /// </summary>
        private void LateUpdate()
        {
            currentZoom = Mathf.Lerp(currentZoom, newZoom, Time.deltaTime * zoomSpeed);
            cam.m_Lens.OrthographicSize = currentZoom;
        }

        /// <summary>
        /// Character Movement Action
        /// </summary>
        /// <param name="direction"></param>
        private void Movement(Vector2 direction)
        {
            if(rigidBody.velocity.magnitude > speed)
            {
                rigidBody.velocity *= (speed / rigidBody.velocity.magnitude);    
            }

            rigidBody.AddForce(direction.normalized * moveForce, ForceMode2D.Force);
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

        /// <summary>
        /// Calculate Camera Zoom Action
        /// </summary>
        private void CalculateCamZoom()
        {
            var camHeight = cam.m_Lens.OrthographicSize;
            var camWidth = camHeight * cam.m_Lens.Aspect;

            var maxCamHeight = 100f / 2f;
            var maxCamWidth = 150f / 2f / cam.m_Lens.Aspect;
            if (Mathf.Abs(scroll) > 0.01f)
            {
                newZoom = currentZoom - Mathf.Clamp(scroll * zoomRate, -1f, 1f);
                Debug.Log(newZoom);
                newZoom = Mathf.Clamp(newZoom, minZoom, maxZoom);
                scroll = 0;
            }
        }

        #region Input Actions

        private void OnMove(InputValue value)
        {
            if (character.IsDashing) return;
            movementDirection = value.Get<Vector2>();
        }

        private void OnLook(InputValue value)
        {
            var mousePosition = value.Get<Vector2>();
            var activeCamera = CinemachineCore.Instance.GetActiveBrain(0).OutputCamera;
            Vector2 worldPos = activeCamera.ScreenToWorldPoint(mousePosition);

            lookAtDirection = (worldPos - (Vector2)transform.position).normalized;
        }

        private void OnZoom(InputValue value)
        {
            scroll = value.Get<float>();
            CalculateCamZoom();
        }

        private void OnDash(InputValue value)
        {
            if (!character.IsDashAvailable) return;
            
            var val = value.Get<float>();
            if (val <= 0f) return;
            character.OnDash();
            if(movementDirection == Vector2.zero) 
                rigidBody.AddForce(lookAtDirection * character.Speed * 10f, ForceMode2D.Impulse);
            else rigidBody.AddForce(movementDirection * character.Speed * 10f, ForceMode2D.Impulse);
        }

        private void OnFire(InputValue value)
        {
            var val = value.Get<float>();
            Debug.Log(val);
            character.IsAttacking = value.Get<float>() > 0;
        }

        private void OnGuard(InputValue value)
        {
            var val = value.Get<float>();
            Debug.Log(val);
            character.ChangeState(val > 0 ? PlayerStates.Guard : PlayerStates.Idle);
        }
        
        #endregion
    }
}
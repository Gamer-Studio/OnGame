#nullable enable
using System;
using OnGame.Utils;
using UnityEngine;

namespace OnGame.Prefabs.Entities
{
    public enum Direction{South, East, North, West}
    
    public class Character : Entity
    {
        // Const Fields
        private static readonly int Angle = Animator.StringToHash("Direction");
        private static readonly int IsDamage = Animator.StringToHash("IsDamage");
        public readonly int IsMove = Animator.StringToHash("IsMove");
        
        // Component Fields
        private Animator animator;
        
        // State Fields
        [Header("State")]
        [SerializeField][GetSet("IsInvincible")] private bool isInvincible;
        [SerializeField][GetSet("IsAlive")] private bool isAlive = true;
        [SerializeField][GetSet("IsAttacking")] private bool isDashing;
        [SerializeField][GetSet("IsInteracting")] private bool isAttacking;
        [SerializeField][GetSet("IsDashing")] private bool isInteracting;
        
        // Cooldown Fields
        protected float TimeSinceLastAttack = float.MaxValue; 
        protected float TimeSinceLastInvincible = float.MaxValue;
        protected float TimeSinceLastDashed = float.MaxValue;
        protected float AttackDelay = 0.5f;
        protected float DashCoolTime = 20f;
        protected float InvincibleTimeDelay = 0.5f;
        private bool isDashAvailable = true;
        
        // Stats Fields
        private float originalSpeed;
        
        // Properties
        public bool IsInvincible { get => isInvincible; set => isInvincible = value; }
        public bool IsAlive { get => isAlive; set => isAlive = value; }
        public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
        public bool IsInteracting{ get=> isInteracting; set=> isInteracting = value; }
        public bool IsDashing{ get=> isDashing; set=> isDashing = value; }
        public Animator Animator => animator;
        
        // Action event
        public event Action? OnDeath; 
        
        protected override void Awake()
        {
            base.Awake();
            animator = Helper.GetComponentInChildren_Helper<Animator>(gameObject, true);
        }

        protected override void FixedUpdate()
        {
            if(!isAlive || isDashing) return;    
            base.FixedUpdate();
        }
        
        protected override void Update()
        {
            base.Update();
            Rotate(MovementDirection);
            HandleAttackDelay();
            HandleDashCoolTime();
            HandleInvincibleTimeDelay();
        }

        protected override void Rotate(Vector2 direction)
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
            animator.SetInteger(Angle, (int)confDirection);
        }

        protected virtual void HandleAttackDelay()
        {
            if (TimeSinceLastAttack <= AttackDelay){ TimeSinceLastAttack += Time.deltaTime; }
            if (isAttacking && TimeSinceLastAttack > AttackDelay)
            {
                TimeSinceLastAttack = 0;
                OnAttack();
            }
        }

        protected virtual void HandleInvincibleTimeDelay()
        {
            if (!isInvincible) return;
            
            if(TimeSinceLastInvincible <= InvincibleTimeDelay){ TimeSinceLastInvincible += Time.deltaTime; }
            else { isInvincible = false; TimeSinceLastInvincible = 0; }
        }

        protected virtual void HandleDashCoolTime()
        {
            if (isDashAvailable) return;

            if (TimeSinceLastDashed <= DashCoolTime){ TimeSinceLastDashed += Time.deltaTime; }
            else { isDashAvailable = true; TimeSinceLastDashed = 0; }
        }
        
        protected virtual void OnEarnExp(int exp) { }
        
        protected virtual void OnLevelUp() { }

        protected virtual void OnAttack() { }

        protected virtual void OnDash()
        {
            if (!isAlive || !isDashAvailable) return;

            isDashAvailable = false;
            isInvincible = true;
            TimeSinceLastInvincible = 0;
            TimeSinceLastDashed = 0;
        }

        protected virtual void OnGuard()
        {
            if (!isAlive) return;
        }
        
        protected virtual void OnDamage(float damage)
        {
            if (!isAlive || isInvincible) return;
            
            var calculatedDamage = damage * (1f - defenseStat.Value / 100f);
            health.Value -= Mathf.CeilToInt(calculatedDamage);

            isInvincible = true;
            TimeSinceLastInvincible = 0f;
        }
        

        protected virtual void OnHealthRecover(int coef)
        {
            if (!isAlive) return;
            health.Value += coef;
        }

        protected virtual void OnManaConsume(int coef)
        {
            if (!isAlive) return;
            mana.Value -= coef;
        }

        protected virtual void OnManaRecover(int coef)
        {
            if (!isAlive) return;
            mana.Value += coef;
        }

        public virtual void Die()
        {
            isAlive = false;
            rigidBody.velocity = Vector2.zero;
            
            OnDeath?.Invoke();
        }
    }
}

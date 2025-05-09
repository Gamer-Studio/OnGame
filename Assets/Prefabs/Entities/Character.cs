#nullable enable
using System;
using OnGame.Utils;
using OnGame.Utils.States.PlayerState;
using UnityEngine;

namespace OnGame.Prefabs.Entities
{
    public enum Direction
    {
        South,
        East,
        North,
        West
    }

    public class Character : Entity
    {   // Const Fields
        public readonly int Angle = Animator.StringToHash("Direction");
        public readonly int IsDamage = Animator.StringToHash("IsDamage");
        public readonly int IsMove = Animator.StringToHash("IsMove");
        // Component Fields
        [Header("Components")]
        [SerializeField] protected Animator animator;
        
        // State Fields
        [Header("State")] 
        [SerializeField] [GetSet("IsInvincible")] private bool isInvincible;
        [SerializeField] [GetSet("IsAlive")] private bool isAlive = true;
        [SerializeField] [GetSet("IsAttacking")] private bool isDashing;
        [SerializeField] [GetSet("IsInteracting")] private bool isAttacking;
        [SerializeField] [GetSet("IsDashing")] private bool isInteracting;
        
        // Stats Fields
        private float originalSpeed;
        
        // Cooldown Fields
        protected float TimeSinceLastAttack = float.MaxValue;
        protected float TimeSinceLastDashed = float.MaxValue;
        protected float TimeSinceLastInvincible = float.MaxValue;
        protected float AttackDelay = 0.5f; 
        protected float DashCoolTime = 20f;
        protected float InvincibleTimeDelay = 0.5f; 
        private bool isDashAvailable = true;

        // Properties
        public bool IsInvincible { get => isInvincible; set => isInvincible = value; }
        public bool IsAlive { get => isAlive; set => isAlive = value; }
        public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
        public bool IsInteracting { get => isInteracting; set => isInteracting = value; }
        public bool IsDashing { get => isDashing; set => isDashing = value; }
        public Animator Animator => animator;
        
        // State Machine
        private State<Character>[] states;
        public PlayerStates CurrentState { get; private set; } = PlayerStates.Idle;
        public StateMachine<Character> StateMachine { get; private set; }

        private void Awake()
        {
            // Sets Player States
            SetUp();
        }

        protected override void Update()
        {
            base.Update();
            HandleAttackDelay();
            HandleDashCoolTime();
            HandleInvincibleTimeDelay();

            // State Machine
            StateMachine.Execute();
        }

        // Action event
        public event Action? OnDeath;

        private void HandleAttackDelay()
        {
            if (TimeSinceLastAttack <= AttackDelay) TimeSinceLastAttack += Time.deltaTime;
            if (isAttacking && TimeSinceLastAttack > AttackDelay)
            {
                TimeSinceLastAttack = 0;
                OnAttack();
            }
        }

        private void HandleInvincibleTimeDelay()
        {
            if (!isInvincible) return;

            if (TimeSinceLastInvincible <= InvincibleTimeDelay)
            {
                TimeSinceLastInvincible += Time.deltaTime;
            }
            else
            {
                isInvincible = false;
                TimeSinceLastInvincible = 0;
            }
        }

        private void HandleDashCoolTime()
        {
            if (isDashAvailable) return;

            if (TimeSinceLastDashed <= DashCoolTime)
            {
                TimeSinceLastDashed += Time.deltaTime;
            }
            else
            {
                isDashAvailable = true;
                TimeSinceLastDashed = 0;
            }
        }

        private void OnEarnExp(int exp)
        {
        }

        private void OnLevelUp()
        {
        }

        private void OnAttack()
        {
        }

        private void OnDash()
        {
            if (!isAlive || !isDashAvailable) return;

            isDashAvailable = false;
            isInvincible = true;
            TimeSinceLastInvincible = 0;
            TimeSinceLastDashed = 0;
        }

        private void OnGuard()
        {
            if (!isAlive) return;
        }

        private void OnDamage(float damage)
        {
            if (!isAlive || isInvincible) return;

            var calculatedDamage = damage * (1f - defenseStat.Value / 100f);
            health.Value -= Mathf.CeilToInt(calculatedDamage);

            isInvincible = true;
            TimeSinceLastInvincible = 0f;
        }


        private void OnHealthRecover(int coef)
        {
            if (!isAlive) return;
            health.Value += coef;
        }

        private void OnManaConsume(int coef)
        {
            if (!isAlive) return;
            mana.Value -= coef;
        }

        private void OnManaRecover(int coef)
        {
            if (!isAlive) return;
            mana.Value += coef;
        }

        private void Die()
        {
            isAlive = false;
            rigidBody.velocity = Vector2.zero;

            OnDeath?.Invoke();
        }

        #region Basic Action Rules

        private void SetUp()
        {
            states = new State<Character>[Enum.GetValues(typeof(PlayerStates)).Length];
            for (var i = 0; i < states.Length; i++) states[i] = GetState((PlayerStates)i);
            StateMachine = new StateMachine<Character>();
            StateMachine.SetUp(this, states[(int)PlayerStates.Idle]);
        }

        private State<Character> GetState(PlayerStates state)
        {
            return state switch
            {
                PlayerStates.Idle => new IdleState(),
                PlayerStates.Move => new MoveState(),
                PlayerStates.Dash => new DashState(),
                PlayerStates.Guard => new GuardState(),
                PlayerStates.Dead => new DeadState(),
                _ => new IdleState()
            };
        }

        public void ChangeState(PlayerStates newState)
        {
            CurrentState = newState;
            StateMachine.ChangeState(states[(int)newState]);
        }

        #endregion
    }
}
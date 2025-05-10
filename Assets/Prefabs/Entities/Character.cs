#nullable enable
using System;
using OnGame.Utils;
using OnGame.Utils.States;
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

    public enum StatTypes
    {
        Health,
        Mana, 
        Attack, 
        Defense, 
        CriticalMultiplier, 
        CriticalPossibility,
    }

    public class Character : Entity
    {   
        // Const Fields
        public readonly int Angle = Animator.StringToHash("Direction");
        public readonly int IsDamage = Animator.StringToHash("IsDamage");
        public readonly int IsMove = Animator.StringToHash("IsMove");
        
        // Component Fields
        [Header("Components")]
        [SerializeField] protected Animator animator;
        
        // State Fields
        [Header("States")] 
        [SerializeField] [GetSet("IsInvincible")] private bool isInvincible;
        [SerializeField] [GetSet("IsAlive")] private bool isAlive = true;
        [SerializeField] [GetSet("IsAttacking")] private bool isAttacking = false;
        [SerializeField] [GetSet("IsInteracting")] private bool isInteracting;
        [SerializeField] [GetSet("IsDashing")] private bool isDashing;
        
        // Stats Fields
        private int availablePoint;
        
        // Cooldown Fields
        [Header("Cooldowns")]
        [SerializeField] private float timeSinceLastAttack = float.MaxValue;
        [SerializeField] private float timeSinceLastDashed = float.MaxValue;
        [SerializeField] private float timeSinceLastInvincible = float.MaxValue;
        [SerializeField] private float attackDelay = 0.5f; 
        [SerializeField] private float dashCoolTime = 5f;
        [SerializeField] private float invincibleTimeDelay = 0.5f; 
        

        // Properties
        public bool IsInvincible { get => isInvincible; set => isInvincible = value; }
        public bool IsAlive { get => isAlive; set => isAlive = value; }
        public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
        public bool IsInteracting { get => isInteracting; set => isInteracting = value; }
        public bool IsDashing { get => isDashing; set => isDashing = value; }
        public bool IsDashAvailable { get; private set; } = true;
        public Animator Animator => animator;
        
        // State Machine
        private State<Character>[] states;
        public PlayerStates CurrentState { get; private set; } = PlayerStates.Idle;
        public StateMachine<Character> StateMachine { get; private set; }
        
        // Action event
        public event Action? OnDeath;
        
        private void Awake()
        {
            if (animator == null) animator = Helper.GetComponentInChildren_Helper<Animator>(gameObject);
            
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
        
        private void HandleAttackDelay()
        {
            if (!isAttacking) return;

            if (timeSinceLastAttack <= attackDelay)
            {
                timeSinceLastAttack += Time.deltaTime;
            }
            else
            {
                timeSinceLastAttack = 0;
                OnAttack();
            }
        }

        private void HandleInvincibleTimeDelay()
        {
            if (!isInvincible) return;

            if (timeSinceLastInvincible <= invincibleTimeDelay)
            {
                timeSinceLastInvincible += Time.deltaTime;
            }
            else
            {
                isInvincible = false;
                timeSinceLastInvincible = 0;
            }
        }

        private void HandleDashCoolTime()
        {
            if (IsDashAvailable) return;

            if (timeSinceLastDashed <= dashCoolTime)
            {
                timeSinceLastDashed += Time.deltaTime;
            }
            else
            {
                IsDashAvailable = true;
                timeSinceLastDashed = 0;
            }
        }

        private void OnEarnExp(int exp)
        {
            experience.Value += exp;
        }

        private void OnLevelUp()
        {
            availablePoint++;
            health.Value += health.Max;
            mana.Value += mana.Max;
            experience.Value -= experience.Max;
            MaxExperienceOpers.Add(x => x + 50);
        }

        public void OnStatusChange(StatTypes statType)
        {
            if (availablePoint <= 0) return;
            availablePoint--;
            switch (statType)
            {
                case StatTypes.Health:
                    MaxHealthOpers.Add(x => x + 50);
                    break;
                case StatTypes.Mana:
                    MaxManaOpers.Add(x => x + 25);
                    break;
                case StatTypes.Attack:
                    AttackOpers.Add(x => x + 5);
                    break;
                case StatTypes.Defense:
                    DefenseOpers.Add(x => x + 5);
                    break;
                case StatTypes.CriticalMultiplier:
                    CriticalMultiplierOpers.Add(x => x + 0.1f);
                    break;
                case StatTypes.CriticalPossibility:
                    CriticalPossibilityOpers.Add(x => x + 0.1f);
                    break;
            }
        }

        private void OnAttack()
        {
        }

        public void OnDash()
        {
            if (!isAlive || !IsDashAvailable || mana.Value <= 0) return;

            IsDashAvailable = false;
            isInvincible = true;
            timeSinceLastInvincible = 0;
            timeSinceLastDashed = 0;
            
            ChangeState(PlayerStates.Dash);
        }

        private void OnGuard()
        {
            if (!isAlive) return;
            
            ChangeState(PlayerStates.Guard);
        }

        public void OnDamage(float damage)
        {
            if (!isAlive || isInvincible) return;

            var calculatedDamage = damage * (1f - defenseStat.Value / 100f);
            health.Value -= Mathf.CeilToInt(calculatedDamage);

            isInvincible = true;
            timeSinceLastInvincible = 0f;
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

        /// <summary>
        /// State Machine Setup
        /// </summary>
        private void SetUp()
        {
            states = new State<Character>[Enum.GetValues(typeof(PlayerStates)).Length];
            for (var i = 0; i < states.Length; i++) states[i] = GetState((PlayerStates)i);
            StateMachine = new StateMachine<Character>();
            StateMachine.SetUp(this, states[(int)PlayerStates.Idle]);
        }

        /// <summary>
        /// PlayerState 기준에 따라 어떤 작업을 수행할 것인지 정해주는 작업
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
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

        /// <summary>
        /// State Change가 필요할 때 호출하는 함수
        /// </summary>
        /// <param name="newState"></param>
        public void ChangeState(PlayerStates newState)
        {
            if (CurrentState == newState) return;
            CurrentState = newState;
            StateMachine.ChangeState(states[(int)newState]);
        }

        #endregion
    }
}
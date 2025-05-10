using System;
using System.Collections;
using System.Collections.Generic;
using OnGame.Prefabs.Entities;
using OnGame.Utils;
using OnGame.Utils.States;
using OnGame.Utils.States.EnemyState;
using UnityEngine;

namespace OnGame
{
    public class EnemyCharacter : Entity
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
        
        // Stats Fields
        private float originalSpeed;
        
        // Cooldown Fields
        [Header("Cooldowns")]
        [SerializeField] private float timeSinceLastAttack = float.MaxValue;
        [SerializeField] private float timeSinceLastInvincible = float.MaxValue;
        [SerializeField] private float attackDelay = 0.5f; 
        [SerializeField] private float invincibleTimeDelay = 0.5f; 
        

        // Properties
        public bool IsInvincible { get => isInvincible; set => isInvincible = value; }
        public bool IsAlive { get => isAlive; set => isAlive = value; }
        public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
        public Animator Animator => animator;
        
        // State Machine
        private State<EnemyCharacter>[] states;
        public EnemyStates CurrentState { get; private set; } = EnemyStates.Patrol;
        public StateMachine<EnemyCharacter> StateMachine { get; private set; }
        
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

        private void OnAttack()
        {
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
            states = new State<EnemyCharacter>[Enum.GetValues(typeof(EnemyStates)).Length];
            for (var i = 0; i < states.Length; i++) states[i] = GetState((EnemyStates)i);
            StateMachine = new StateMachine<EnemyCharacter>();
            StateMachine.SetUp(this, states[(int)EnemyStates.Patrol]);
        }

        /// <summary>
        /// PlayerState 기준에 따라 어떤 작업을 수행할 것인지 정해주는 작업
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private State<EnemyCharacter> GetState(EnemyStates state)
        {
            return state switch
            {
                EnemyStates.Patrol => new PatrolState(),
                EnemyStates.Chase => new ChaseState(),
                EnemyStates.Attack => new AttackState(),
                EnemyStates.Dead => new DeadState(),
                _ => new PatrolState()
            };
        }

        /// <summary>
        /// State Change가 필요할 때 호출하는 함수
        /// </summary>
        /// <param name="newState"></param>
        public void ChangeState(EnemyStates newState)
        {
            if (CurrentState == newState) return;
            CurrentState = newState;
            StateMachine.ChangeState(states[(int)newState]);
        }

        #endregion
    }
}

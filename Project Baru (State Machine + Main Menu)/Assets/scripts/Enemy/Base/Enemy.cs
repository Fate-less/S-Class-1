using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IEnemymoveable, ITriggerCheckable
{
    [field: SerializeField] public float MaxHealth { get; set; } = 10f;

    public float CurrentHealth { get; set; }
    public Rigidbody2D RB { get; set; }
    public bool IsFacingRight { get; set; } = true;

    #region State Machine Variables
    public EnemyStateMachine StateMachine { get; set; }
    public EnemyIdleState IdleState { get; set; }
    public EnemyChaseState ChaseState { get; set; }
    public EnemyAttackState AttackState { get; set; }
    public EnemyRecoveryState RecoveryState { get; set; }
    public EnemyLowState LowState { get; set; }
    public SpriteRenderer SpriteRenderer { get; set; }

    public bool IsAgroed { get; set; }
    public bool IsWithinStrikingDistance { get; set; }

    #endregion

    #region Idle variables

    public GameObject BulletPrefab;
    public float RandomMovementRange = 5f;
    public float RandomMovementSpeed = 1f;

    #endregion

    private void Awake()
    {
        StateMachine = new EnemyStateMachine();

        IdleState = new EnemyIdleState(this, StateMachine);
        ChaseState = new EnemyChaseState(this, StateMachine);
        AttackState = new EnemyAttackState(this, StateMachine);
        RecoveryState = new EnemyRecoveryState(this, StateMachine);
        LowState = new EnemyLowState(this, StateMachine);
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void Start()
    {
        CurrentHealth = MaxHealth;

        RB = GetComponent<Rigidbody2D>();

        StateMachine.initialize(IdleState);
    }

    private void Update()
    {
        StateMachine.CurrentEnemyState.FrameUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentEnemyState.PhysicsUpdate();
    }

    #region Health / Die Functions
    public void Damage(float damageAmount)
    {
        CurrentHealth -= damageAmount;

        if (CurrentHealth <= 0f)
        {
            Die();
            return;
        }

        if (CurrentHealth < MaxHealth * 0.5f)
        {
            StateMachine.ChangeState(LowState);
        }
        else if (CurrentHealth < MaxHealth * 0.8f)
        {
            StateMachine.ChangeState(RecoveryState);
        }
    }
    public void Die()
    {
        Destroy(gameObject);
    }

    #endregion
    #region Movement Functions

    

    public void MoveEnemy(Vector2 velocity)
    {
        RB.velocity = velocity;
        CheckForLeftorRightFacing(velocity);
    }

    public void CheckForLeftorRightFacing(Vector2 velocity)
    {
        if (IsFacingRight && velocity.x < 0f)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;
        }

        else if (IsFacingRight && velocity.x > 0f)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;
        }
    }

    #endregion

    private void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        StateMachine.CurrentEnemyState.AnimationTriggerEvent(triggerType);
    }

    #region Distance Checks
    public void SetAgroStatus(bool isAgroed)
    {
        IsAgroed = isAgroed;
    }

    public void SetStrikingDistanceBool(bool isWithinStrikingDistance)
    {
        IsWithinStrikingDistance = isWithinStrikingDistance;
    }

    #endregion

    public enum AnimationTriggerType
    {
        EnemyDamaged,
        PlayFootstepSound
    }
}

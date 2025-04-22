using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Enemy_Shady : Enemy
{
    [Header("Shady spesifics")]
    public float battleStateMoveSpeed;

    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float growSpeed;
    [SerializeField] private float maxSize;


    #region States

    public ShadyIdleState idleState { get; private set; }
    public ShadyMoveState moveState { get; private set; }
    public ShadyDeadState deadState { get; private set; }
    public ShadyStunnedState stunnedState { get; private set; }
    public ShadyBatttleState battleState { get; private set; }
    #endregion

    
    protected override void Awake()
    {
        base.Awake();

        idleState = new ShadyIdleState(this, stateMachine, "Idle", this);
        moveState = new ShadyMoveState(this, stateMachine, "Move", this);
        deadState = new ShadyDeadState(this, stateMachine, "Dead", this);
        stunnedState = new ShadyStunnedState(this, stateMachine, "Stunned", this);
        battleState = new ShadyBatttleState(this, stateMachine, "MoveFast", this);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }

        return false;
    }

    public override void Die()
    {
        base.Die();
        PlayerManager.instance.iShadyKilled += 1;
        stateMachine.ChangeState(deadState);
    }

    public override void AnimationSpecialAttaackTrigger()
    {
        GameObject newExplosive= Instantiate(explosionPrefab, attackCheck.position, Quaternion.identity);
        newExplosive.GetComponent<Explosive_Controller>().SetupExplosive(stats,growSpeed,maxSize,attackCheckRadius);

        cd.enabled = false;
        rb.gravityScale = 0;
    }

    public void SelDestroy() => Destroy(gameObject);
}

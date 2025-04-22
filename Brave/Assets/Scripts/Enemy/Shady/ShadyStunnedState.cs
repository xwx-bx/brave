using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyStunnedState : EnemyState
{
    private Enemy_Shady enemy;

    public ShadyStunnedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_Shady _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.fx.InvokeRepeating("RedColorBlink", 0, .1f);

        stateTimer = enemy.stunDuration;

        rb.velocity = new Vector2(-enemy.facingDir * enemy.stunDirection.x, enemy.stunDirection.y);
    }

    public override void Exit()
    {
        base.Exit();


        enemy.stats.MakeInvincible(false);
    }

    public override void Update()
    {
        base.Update();

        if (rb.velocity.y < .1f && enemy.IsGroundDetected())
        {
            enemy.fx.Invoke("CancelColorChange", 0);
            enemy.stats.MakeInvincible(true);
        }

        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.deadState);
    }
}

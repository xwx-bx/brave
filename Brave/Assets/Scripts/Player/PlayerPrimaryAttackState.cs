using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{

    public int comboCounter { get; set; }

    private float lastTimeAttacked;
    private float comboWindow = 2;

    public PlayerPrimaryAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        xInput = Input.GetAxisRaw("Horizontal");

        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
            comboCounter = 0;

        player.anim.SetInteger("ComboCounter", comboCounter);


        float attackDir = player.facingDir;

        if(xInput != 0)
            attackDir = xInput;

        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y);


        stateTimer = .1f;
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", .15f);

        comboCounter++;
        lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            player.SetZeroVelocity();

        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}

using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerDeadState : PlayerState
{
    private float z = 0;
    public PlayerDeadState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();
        z = 0;

        AudioManager.instance.PlaySFX(6, null);

        GameObject.Find("Canvas").GetComponent<UI>().SwitchOnEndScreen();
    }

    public override void Exit()
    {
        base.Exit();

    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();

        AudioManager.instance.StopAllBGM();

        if (z >= 1)
        {
            AudioManager.instance.StopAllSFX();
        }
        else
        {
            z=z+Time.deltaTime;
        }
        

    }

   
}

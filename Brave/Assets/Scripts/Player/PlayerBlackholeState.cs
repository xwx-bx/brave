using UnityEngine;

public class PlayerBlackholeState : PlayerState
{
    private float flyTime = 0.4f;
    private bool skillUsed;


    private float defaultGravity;

    public PlayerBlackholeState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();
        player.transform.Find("Intity_Status_UI").gameObject.SetActive(false);
        player.skill.dash.setDashUnlocked(false);
        defaultGravity = player.rb.gravityScale;
        skillUsed = false;
        stateTimer = flyTime;
        rb.gravityScale = 0;
    }

    public override void Exit()
    {
        base.Exit();
        player.transform.Find("Intity_Status_UI").gameObject.SetActive(true);
        player.skill.dash.setDashUnlocked(true);
        player.rb.gravityScale = defaultGravity;
        player.fx.MakeTransprent(false);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
            rb.velocity = new Vector2(0, 15);
        if (stateTimer < 0)
        {
            rb.velocity = new Vector2(0, -.1f);

            if (!skillUsed)
            {
                if (player.skill.blackhole.CanUseSkill())
                    skillUsed = true;
            }
        }
        if (player.skill.blackhole.SkillCompleted())
            stateMachine.ChangeState(player.airState);
    }
}

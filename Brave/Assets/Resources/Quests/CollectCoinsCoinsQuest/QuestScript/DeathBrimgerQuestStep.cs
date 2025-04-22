using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBrimgerQuestStep : QuestStep
{
    private int iKillCountCollected = 0;
    [SerializeField] private int iKillCountToComplete = 6;
    private int x = 0;
    private int y = 0;

    protected override void SetQuestStepState(string state)
    {
        this.y = System.Int32.Parse(state);
        UpdateState();
    }

    private void Start()
    {
        x = 0;
    }

    private void Update()
    {

        if (PlayerManager.instance.player.stats.x == 0)
        {
            if (x == 0)
            {
                iKillCountCollected = PlayerManager.instance.iDeathBringerKilled;
                x = 1;
            }
            if (PlayerManager.instance.iDeathBringerKilled > iKillCountCollected)
            {
                y += PlayerManager.instance.iDeathBringerKilled - iKillCountCollected;
                UpdateState();
            }
            iKillCountCollected = PlayerManager.instance.iDeathBringerKilled;

            if (y >= iKillCountToComplete)
            {
                FinishQuestStep();
            }
        }
    }

    private void UpdateState()
    {
        string state = y.ToString();
        ChangeState(state);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonKilledQuestStep : QuestStep
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
                iKillCountCollected = PlayerManager.instance.iSkeletonKilled;
                x = 1;
            }
            if (PlayerManager.instance.iSkeletonKilled > iKillCountCollected)
            {
                y += PlayerManager.instance.iSkeletonKilled - iKillCountCollected;
                UpdateState();
            }
            iKillCountCollected = PlayerManager.instance.iSkeletonKilled;

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

using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCoinsQuestStep : QuestStep
{
    private int coinsCollected = 0;
    [SerializeField] private int coinsToComplete = 100;
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
        
        if ( PlayerManager.instance.player.stats.x == 0)
        {
            if (x == 0)
            {
                coinsCollected = PlayerManager.instance.currency;
                x = 1;
            }
            if (PlayerManager.instance.currency > coinsCollected)
            {
                y += PlayerManager.instance.currency - coinsCollected;
                UpdateState();
            }
            coinsCollected = PlayerManager.instance.currency;

            if (y >= coinsToComplete)
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

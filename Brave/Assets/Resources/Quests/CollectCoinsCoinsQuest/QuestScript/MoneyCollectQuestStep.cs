using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyCollectQuestStep : QuestStep
{
    [SerializeField] private int coinsToComplete = 100;
   

    protected override void SetQuestStepState(string state)
    {
        UpdateState();
    }

    

    private void Update()
    {

        if(PlayerManager.instance.currency >= coinsToComplete)
        {
            FinishQuestStep();
        }

          
        
    }

    private void UpdateState()
    {
        string state = PlayerManager.instance.currency.ToString();
        ChangeState(state);
    }
}


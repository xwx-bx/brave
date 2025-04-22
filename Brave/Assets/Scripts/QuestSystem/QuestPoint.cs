using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class QuestPoint : MonoBehaviour
{

    [Header("Quest")]
    [SerializeField] private QuestInfoSO questInfoForPoint;

    [Header("Config")]
    [SerializeField] private bool startPoint = true;
    [SerializeField] private bool finishPoint = true;

    private bool playerIsNear = false;

    private string questId;

    private QuestState currentQuestState;

    private QuestIcon questIcon;

    private void Awake()
    {
        questId = questInfoForPoint.id;
        questIcon = GetComponentInChildren<QuestIcon>();
        
    }

    private void Start()
    {
        GameManager.instance.questEvents.onQuestStateChange += QuestStateChange;
    }

    private void OnEnable()
    {
        if(GameManager.instance!= null)
             GameManager.instance.questEvents.onQuestStateChange += QuestStateChange;
    }

    private void OnDisable()
    {
       GameManager.instance.questEvents.onQuestStateChange -= QuestStateChange;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            SubmitPressed();
        }
        
    }

    private void SubmitPressed()
    {
        if (!playerIsNear)
        {
            return;
        }

        if(currentQuestState.Equals(QuestState.CAN_START) && startPoint && Time.timeScale!= 0)
        {
            PlayerManager.instance.player.fx.CreatePopUpText("接受任务");
            GameManager.instance.questEvents.StartQuest(questId);
        }
        else if(currentQuestState.Equals(QuestState.CAN_FINISH) && finishPoint)
        {
            GameManager.instance.questEvents.FinishQuest(questId);
        }
        
    }

    private void QuestStateChange(Quest quest)
    {
        if(quest.info.id.Equals(questId))
        {
            currentQuestState = quest.state;
            questIcon.SetState(currentQuestState, startPoint, finishPoint);
        }
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if(otherCollider.CompareTag("Player"))
        {
            playerIsNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D otherCollider)
    {
        if(otherCollider.CompareTag("Player"))
        {
            playerIsNear = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestIcon : MonoBehaviour
{
    [Header("Icons")]
    [SerializeField] private GameObject requirementNotMetToStartIcon;
    [SerializeField] private GameObject canStartIcon;
    [SerializeField] private GameObject requirementNotMetToFinishIcon;
    [SerializeField] private GameObject canFinishIcon;

    public void SetState(QuestState newState,bool startPoint, bool finishPoint)
    {
        requirementNotMetToStartIcon.SetActive(false);
        canStartIcon.SetActive(false);
        requirementNotMetToFinishIcon.SetActive(false);
        canFinishIcon.SetActive(false);

        switch (newState)
        {
            case QuestState.REQUIREMENT_NOT_MET:
                if (startPoint)
                {
                    requirementNotMetToStartIcon.SetActive(true);
                }
                break;
            case QuestState.CAN_START:
                if (startPoint)
                {
                    canStartIcon.SetActive(true);
                }
                break;
            case QuestState.IN_PROGRESS:
                if (finishPoint)
                {
                    requirementNotMetToFinishIcon.SetActive(true);
                }
                break;
            case QuestState.CAN_FINISH:
                if (finishPoint)
                {
                    canFinishIcon.SetActive(true);
                }
                break;
            case QuestState.FINISHED:
                break;
           default:
                Debug.LogWarning("Quest State not recognized by switch statement for quest icon: " + newState);
                break;
        }
    }
}

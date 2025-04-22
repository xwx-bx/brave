using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    [Header("Config")]
    [SerializeField] private bool loadQuestState = true;

    private Dictionary<string, Quest> questMap;

    private int x = 0;

    [SerializeField] private GameObject dropPrefab;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
        questMap = CreateQuestMap();
    }

    private void OnEnable()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.questEvents.onStartQuest += StartQuest;
            GameManager.instance.questEvents.onAdvanceQuest += AdvanceQuest;
            GameManager.instance.questEvents.onFinishQuest += FinishQuest;
            GameManager.instance.questEvents.onQuestStepStateChange += QuestStepStateChange;
        }
    }
    private void OnDisable()
    {
        GameManager.instance.questEvents.onStartQuest -= StartQuest;
        GameManager.instance.questEvents.onAdvanceQuest -= AdvanceQuest;
        GameManager.instance.questEvents.onFinishQuest -= FinishQuest;
        GameManager.instance.questEvents.onQuestStepStateChange -= QuestStepStateChange;
    }

    private void Start()
    {
        GameManager.instance.questEvents.onStartQuest += StartQuest;
        GameManager.instance.questEvents.onAdvanceQuest += AdvanceQuest;
        GameManager.instance.questEvents.onFinishQuest += FinishQuest;
        GameManager.instance.questEvents.onQuestStepStateChange += QuestStepStateChange;
        foreach (Quest quest in questMap.Values)
        {
            if (quest.state == QuestState.IN_PROGRESS)
            {
                quest.InstantiateCurrentQuestStep(this.transform);
            }

            GameManager.instance.questEvents.QuestStateChange(quest);
        }

    }

    private void ChangeQuestState(string id, QuestState state)
    {
        Quest quest = GetQuestById(id);
        quest.state = state;
        GameManager.instance.questEvents.QuestStateChange(quest);
    }

    private bool CheckRequirementsMet(Quest quest)
    {
        bool meetRequirements = true;

        foreach (QuestInfoSO prerequisiteQuestInfo in quest.info.questPrerequisites)
        {
            if (GetQuestById(prerequisiteQuestInfo.id).state != QuestState.FINISHED)
            {
                meetRequirements = false;
            }
        }
        return meetRequirements;
    }

    private void Update()
    {
        foreach (Quest quest in questMap.Values)
        {
            if (quest.state == QuestState.REQUIREMENT_NOT_MET && CheckRequirementsMet(quest))
            {
                ChangeQuestState(quest.info.id, QuestState.CAN_START);
            }
        }

        if (PlayerManager.instance.player.stats.isDead && x == 0)
        {
            Save();
            x = 1;
        }
        if (PlayerManager.instance.player.stats.isDead == false)
        {
            x = 0;
        }
    }
    private void StartQuest(string id)
    {
        Quest quest = GetQuestById(id);
        quest.InstantiateCurrentQuestStep(this.transform);
        ChangeQuestState(quest.info.id, QuestState.IN_PROGRESS);
    }

    private void AdvanceQuest(string id)
    {
        Quest quest = GetQuestById(id);

        quest.MoveToNextStep();

        if (quest.CurrentStepExists())
        {
            quest.InstantiateCurrentQuestStep(this.transform);
        }
        else
        {
            ChangeQuestState(quest.info.id, QuestState.CAN_FINISH);
        }
    }

    private void FinishQuest(string id)
    {
        Quest quest = GetQuestById(id);
        ClaimRewards(quest);
        ChangeQuestState(quest.info.id, QuestState.FINISHED);
        PlayerManager.instance.player.fx.CreatePopUpText("完成任务");
        PlayerManager.instance.iQuestCompleted += 1;
    }

    private void ClaimRewards(Quest quest)
    {
        PlayerManager.instance.currency += quest.info.goldReward;
        foreach (ItemData itemData in quest.info.possibleDrop)
        {
            DropItem(itemData, quest);
        }
    }

    protected void DropItem(ItemData _itemData, Quest quest)
    {
        GameObject newDrop = Instantiate(dropPrefab, PlayerManager.instance.player.transform.position, Quaternion.identity);

        Vector2 randomVelocity = new Vector2(UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(15, 20));


        newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomVelocity);
    }

    private void QuestStepStateChange(string id, int stepIndex, QuestStepState questStepState)
    {
        Quest quest = GetQuestById(id);
        quest.StoreQuestStepState(questStepState, stepIndex);
        ChangeQuestState(id, quest.state);
    }

    private Dictionary<string, Quest> CreateQuestMap()
    {
        QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>("Quests");
        Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();
        foreach (QuestInfoSO questInfo in allQuests)
        {
            if (idToQuestMap.ContainsKey(questInfo.id))
            {
                Debug.LogWarning("Duplicate ID found when creating quest map: " + questInfo.id);
            }
            idToQuestMap.Add(questInfo.id, LoadQuest(questInfo));
        }
        return idToQuestMap;
    }

    private Quest GetQuestById(string id)
    {
        Quest quest = questMap[id];
        if (quest == null)
        {
            Debug.LogError("ID not found in the Quest Map; " + id);
        }
        return quest;
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    private void Save()
    {
        foreach (Quest quest in questMap.Values)
        {
            SaveQuest(quest);
        }
    }


    private void SaveQuest(Quest quest)
    {
        try
        {
            QuestData questData = quest.GetQuestData();
            string serializedData = JsonUtility.ToJson(questData);
            PlayerPrefs.SetString(quest.info.id, serializedData);
            //Debug.Log(serializedData);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save quest with id " + quest.info.id + ": " + e);
        }
    }

    private Quest LoadQuest(QuestInfoSO questInfo)
    {
        Quest quest = null;
        try
        {
            if (PlayerPrefs.HasKey(questInfo.id) && loadQuestState)
            {
                string serializedData = PlayerPrefs.GetString(questInfo.id);
                QuestData questData = JsonUtility.FromJson<QuestData>(serializedData);
                quest = new Quest(questInfo, questData.state, questData.questStepIndex, questData.questStepStates);
            }
            else
            {
                quest = new Quest(questInfo);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to load quest with id " + questInfo.id + ": " + e);
        }
        return quest;
    }

    public void ClearQuestData()
    {
        foreach (Quest quest in questMap.Values)
        {
            try
            {
                Quest quest1 = new Quest(quest.info);
                QuestData questData = quest1.GetQuestData();
                string serializedData = JsonUtility.ToJson(questData);
                PlayerPrefs.SetString(quest.info.id, serializedData);
                Debug.Log(serializedData);
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to save quest with id " + quest.info.id + ": " + e);
            }
        }
        questMap = CreateQuestMap();
        Save();
    }



}

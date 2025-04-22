using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public int currency;
    public int iSkeletonKilled;
    public int iSlimeKilled;
    public int iArcherKilled;
    public int iShadyKilled;
    public int iDeathBringerKilled;
    public int iQuestCompleted;
    public bool isGameOver;

    public int timeHours;
    public int timeMinutes;
    public int timeSeconds;


    public SerializableDictionary<string, bool> skillTree;
    public SerializableDictionary<string, int> inventory;
    public List<string> equipmentId;



    public SerializableDictionary<string, bool> checkpoints;
    public string cloestCheckpointId;

    public float lostCurrencyX;
    public float lostCurrencyY;
    public int lostCurrencyAmount;

    public SerializableDictionary<string, float> volumeSettings;

    public int iPlayerLevel;

    public GameData()
    {
        this.lostCurrencyX = 0;
        this.lostCurrencyY = 0;
        this.lostCurrencyAmount = 0;

        this.currency = 0;
        this.iSkeletonKilled = 0;
        this.iSlimeKilled = 0;
        this.iArcherKilled = 0;
        this.iShadyKilled = 0;
        this.iDeathBringerKilled = 0;
        this.iQuestCompleted = 0;
        this.isGameOver = false;

        this.timeHours = 0;
        this.timeMinutes = 0;
        this.timeSeconds = 0;

        this.iPlayerLevel = 0;

        skillTree = new SerializableDictionary<string, bool>();
        inventory = new SerializableDictionary<string, int>();
        equipmentId = new List<string>();

        cloestCheckpointId = string.Empty;
        checkpoints = new SerializableDictionary<string, bool>();

        volumeSettings = new SerializableDictionary<string, float>();
    }
}

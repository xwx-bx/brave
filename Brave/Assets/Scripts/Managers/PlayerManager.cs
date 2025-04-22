using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour, ISaveManager
{
    public static PlayerManager instance;
    public Player player;

    [SerializeField] GameObject gameObject1;

    public int currency;
    public int iSkeletonKilled;
    public int iSlimeKilled;
    public int iArcherKilled;
    public int iShadyKilled;
    public int iDeathBringerKilled;
    public int iQuestCompleted;
    public bool isGameOver;
    public TimeSpan time;
    public int iPlayerLevel;

    private int y = 0;

    private int z = 0;

    private int a = 0;
    public int b = 0;

    private int c = 0;
    private int d = 0;
    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    private void Update()
    {
        b = iQuestCompleted / 7;
        if (b > a)
        {
            z++;
            EnemyStats[] allEnemyStats = FindObjectsOfType<EnemyStats>();
            if (allEnemyStats.Length > 0)
            {
                foreach (EnemyStats enemyStats in allEnemyStats)
                {
                    enemyStats.setLevel(b - a);
                }
            }
            if(z > 1)
            {
                QuestManager.instance.ClearQuestData();
                SaveManager.instance.SaveGame();
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }
                
        }
        a = iQuestCompleted / 7;

        c = iPlayerLevel;
        if (c > d)
        {
            for (int i = 0; i < c-d; i++)
            {

                PlayerManager.instance.player.stats.strength.AddModifier(1);
                PlayerManager.instance.player.stats.agility.AddModifier(1);
                PlayerManager.instance.player.stats.intelligence.AddModifier(1);
                PlayerManager.instance.player.stats.vitality.AddModifier(1);
                PlayerManager.instance.player.stats.currentHealth = PlayerManager.instance.player.stats.GetMaxHealthValue();
                PlayerManager.instance.player.stats.onHealthChanged();
                Inventory.instance.UpdateStatsUI();
            }
        }
        d = iPlayerLevel;

        if (iQuestCompleted == 7 && isGameOver == false)
        {
            isGameOver = true;
            QuestManager.instance.ClearQuestData();
            SaveManager.instance.SaveGame();
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        if (isGameOver && y == 0)
        {
            gameObject1.SetActive(true);
            y = 1;
        }
    }

    public bool HaveEnoughMoney(int _price)
    {
        if (_price > currency)
        {
            return false;
        }

        currency -= _price;
        return true;
    }

    public int GetCurrency() => currency;

    public void LoadData(GameData _data)
    {
        this.currency = _data.currency;
        this.iSkeletonKilled = _data.iSkeletonKilled;
        this.iSlimeKilled = _data.iSlimeKilled;
        this.iArcherKilled = _data.iArcherKilled;
        this.iShadyKilled = _data.iShadyKilled;
        this.iDeathBringerKilled = _data.iDeathBringerKilled;
        this.isGameOver = _data.isGameOver;
        this.iQuestCompleted = _data.iQuestCompleted;
        this.time = new TimeSpan(_data.timeHours, _data.timeMinutes, _data.timeSeconds);
        this.iPlayerLevel = _data.iPlayerLevel;
    }

    public void SaveData(ref GameData _data)
    {
        _data.currency = this.currency;
        _data.iSkeletonKilled = this.iSkeletonKilled;
        _data.iSlimeKilled = this.iSlimeKilled;
        _data.iArcherKilled = this.iArcherKilled;
        _data.iShadyKilled = this.iShadyKilled;
        _data.iDeathBringerKilled = this.iDeathBringerKilled;
        _data.isGameOver = this.isGameOver;
        _data.iQuestCompleted = this.iQuestCompleted;
        _data.timeHours = time.Hours;
        _data.timeMinutes = time.Minutes;
        _data.timeSeconds = time.Seconds;
        _data.iPlayerLevel = this.iPlayerLevel;

    }
}

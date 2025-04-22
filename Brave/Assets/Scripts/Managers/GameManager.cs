using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour, ISaveManager
{
    public static GameManager instance;

    private Transform player;

    [SerializeField] private Checkpoint[] checkpoints;
    [SerializeField] private string closestCheckpointId;

    [Header("Lost currency")]
    [SerializeField] private GameObject lostCurrencyPrefab;
    public int lostCurrencyAmount;
    [SerializeField] private float lostCurrencyX;
    [SerializeField] private float lostCurrencyY;


   
    public QuestEvents questEvents;



    private void Awake()
    {
        checkpoints = FindObjectsOfType<Checkpoint>();
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;

        questEvents = new QuestEvents();
    }

    private void Start()
    {
        player = PlayerManager.instance.player.transform;
    }

    public void RestartScene()
    {
        PlayerManager.instance.player.stats.x = 1;
        SaveManager.instance.SaveGame();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        PlayerManager.instance.player.stats.x = 0;
    }

    public void LoadData(GameData _data) => StartCoroutine(LoadWithDelay(_data));

    private void LoadCheckopints(GameData _data)
    {
        foreach (KeyValuePair<string, bool> pair in _data.checkpoints)
        {
            foreach (Checkpoint checkpoint in checkpoints)
            {
                if (checkpoint.id == pair.Key && pair.Value == true)
                    checkpoint.ActivateCheckpoint();
            }
        }
    }

    private void LoadLostCurrency(GameData _data)
    {
        lostCurrencyAmount = _data.lostCurrencyAmount;
        lostCurrencyX = _data.lostCurrencyX;
        lostCurrencyY = _data.lostCurrencyY;

        if (lostCurrencyAmount > 0)
        {
            ;
            GameObject newlostCurrency = Instantiate(lostCurrencyPrefab, new Vector3(lostCurrencyX, lostCurrencyY), Quaternion.identity);
            newlostCurrency.GetComponent<LostCurrencyController>().currency = lostCurrencyAmount;
        }

        lostCurrencyAmount = 0;
    }

    private IEnumerator LoadWithDelay(GameData _data)
    {
        yield return new WaitForSeconds(.1f);

        LoadCheckopints(_data);
        LoadClosestCheckpoint(_data);
        LoadLostCurrency(_data);
    }

    public void SaveData(ref GameData _data)
    {
        _data.checkpoints.Clear();

        foreach (Checkpoint checkpoint in checkpoints)
        {
            _data.checkpoints.Add(checkpoint.id, checkpoint.activationStatus);
        }

        _data.lostCurrencyAmount = lostCurrencyAmount;
        _data.lostCurrencyX = player.position.x;
        _data.lostCurrencyY = player.position.y;

        var closestCheckpoint = FindClosestCheckpoint();
        if (closestCheckpoint == null)
            return;
        _data.cloestCheckpointId = closestCheckpoint.id;
    }

    private void LoadClosestCheckpoint(GameData _data)
    {
        if (_data.cloestCheckpointId == null)
            return;

        closestCheckpointId = _data.cloestCheckpointId;

        foreach (Checkpoint checkpoint in checkpoints)
        {
            if (checkpoint.id == closestCheckpointId)
                player.position = checkpoint.transform.position;
        }
    }

    private Checkpoint FindClosestCheckpoint()
    {
        float closestDistance = Mathf.Infinity;
        Checkpoint closestCheckpoint = null;

        foreach (var checkpoint in checkpoints)
        {
            float distanceToCheckpoint = Vector2.Distance(player.position, checkpoint.transform.position);

            if (distanceToCheckpoint < closestDistance && checkpoint.activationStatus == true)
            {
                closestDistance = distanceToCheckpoint;
                closestCheckpoint = checkpoint;
            }
        }

        return closestCheckpoint;
    }

    public void PauseGame(bool _pause)
    {
        if (_pause)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    [SerializeField] private string fileName;
    [SerializeField] private bool encryptData;
    private GameData gameData;
    [SerializeField] private List<ISaveManager> saveManagers;
    public FileDataHandler dataHandler;

    private HpBar_ToggleController hpBar_ToggleController;

    [ContextMenu("Delete save flie")]
    public void DeleteSaveData()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        dataHandler.Delete();
    }


    //private void LogToFile(string message)
    //{
    //    System.IO.File.AppendAllText(Application.persistentDataPath + "/log.txt", message + "\n");
    //}

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;

    }

    public void Start()
    {
        hpBar_ToggleController = FindObjectOfType<HpBar_ToggleController>(true);
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        saveManagers = GameObject.FindObjectsOfType(typeof(MonoBehaviour), true)
                          .OfType<ISaveManager>().ToList();

        LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
        
    }

    public void LoadGame()
    {
        if(hpBar_ToggleController != null)
            hpBar_ToggleController.Start();

        
        gameData = dataHandler.Load();

        if (this.gameData == null)
        {
            NewGame();
        }

        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }
        
    }

   

    public void SaveGame()
    {
        int z = 0;
        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
            z++;
        }


        //LogToFile("Before saving: " + JsonUtility.ToJson(gameData) + " " + z); // 添加调试信息
        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    //private List<ISaveManager> FindAllSaveManagers()
    //{
    //    IEnumerable<ISaveManager> SaveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();

    //    return new List<ISaveManager>(SaveManagers);
    //}

    public bool HasSavedData()
    {
        if (dataHandler.Load() != null)
        {
            return true;
        }

        return false;
    }
}

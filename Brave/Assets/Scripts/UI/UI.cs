using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour ,ISaveManager
{
    [Header("End screen")]
    [SerializeField] private UI_FadeScreen fadeScreen;
    [SerializeField] private GameObject endText;
    [SerializeField] private GameObject restartButton;
    [Space]

    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject creftUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private GameObject deadIcon;

    public UI_SkillToolTip skillTooltip;
    public UI_ItemToolTip itemTooltip;
    public UI_StatToolTip statTooltip;
    public UI_CraftWindow craftWindow;

    [SerializeField] private UI_VolumeSlider[] volumeSettings;

    [SerializeField] private GameObject dialogBox;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private TextMeshProUGUI enemysLevel;
    [SerializeField] private TextMeshProUGUI playerLevel;

    [TextArea]
    [SerializeField] private string signTextContent;


    private void Awake()
    {
        SwitchTo(skillTreeUI);
        fadeScreen.gameObject.SetActive(true);
    }

    void Start()
    {
        SwitchTo(inGameUI);

        itemTooltip.gameObject.SetActive(false);
        statTooltip.gameObject.SetActive(false);
    }

    void Update()
    {
        enemysLevel.text = "难度：" + PlayerManager.instance.b;
        playerLevel.text = "等级：" + PlayerManager.instance.iPlayerLevel;
        if (Input.GetKeyDown(KeyCode.C)  && PlayerManager.instance.player.stats.isDead == false)
            SwitchWithKeyTo(characterUI);

        if (Input.GetKeyDown(KeyCode.B) && PlayerManager.instance.player.stats.isDead == false)
            SwitchWithKeyTo(skillTreeUI);

        if (Input.GetKeyDown(KeyCode.K) && PlayerManager.instance.player.stats.isDead == false)
            SwitchWithKeyTo(creftUI);

        if (Input.GetKeyDown(KeyCode.O) && PlayerManager.instance.player.stats.isDead == false)
            SwitchWithKeyTo(optionsUI);
        if (Input.GetKeyDown(KeyCode.H) && PlayerManager.instance.player.stats.isDead == false)
        {
            dialogText.text = signTextContent;
            SwitchWithKeyTo(dialogBox);
        }
        if (Input.GetKeyDown(KeyCode.T) && PlayerManager.instance.player.stats.isDead == false)
        {
            dialogText.text = "骷髅击杀数：" + PlayerManager.instance.iSkeletonKilled.ToString()
                + "\n史莱姆击杀数：" + PlayerManager.instance.iSlimeKilled.ToString()
                + "\n带红帽的怪物击杀数：" + PlayerManager.instance.iShadyKilled.ToString()
                + "\n弓箭手击杀数：" + PlayerManager.instance.iArcherKilled.ToString()
                + "\nBOSS击杀数：" + PlayerManager.instance.iDeathBringerKilled.ToString();
            SwitchWithKeyTo(dialogBox);
        }
    }

    public void SwitchTo(GameObject _menu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            bool fadeScreen = transform.GetChild(i).gameObject.GetComponent<UI_FadeScreen>() != null;

            if (!fadeScreen)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        if (_menu != null)
        {
            AudioManager.instance.PlaySFX(5, null);
            _menu.SetActive(true);
        }

        if(GameManager.instance != null)
        {
            if(_menu == inGameUI)
                GameManager.instance.PauseGame(false);
            else
                GameManager.instance.PauseGame(true);
        }
    }

    public void SwitchWithKeyTo(GameObject _menu)
    {
        if (_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            CheckForInGameUI();
            return;
        }

        SwitchTo(_menu);
    }

    private void CheckForInGameUI()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).gameObject.activeSelf && transform.GetChild(i).GetComponent<UI_FadeScreen>() == null)
                return;
        }
        
        SwitchTo(inGameUI);
    }

    public void SwitchOnEndScreen()
    {
        fadeScreen.FadeOut();
        Invoke("x", 1f);

        StartCoroutine(EndScreenCoroutine());
    }

    private void x()
    {
        deadIcon.SetActive(true);
    }

    IEnumerator EndScreenCoroutine()
    {
        yield return new WaitForSeconds(1);
        endText.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        restartButton.SetActive(true);
    }

    public void RestartGameButton() => GameManager.instance.RestartScene();

    public void SaveGameButton()
    {
        SaveManager.instance.SaveGame();
        Application.Quit();
    }

    public void LoadData(GameData _data)
    {
       foreach(KeyValuePair<string, float> pair in _data.volumeSettings)
        {
            foreach(UI_VolumeSlider item in volumeSettings)
            {
                if (item.parameter == pair.Key)
                    item.LoadSlider(pair.Value);
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.volumeSettings.Clear();

        foreach (UI_VolumeSlider item in volumeSettings)
        {
            _data.volumeSettings.Add(item.parameter,item.slider.value);
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName = "MainScene";
    [SerializeField] private GameObject continueButton;
    [SerializeField] UI_FadeScreen fadeScreen;
    [SerializeField] private GameObject mainIcon;
    private int z = 0;

    private void Start()
    {
       

    }

    private void Update()
    {
        if(z == 0)
        {
            if (SaveManager.instance.HasSavedData() == false)
                continueButton.SetActive(false);
            Invoke("x", 0.1f);
            z = 1;
        }

    }

    private void x()
    {
        mainIcon.SetActive(true);
    }
    private void y()
    {
        mainIcon.SetActive(false);
    }

    public void ContinueGame()
    {
        StartCoroutine(LoadSceneWithFadeEffect(1.5f));
    }

    public void NewGame()
    {
        SaveManager.instance.DeleteSaveData();
        PlayerPrefs.DeleteAll();
        StartCoroutine(LoadSceneWithFadeEffect(1.5f));
    }

    public void QuitGame()
    {
       Application.Quit();
    }

    IEnumerator LoadSceneWithFadeEffect(float _delay)
    {
        Invoke("y", 1.4f);
        fadeScreen.FadeOut();

        yield return new WaitForSeconds(_delay);

        SceneManager.LoadScene(sceneName);
    }
}

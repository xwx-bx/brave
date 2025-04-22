using UnityEngine;
using UnityEngine.UI;

public class HpBar_ToggleController : MonoBehaviour
{
    public Toggle hpBarToggle;           
    public GameObject entityStatusUI;

    private const string HpBarToggleKey = "HpBarToggleState";

    public void Start()
    {
        bool isToggled = PlayerPrefs.GetInt(HpBarToggleKey, 1) == 1;
        hpBarToggle.isOn = isToggled;
        SetEntityStatusUIActive(isToggled);  

        hpBarToggle.onValueChanged.AddListener(OnHpBarToggleChanged);
    }

   
    private void OnHpBarToggleChanged(bool isOn)
    {

        PlayerPrefs.SetInt(HpBarToggleKey, isOn ? 1 : 0);
        PlayerPrefs.Save();  

        SetEntityStatusUIActive(isOn);
    }

    private void SetEntityStatusUIActive(bool isActive)
    {
        if (entityStatusUI != null)
        {
            
            entityStatusUI.SetActive(isActive);  
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    private Entity entity;
    private CharacterStats myStats;
    private RectTransform myTransform;
    private Slider slider;
    private Player player;
    private TextMeshProUGUI text;
    [SerializeField] private string s;

    private int x = 1;
    

    private void Start()
    {
        
        myTransform = GetComponent<RectTransform>();
        entity = GetComponentInParent<Entity>();
        slider = GetComponentInChildren<Slider>();
        myStats = GetComponentInParent<CharacterStats>();
        text = GetComponentInChildren<TextMeshProUGUI>();

        entity.onFlipped += FlipUI;
        myStats.onHealthChanged += UpdateHealthUI;
        UpdateHealthUI();
    }

    private void OnEnable()
    {
        player = PlayerManager.instance.player;
        if (entity != null)
        {
            entity.onFlipped += FlipUI;
        }

        if (myStats != null)
        {
            myStats.onHealthChanged += UpdateHealthUI;
            UpdateHealthUI();
        }

        
        
        
    }

    private void Update()
    {
        if (myTransform != null && player.facingDir != x && s == "Player")
        {
            FlipUI();
        }
    }


    private void UpdateHealthUI()
    {
        slider.maxValue = myStats.GetMaxHealthValue();
        slider.value = myStats.currentHealth;
        text.text = myStats.currentHealth + "/" + myStats.GetMaxHealthValue();
    }

    private void OnDisable()
    {
        if (entity != null)
            entity.onFlipped -= FlipUI;

        if (myStats != null)
            myStats.onHealthChanged -= UpdateHealthUI;
    }
    private void FlipUI()
    {
        myTransform.Rotate(0, 180, 0);
        x = x * -1;
    }

    
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI healthText;

    [SerializeField] private Image dashImage;
    [SerializeField] private Image ParryImage;
    [SerializeField] private Image crystalImage;
    [SerializeField] private Image swordImage;
    [SerializeField] private Image blackholeImage;
    [SerializeField] private Image flaskImage;

    private SkillManager skills;

    [Header("Souls info")]
    [SerializeField] private TextMeshProUGUI currentSouls;
    [SerializeField] private float soulsAmount;
    [SerializeField] private float increaseRate = 100;

    private Player player;

    void Start()
    {
        player = PlayerManager.instance.player;
        if (playerStats != null)
            playerStats.onHealthChanged += UpdateHealthUI;


        skills = SkillManager.instance;
    }


    void Update()
    {
        UpdateHealthUI();
        UpdateSoulsUI();

        if (Input.GetKeyDown(KeyCode.LeftShift) && skills.dash.dashUnlocked)
            SetCooldownOf(dashImage);

        if (Input.GetKeyDown(KeyCode.Q) && skills.parry.parryUnlocked && player.IsGroundDetected())
            SetCooldownOf(ParryImage);

        if (Input.GetKeyDown(KeyCode.F) && skills.crystal.crystalUnlocked)
            SetCooldownOf(crystalImage);

        //if (Input.GetKeyUp(KeyCode.Mouse1) && skills.sword.swordUnlocked && HasNoSword())
        //    SetCooldownOf(swordImage);

        if (Input.GetKeyDown(KeyCode.R) && skills.blackhole.blackholeUnlocked)
            SetCooldownOf(blackholeImage);

        if (Input.GetKeyDown(KeyCode.Alpha1) && Inventory.instance.GetEquipment(EquipmentType.Flask) != null)
            SetCooldownOf(flaskImage);

        checkCooldownOf(dashImage, skills.dash.cooldown);
        checkCooldownOf(ParryImage, skills.parry.cooldown);
        checkCooldownOf(crystalImage, skills.crystal.cooldown);
        checkCooldownOf(swordImage, skills.sword.cooldown);
        checkCooldownOf(blackholeImage, skills.blackhole.cooldown);
        checkCooldownOf(flaskImage, Inventory.instance.flaskCooldown);
    }

    private void UpdateSoulsUI()
    {
        if (soulsAmount < PlayerManager.instance.GetCurrency())
            soulsAmount += increaseRate * Time.deltaTime;
        else
            soulsAmount = PlayerManager.instance.GetCurrency();

        currentSouls.text = ((int)soulsAmount).ToString();
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = playerStats.GetMaxHealthValue();
        slider.value = playerStats.currentHealth;
        healthText.text = playerStats.currentHealth + "/" + playerStats.GetMaxHealthValue();
    }

    

    private void SetCooldownOf(Image _image)
    {
        if(_image.fillAmount <= 0)
            _image.fillAmount = 1;
    }

    public void SetCooldownOfCrystalImage()
    {
        SetCooldownOf(crystalImage);
    }
    public void SetCooldownOfSwordImage()
    {
        SetCooldownOf(swordImage);
    }

    private void checkCooldownOf(Image _image, float _cooldown)
    {
        if(_image.fillAmount > 0)
            _image.fillAmount -= 1/ _cooldown * Time.deltaTime;
    }

    //private bool HasNoSword()
    //{
    //    if (!PlayerManager.instance.player.sword)
    //    {
    //        return true;
    //    }
    //    return false;
    //}
}

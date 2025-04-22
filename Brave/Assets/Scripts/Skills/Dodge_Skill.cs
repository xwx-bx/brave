using UnityEngine;
using UnityEngine.UI;

public class Dodge_Skill : Skill
{
    [Header("Dodge")]
    [SerializeField] private UI_SkillTreeSlot unlockDodgeButton;
    [SerializeField] private int evasionAmount;
    public bool dodgeUnlocked;

    [Header("Mirage doge")]
    [SerializeField] private UI_SkillTreeSlot unlockMirageDodge;
    public bool mirageDodgeUnlocked;

    protected override void Start()
    {
        base.Start();

        unlockDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockDodge);
        unlockMirageDodge.GetComponent<Button>().onClick.AddListener(UnlockMirageDodge);
    }

    protected override void CheckUnlock()
    {
        UnlockDodge();
        UnlockMirageDodge();
    }
    protected void UnlockDodge()
    {
        if (unlockDodgeButton.unlocked  && !dodgeUnlocked)
        {
            player.stats.evasion.AddModifier(evasionAmount);
            Inventory.instance.UpdateStatsUI();
            dodgeUnlocked = true;
        }
    }

    protected void UnlockMirageDodge()
    {
        if (unlockMirageDodge.unlocked)
            mirageDodgeUnlocked = true;
    }

    public void CreateMirageOnDodge()
    {
        if (mirageDodgeUnlocked)
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
    }
}

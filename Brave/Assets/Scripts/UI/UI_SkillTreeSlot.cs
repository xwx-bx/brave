using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillTreeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,ISaveManager
{
    private UI ui;
    private Image skillImage;

    [SerializeField] private int skillCost;
    [SerializeField] private string skillName;
    [TextArea]
    [SerializeField] private string skillDescription;
    [SerializeField] private Color lockedSkillColor;

    public bool unlocked;

    [SerializeField] private UI_SkillTreeSlot[] shoulderBeUnlocked;
    [SerializeField] private UI_SkillTreeSlot[] shoulderBeLocked;

    [SerializeField] private Dictionary<string, bool> Skills = new Dictionary<string, bool>();

    private void OnValidate()
    {
        gameObject.name = "SkillTreeSlot_UI - " + skillName;
    }

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());
    }

    private void Start()
    {
        skillImage = GetComponent<Image>();
        ui = GetComponentInParent<UI>();

        skillImage.color = lockedSkillColor;

        if (unlocked)
            skillImage.color = Color.white;
    }

    public void UnlockSkillSlot()
    {
        if (unlocked) return;

        if (!PlayerManager.instance.HaveEnoughMoney(skillCost)) return;

        foreach (UI_SkillTreeSlot slot in shoulderBeUnlocked)
        {
            if (!slot.unlocked)
            {
                return; // 确保所有需要解锁的技能都已解锁
            }
        }

        foreach (UI_SkillTreeSlot slot in shoulderBeLocked)
        {
            if (slot.unlocked)
            {
                return; // 确保没有已解锁的技能
            }
        }

        unlocked = true; // 更新技能状态
        skillImage.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillTooltip.ShowToolTip(skillDescription, skillName,skillCost);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillTooltip.HideToolTip();
    }

    public void LoadData(GameData _data)
    {
        if (_data.skillTree.TryGetValue(skillName, out bool value))
        {
            unlocked = value;
        }
    }

    public void SaveData(ref GameData _data)
    {
        if (_data.skillTree.TryGetValue(skillName, out bool value))
        {
            _data.skillTree.Remove(skillName);
            _data.skillTree.Add(skillName, unlocked);
        }
        else
        {
            _data.skillTree.Add(skillName, unlocked);
        }
    }
}

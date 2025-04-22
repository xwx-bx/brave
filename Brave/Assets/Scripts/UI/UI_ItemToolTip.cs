using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ItemToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemDescription;

    public void ShowTooltip(ItemData_Equipment item)
    {
        if (item == null)
            return;

        itemNameText.text = item.itemName;

        switch (item.equipmentType)
        {
            case EquipmentType.Weapon: itemTypeText.text = "ÎäÆ÷"; break;
            case EquipmentType.Armor: itemTypeText.text = "·À¾ß"; break;
            case EquipmentType.Amulet: itemTypeText.text = "ÊÎÆ·"; break;
            case EquipmentType.Flask: itemTypeText.text = "Ò©Ë®"; break;
            default: itemTypeText.text = ""; break;
        }

        itemDescription.text = item.GetDescription();

        AdjustPosition();

        gameObject.SetActive(true);
    }

    public void HideTooltip() => gameObject.SetActive(false);
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Ui_EquipmentSlot : UI_ItemSlot
{
    public EquipmentType slotType;

    private void OnValidate()
    {
        gameObject.name = "Equipment slot - " + slotType.ToString();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (item != null && item.data != null)
        {
            Inventory.instance.UnequipItem(item.data as ItemData_Equipment);
            Inventory.instance.AddItem(item.data as ItemData_Equipment);

            ui.itemTooltip.HideTooltip();

            CleanUpSlot();
        }

        
    }
}

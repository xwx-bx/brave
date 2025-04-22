using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("Player's drop")]
    [SerializeField] private float chanceTolooseItems;
    [SerializeField] private float chanceToLooseMaterials;

    public override void GenerateDrop()
    {
        Inventory invenotory = Inventory.instance;


        List<InventoryItem> itemsToUnequip = new List<InventoryItem>();
        List<InventoryItem> materialsToLoose = new List<InventoryItem>();

        foreach (InventoryItem item in invenotory.GetEquipmentList())
        {
            if (Random.Range(0, 100) < chanceTolooseItems)
            {
                DropItem(item.data);
                itemsToUnequip.Add(item);
            }
        }

        for (int i = 0; i < itemsToUnequip.Count; i++)
        {
            invenotory.UnequipItem(itemsToUnequip[i].data as ItemData_Equipment);
            invenotory.UpdateEquipmentSlot();
        }

        foreach (InventoryItem item in invenotory.GetStashList())
        {
            if (Random.Range(0, 100) <= chanceToLooseMaterials)
            {
                DropItem(item.data);
                materialsToLoose.Add(item);
            }
        }

        for (int i = 0; i < materialsToLoose.Count; i++)
        {
            invenotory.RemoveItem(materialsToLoose[i].data);
        }
    }
}

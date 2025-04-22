using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Inventory : MonoBehaviour, ISaveManager
{
    public static Inventory instance;

    public List<ItemData> startingEquipment;

    public List<InventoryItem> equipment;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictiionary;

    public List<InventoryItem> inventory;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;

    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDictionary;

    public bool x;
    public bool y;
    private float z = 0;

    [Header("Inventory UI")]

    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent;

    private UI_ItemSlot[] inventoryItemSlot;
    private UI_ItemSlot[] stashItemSlot;
    private Ui_EquipmentSlot[] equipmentSlot;
    private UI_StatSlot[] statSlot;

    [Header("Items cooldown")]
    private float lastTimeUsedFlask;
    private float lastTimeUsedArmor;

    public float flaskCooldown { get; private set; }

    [Header("Data base")]
    public List<ItemData> itemDatabase;
    public List<InventoryItem> loadedItems;
    public List<ItemData_Equipment> loadedEquipment;

    private void Awake()
    {
        x = true;
        y = true;
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        inventory = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();

        equipment = new List<InventoryItem>();
        equipmentDictiionary = new Dictionary<ItemData_Equipment, InventoryItem>();

        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentSlot = equipmentSlotParent.GetComponentsInChildren<Ui_EquipmentSlot>();
        statSlot = statSlotParent.GetComponentsInChildren<UI_StatSlot>();
        Invoke("AddStartingItems", 0.1f);
    }

    private void AddStartingItems()
    {
        foreach (ItemData_Equipment item in loadedEquipment)
        {
            EquipItem(item);
        }

        if (loadedItems.Count > 0)
        {
            foreach (InventoryItem item in loadedItems)
            {
                for (int i = 0; i < item.stackSize; i++)
                {
                    AddItem(item.data);
                }
            }
            return;
        }

        for (int i = 0; i < startingEquipment.Count; i++)
        {
            if (startingEquipment[i] != null)
                AddItem(startingEquipment[i]);
        }
    }

    public void EquipItem(ItemData _item)
    {
        ItemData_Equipment newEquipment = _item as ItemData_Equipment;
        InventoryItem newItem = new InventoryItem(newEquipment);

        ItemData_Equipment oldEquipment = null;

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictiionary)
        {
            if (item.Key.equipmentType == newEquipment.equipmentType)
                oldEquipment = item.Key;
        }

        if (oldEquipment != null)
        {
            UnequipItem(oldEquipment);
            AddItem(oldEquipment);
        }

        equipment.Add(newItem);
        equipmentDictiionary.Add(newEquipment, newItem);
        newEquipment.AddModifiers();

        RemoveItem(_item);

        UpdateSlotUI();

        if (z <= 1)
        {
            PlayerManager.instance.player.stats.currentHealth = PlayerManager.instance.player.stats.GetMaxHealthValue();
        }
        PlayerManager.instance.player.stats.onHealthChanged();
    }

    private void Update()
    {
        if (z <= 1)
        {
            z += Time.deltaTime;
        }
    }

    public void UnequipItem(ItemData_Equipment itemToRemove)
    {
        if (equipmentDictiionary.TryGetValue(itemToRemove, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDictiionary.Remove(itemToRemove);
            itemToRemove.RemoveModifiers();
        }

        if(PlayerManager.instance.player.stats.currentHealth > PlayerManager.instance.player.stats.GetMaxHealthValue())
            PlayerManager.instance.player.stats.currentHealth = PlayerManager.instance.player.stats.GetMaxHealthValue();
        PlayerManager.instance.player.stats.onHealthChanged();
    }

    private void UpdateSlotUI()
    {
        for (int i = 0; i < inventoryItemSlot.Length; i++)
        {
            inventoryItemSlot[i].CleanUpSlot();
        }

        for (int i = 0; i < stashItemSlot.Length; i++)
        {
            stashItemSlot[i].CleanUpSlot();
        }

        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictiionary)
            {
                if (item.Key.equipmentType == equipmentSlot[i].slotType)
                    equipmentSlot[i].UpdateSlot(item.Value);
            }
        }

        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlot[i].UpdateSlot(inventory[i]);
        }

        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlot[i].UpdateSlot(stash[i]);
        }

        UpdateStatsUI();
    }

    public void UpdateStatsUI()
    {
        if (statSlot == null)
            return;

        for (int i = 0; i < statSlot.Length; i++)
        {
            if (statSlot[i] == null)
                continue;

            statSlot[i].UpdateStatValueUI();
        }
    }

    public void UpdateEquipmentSlot()
    {
        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            equipmentSlot[i].CleanUpSlot();
        }
    }

    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment && CanAddItem())
            AddToInventory(_item);
        else if (_item.itemType == ItemType.Material)
            AddToStash(_item);


        UpdateSlotUI();
    }

    private void AddToStash(ItemData _item)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }

    private void AddToInventory(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }

    public void RemoveItem(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventory.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            else
                value.RemoveStack();
        }

        if (stashDictionary.TryGetValue(_item, out InventoryItem stashValue))
        {
            if (stashValue.stackSize <= 1)
            {
                stash.Remove(stashValue);
                stashDictionary.Remove(_item);
            }
            else
                stashValue.RemoveStack();
        }

        UpdateSlotUI();
    }

    public bool CanAddItem()
    {
        if (inventory.Count >= inventoryItemSlot.Length)
            return false;

        return true;
    }

    public bool CanCraft(ItemData_Equipment _itemTocraft, List<InventoryItem> _requireMaterials)
    {
        List<InventoryItem> materialsToRemove = new List<InventoryItem>();
        for (int i = 0; i < _requireMaterials.Count; i++)
        {
            if (stashDictionary.TryGetValue(_requireMaterials[i].data, out InventoryItem stashValue))
            {
                if (stashValue.stackSize < _requireMaterials[i].stackSize)
                {
                    return false;
                }
                else
                {
                    materialsToRemove.Add(stashValue);
                }
            }
            else
            {
                return false;
            }
        }


        for (int i = 0; i < materialsToRemove.Count; i++)
        {
            for (int j = 0; j < _requireMaterials[i].stackSize; j++)
                RemoveItem(materialsToRemove[i].data);
        }

        AddItem(_itemTocraft);

        return true;
    }

    public List<InventoryItem> GetEquipmentList() => equipment;

    public List<InventoryItem> GetStashList() => stash;

    public ItemData_Equipment GetEquipment(EquipmentType _Type)
    {
        ItemData_Equipment equipedItem = null;

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictiionary)
        {
            if (item.Key.equipmentType == _Type)
                equipedItem = item.Key;
        }

        return equipedItem;
    }

    public void UseFlask()
    {
        ItemData_Equipment currentFlask = GetEquipment(EquipmentType.Flask);

        if (currentFlask == null)
            return;

        flaskCooldown = currentFlask.itemCooldown;

        if (x)
        {
            lastTimeUsedFlask = -1 * currentFlask.itemCooldown;
            x = false;
        }

        bool canUseFlask = Time.time > lastTimeUsedFlask + currentFlask.itemCooldown;

        if (canUseFlask)
        {
            AudioManager.instance.PlaySFX(29, null);
            currentFlask.Effect(null);
            lastTimeUsedFlask = Time.time;
        }
        else
        {

        }
    }

    public bool CanUseArmor()
    {
        ItemData_Equipment currentArmor = GetEquipment(EquipmentType.Armor);

        if (y)
        {
            lastTimeUsedFlask = -1 * currentArmor.itemCooldown;
            y = false;
        }

        if (Time.time > lastTimeUsedArmor + currentArmor.itemCooldown)
        {
            lastTimeUsedArmor = Time.time;
            return true;
        }

        return false;
    }

    public void LoadData(GameData _data)
    {
        foreach (KeyValuePair<string, int> pair in _data.inventory)
        {
            foreach (var item in itemDatabase)
            {
                if (item != null && item.itemId == pair.Key)
                {
                    InventoryItem itemToLoad = new InventoryItem(item);
                    itemToLoad.stackSize = pair.Value;

                    loadedItems.Add(itemToLoad);
                }
            }
        }

        foreach (string loadedItemId in _data.equipmentId)
        {
            foreach (var item in itemDatabase)
            {
                if (item != null && item.itemId == loadedItemId)
                {
                    ItemData_Equipment itemToLoad = item as ItemData_Equipment;
                    loadedEquipment.Add(itemToLoad);
                }
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.inventory.Clear();
        _data.equipmentId.Clear();

        foreach (KeyValuePair<ItemData, InventoryItem> pair in inventoryDictionary)
        {
            _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        }

        foreach (KeyValuePair<ItemData, InventoryItem> pair in stashDictionary)
        {
            _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        }

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> pair in equipmentDictiionary)
        {
            _data.equipmentId.Add(pair.Key.itemId);
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Fill up item data base")]
    private void FillUpItemDataBase() => itemDatabase = new List<ItemData>(GetItemDataBase());

    private List<ItemData> GetItemDataBase()
    {
        List<ItemData> itemDataBase = new List<ItemData>();
        string[] assetNames = AssetDatabase.FindAssets("", new[] { "Assets/Data/Items" });

        foreach (string SQName in assetNames)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SQName);
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOpath);
            itemDataBase.Add(itemData);
        }

        return itemDataBase;
    }
#endif
}

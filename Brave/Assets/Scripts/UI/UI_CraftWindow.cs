using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNane;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Button CraftButton;

    [SerializeField] private Image[] materialImage;

    public void SetupCraftWindow(ItemData_Equipment _data)
    {

        CraftButton.onClick.RemoveAllListeners();

        for(int i = 0; i < materialImage.Length; i++)
        {
            materialImage[i].color = Color.clear;
            materialImage[i].GetComponentsInChildren<TextMeshProUGUI>()[0].color = Color.clear;
        }

        for(int i = 0; i < _data.craftingMaterials.Count; i++)
        {
            if(_data.craftingMaterials.Count > materialImage.Length)
            {

            }

            materialImage[i].sprite = _data.craftingMaterials[i].data.icon;
            materialImage[i].color = Color.white;

            TextMeshProUGUI materialSlotText = materialImage[i].GetComponentInChildren<TextMeshProUGUI>();

            materialSlotText.text = _data.craftingMaterials[i].stackSize.ToString();
            materialSlotText.color = Color.white;
        }

        itemIcon.sprite = _data.icon;
        itemNane.text = _data.itemName;
        itemDescription.text = _data.GetDescription();

        CraftButton.onClick.AddListener(() => Inventory.instance.CanCraft(_data, _data.craftingMaterials));
    }

}

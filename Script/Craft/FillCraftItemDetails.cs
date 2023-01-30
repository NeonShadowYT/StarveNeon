using UnityEngine;
using UnityEngine.UI;

public class FillCraftItemDetails : MonoBehaviour // https://www.youtube.com/c/maximple, https://www.youtube.com/channel/UC2pH2qNfh2sEAeYEGs1k_Lg
{
    [Space]
    [Header("Скрипты")]
    public CraftQueueManager craftQueueManager;
    public CraftManager craftManager;
    public InventoryManager inventoryManager;

    [Space]
    [Header("Текущая информация о крафте")]
    public CraftScriptableObject currentCraftItem;

    [Space]
    public GameObject craftResourcePrefab;
    public GameObject craftInfoPanel;

    public void FillItemDetails()
    {
        craftManager.currentCraftItem = this;
        for (int i = 0; i < craftInfoPanel.transform.childCount; i++) Destroy(craftInfoPanel.transform.GetChild(i).gameObject); //for (int i = 0; i < GameObject.Find(craftInfoPanelName).transform.childCoun
                                                                                                                                //t; i++)
        craftManager.craftItemName.text = currentCraftItem.finalCraft.name;
        craftManager.craftItemDescription.text = currentCraftItem.finalCraft.itemDescription;
        craftManager.craftItemImage.sprite = currentCraftItem.finalCraft.icon;
        craftManager.craftItemDuration.text = currentCraftItem.craftTime.ToString();
        craftManager.craftItemAmount.text = currentCraftItem.craftAmount.ToString();

        bool canCraft = true;

        for (int i = 0; i < currentCraftItem.craftResources.Count; i++)
        {
            CraftResourceDetails crd = Instantiate(craftResourcePrefab, craftInfoPanel.transform).GetComponent<CraftResourceDetails>();

            crd.amountText.text = currentCraftItem.craftResources[i].craftObjectAmount.ToString();
            crd.itemTypeText.text = currentCraftItem.craftResources[i].craftObject.itemName;

            int totalAmount = currentCraftItem.craftResources[i].craftObjectAmount * int.Parse(craftQueueManager.craftAmountInputField.text);
            crd.totalText.text = totalAmount.ToString();
            int resourceAmount = 0;

            foreach (InventorySlot slot in inventoryManager.slots)
            {
                if (slot.isEmpty) continue;
                if (slot.item.itemName == currentCraftItem.craftResources[i].craftObject.itemName) resourceAmount += slot.amount;
            }
            crd.haveText.text = resourceAmount.ToString();
            if (resourceAmount < totalAmount) canCraft = false;
        }
        if (canCraft) craftManager.craftBtn.interactable = true; else craftManager.craftBtn.interactable = false;
        craftQueueManager.currentCraftItem = currentCraftItem;
    }
}

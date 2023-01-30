using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftQueueItemDetails : MonoBehaviour // https://www.youtube.com/c/maximple, https://www.youtube.com/channel/UC2pH2qNfh2sEAeYEGs1k_Lg
{
    [Space]
    [Header("Это должо быть null")]
    public InventoryManager inventoryManager;
    public CraftManager craftManager;

    [Space]
    [Header("Информация")]
    public Image itemImage;
    public TMP_Text amountText, timeText;

    [Space]
    [Header("Информация о текущем крафте")]
    public CraftScriptableObject currentCraftItem;

    public int craftTime;
    public int craftAmount;

    private int initialCraftTime;

    private void Start()
    {
        initialCraftTime = craftTime;
        craftTime++;

        if (transform.parent.childCount <= 1) StartInvoke(); else UpdateTime();
    }

    public void StartInvoke() => InvokeRepeating("UpdateTime", 0f, 1f);

    public void RemoveFromQueue()
    {
        foreach (CraftResource resource in currentCraftItem.craftResources) inventoryManager.AddItem(resource.craftObject, resource.craftObjectAmount * craftAmount);
        CancelInvoke();
        craftManager.currentCraftItem.FillItemDetails();
        if (transform.parent.childCount > 1) transform.parent.GetChild(1).GetComponent<CraftQueueItemDetails>().StartInvoke();
        Destroy(gameObject);
    }

    void UpdateTime()
    {
        amountText.text = "X" + craftAmount.ToString();
        craftTime--;
        if (craftTime <= 0)
        {
            inventoryManager.AddItem(currentCraftItem.finalCraft, currentCraftItem.craftAmount);
            craftAmount--;
            craftTime = initialCraftTime;
            if (craftAmount <= 0)
            {
                CancelInvoke();
                if (transform.parent.childCount > 1) transform.parent.GetChild(1).GetComponent<CraftQueueItemDetails>().StartInvoke();
                Destroy(gameObject);
            }
        }
        else 
        {
            int minutes = Mathf.FloorToInt(craftTime / 60);
            int seconds = craftTime - minutes * 60;
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}

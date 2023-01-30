using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class FractionManager : MonoBehaviour
{
    [Space]
    [Header("Скрипты")]
    public FractionScriptableObject currentFraction;
    public InventoryManager inventoryManager;

    public MoneyManager moneyManager;
    public MoneySlot currentMoneySlot;

    [Space]
    [Header("Разное")]
    public Image FractionImage;
    public TMP_Text FractionNameText;
    public GameObject fractionPanel;

    [Space]
    [Header("Товары")]
    public Transform TovarePanelSpawn;
    public GameObject ToverePrefab;

    public List<TovareList> Tovare;
    private Tovare currentTovare;

    public void FractionFunc()
    {
        FractionImage.sprite = currentFraction.FractionIcon;
        FractionNameText.text = currentFraction.FractionName;
        CheckCurrentMoney();
        fractionPanel.SetActive(true);
        Tovare = currentFraction.Tovare;
        LoadTovareItems();
    }
    public void FractionClose() => fractionPanel.SetActive(false);
    public void AddMoney(MoneyItem money)
    {
        moneyManager.CurrentAddMoney(money);
        if (currentMoneySlot != null && currentTovare != null) currentTovare.UpdateTovare(currentMoneySlot);
    }
    public void LoadTovareItems()
    {
        for (int i = 0; i < TovarePanelSpawn.childCount; i++) Destroy(TovarePanelSpawn.GetChild(i).gameObject);
        for (int i = 0; i < currentFraction.Tovare.Count; i++)
        {
            Tovare currentTovare = Instantiate(ToverePrefab, TovarePanelSpawn).GetComponent<Tovare>(); // делай
            currentTovare.item = currentFraction.Tovare[i].TovareItem;
            currentTovare.inventoryManagers = inventoryManager;
            currentTovare.price = currentFraction.Tovare[i].TovarePrice;
            currentTovare.UpdateTovare(currentMoneySlot);
            currentTovare.fractionManager = this;
        }
    }
    public void CheckCurrentMoney() => moneyManager.CheckCurrentMoney(currentFraction);
}
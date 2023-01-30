using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

/// IPointerDownHandler - Следит за нажатиями мышки по объекту на котором висит этот скрипт
/// IPointerUpHandler - Следит за отпусканием мышки по объекту на котором висит этот скрипт
/// IDragHandler - Следит за тем не водим ли мы нажатую мышку по объекту
public class DragAndDropItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler // https://www.youtube.com/c/maximple, https://www.youtube.com/channel/UC2pH2qNfh2sEAeYEGs1k_Lg
{
    private QuickslotInventory quickslotInventory; // added this++
    private CraftManager craftManager;

    private Transform player, _savingEnvironment;

    [Space]
    [Header("Старый слот")]
    public InventorySlot oldSlot;

    [Space]
    [Header("Одежда")]
    public List<ClothAdder> clothAdders;

    private Image slotImage;

    private void Start()
    {
        _savingEnvironment = GameObject.Find("Saving Environment").transform;
        quickslotInventory = FindObjectOfType<QuickslotInventory>();
        craftManager = FindObjectOfType<CraftManager>();
        player = GameObject.FindObjectOfType<CustomCharacterController>().transform; //ПОСТАВЬТЕ ТЭГ "PLAYER" НА ОБЪЕКТЕ ПЕРСОНАЖА!

        slotImage = oldSlot.iconGO;
        if (oldSlot.clothType != ClothType.None)
        {
            clothAdders = new List<ClothAdder>();
            clothAdders.AddRange(FindObjectsOfType<ClothAdder>());
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (oldSlot.isEmpty) return; // Если слот пустой, то мы не выполняем то что ниже return;

        GetComponent<RectTransform>().position += new Vector3(eventData.delta.x, eventData.delta.y);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (oldSlot.isEmpty) return; // Если слот пустой, то мы не выполняем то что ниже return;

        slotImage.color = new Color(1, 1, 1, 0.75f); //Делаем картинку прозрачнее
        slotImage.raycastTarget = false; // Делаем так чтобы нажатия мышкой не игнорировали эту карти
                                                               // нку
        transform.SetParent(transform.parent.parent.parent); // Делаем наш DraggableObject ребенком InventoryPanel чтобы DraggableObject был над другими слотами инвенторя
    }

    public void ReturnBackToSlot()
    {
        if (oldSlot.isEmpty) return; // Если слот пустой, то мы не выполняем то что ниже return;

        slotImage.color = new Color(1, 1, 1, 1f); // Делаем картинку опять не прозрачной
        slotImage.raycastTarget = true; // И чтобы мышка опять могла ее засечь

        transform.SetParent(oldSlot.transform); //Поставить DraggableObject обратно в свой старый слот
        transform.position = oldSlot.transform.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (oldSlot.isEmpty) return; // Если слот пустой, то мы не выполняем то что ниже return;

        slotImage.color = new Color(1, 1, 1, 1f); // Делаем картинку опять не прозрачной
        slotImage.raycastTarget = true; // И чтобы мышка опять могла ее засечь

        transform.SetParent(oldSlot.transform); //Поставить DraggableObject обратно в свой старый слот
        transform.position = oldSlot.transform.position; //Если мышка отпущена над объектом по имени UIPanel, то...

        if (eventData.pointerCurrentRaycast.gameObject.name == "UIBG") // renamed to UIBG
        {
            if (oldSlot.clothType != ClothType.None && oldSlot.item != null) foreach (ClothAdder clothAdder in clothAdders) clothAdder.RemoveClothes(oldSlot.item.clothingPrefab);

            if (Input.GetKey(KeyCode.LeftShift))
            {
                GameObject itemObject = Instantiate(oldSlot.item.itemPrefab, player.position + Vector3.up + player.forward, Quaternion.identity);
                itemObject.transform.SetParent(_savingEnvironment);
                itemObject.GetComponent<Item>().amount = Mathf.CeilToInt((float)oldSlot.amount / 2);

                oldSlot.amount -= Mathf.CeilToInt((float)oldSlot.amount / 2);
                oldSlot.itemAmountText.text = oldSlot.amount.ToString();
            }
            else if (Input.GetKey(KeyCode.LeftControl))
            {
                GameObject itemObject = Instantiate(oldSlot.item.itemPrefab, player.position + Vector3.up + player.forward, Quaternion.identity);
                itemObject.transform.SetParent(_savingEnvironment);
                itemObject.GetComponent<Item>().amount = 1;

                oldSlot.amount--;
                oldSlot.itemAmountText.text = oldSlot.amount.ToString();
            }
            else
            {
                GameObject itemObject = Instantiate(oldSlot.item.itemPrefab, player.position + Vector3.up + player.forward, Quaternion.identity); // Выброс объектов из инвентаря - Спавним префаб обекта перед персонажем
                itemObject.transform.SetParent(_savingEnvironment); // Устанавливаем количество объектов такое какое было в слоте
                itemObject.GetComponent<Item>().amount = oldSlot.amount;

                NullifySlotData(); // убираем значения InventorySlot
                craftManager.currentCraftItem.FillItemDetails();
            }
            quickslotInventory.CheckItemInHand();
            quickslotInventory.ShowBuildingBlock();
        }
        else if (eventData.pointerCurrentRaycast.gameObject.transform.parent.parent == null) return;
        else if (eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<InventorySlot>() != null)
        {
            InventorySlot inventorySlot = eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<InventorySlot>();
            if (oldSlot.clothType != ClothType.None && oldSlot.item != null) foreach (ClothAdder clothAdder in clothAdders) clothAdder.RemoveClothes(oldSlot.item.clothingPrefab);

            if (inventorySlot.clothType != ClothType.None) //Перемещаем данные из одного слота в другой
            {
                if (inventorySlot.clothType == oldSlot.item.clothType)
                {
                    ExchangeSlotData(inventorySlot);
                    foreach (ClothAdder clothAdder in inventorySlot.GetComponentInChildren<DragAndDropItem>().clothAdders) clothAdder.addClothes(inventorySlot.item.clothingPrefab);
                }
                else return;
            }
            else
            {
                ExchangeSlotData(inventorySlot);
                quickslotInventory.CheckItemInHand();
                quickslotInventory.ShowBuildingBlock();
            }
        }
        if (oldSlot.amount <= 0) NullifySlotData();
    }

    public void NullifySlotData() // made public 
    {
        oldSlot.item = null; // убираем значения InventorySlot
        oldSlot.amount = 0;
        oldSlot.isEmpty = true;
        oldSlot.iconGO.color = new Color(1, 1, 1, 0);
        oldSlot.iconGO.sprite = null;
        oldSlot.itemAmountText.text = ""; //        _durabilityGameObject.SetActive(false);
    }

    void ExchangeSlotData(InventorySlot newSlot)
    {
        ItemScriptableObject item = newSlot.item; // Временно храним данные newSlot в отдельных переменных

        int amount = newSlot.amount;
        bool isEmpty = newSlot.isEmpty;
        float itemDurability = newSlot.itemDurability;

        Image iconGO = newSlot.iconGO;
        TMP_Text itemAmountText = newSlot.itemAmountText;

        if (item == null)
        {
            if (oldSlot.item.maximumAmount > 1 && oldSlot.amount > 1)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    newSlot.item = oldSlot.item;
                    newSlot.amount = Mathf.CeilToInt((float)oldSlot.amount / 2);
                    newSlot.isEmpty = false;
                    newSlot.SetIcon(oldSlot.iconGO.sprite);
                    newSlot.itemAmountText.text = newSlot.amount.ToString();

                    oldSlot.amount = Mathf.FloorToInt((float)oldSlot.amount / 2); ;
                    oldSlot.itemAmountText.text = oldSlot.amount.ToString();
                    return;
                }
                else if (Input.GetKey(KeyCode.LeftControl))
                {
                    newSlot.item = oldSlot.item;
                    newSlot.amount = 1;
                    newSlot.isEmpty = false;
                    newSlot.SetIcon(oldSlot.iconGO.sprite);
                    newSlot.itemAmountText.text = newSlot.amount.ToString();

                    oldSlot.amount--;
                    oldSlot.itemAmountText.text = oldSlot.amount.ToString();
                    return;
                }
            }
        }
        if (newSlot.item != null)
        {
            if (oldSlot.item.name.Equals(newSlot.item.name))
            {
                if (Input.GetKey(KeyCode.LeftShift) && oldSlot.amount > 1)
                {
                    if (Mathf.CeilToInt((float)oldSlot.amount / 2) < newSlot.item.maximumAmount - newSlot.amount)
                    {
                        newSlot.amount += Mathf.CeilToInt((float)oldSlot.amount / 2);
                        newSlot.itemAmountText.text = newSlot.amount.ToString();

                        oldSlot.amount -= Mathf.CeilToInt((float)oldSlot.amount / 2);
                        oldSlot.itemAmountText.text = oldSlot.amount.ToString();
                    }
                    else
                    {
                        int difference = newSlot.item.maximumAmount - newSlot.amount;
                        newSlot.amount = newSlot.item.maximumAmount;
                        newSlot.itemAmountText.text = newSlot.amount.ToString();

                        oldSlot.amount -= difference;
                        oldSlot.itemAmountText.text = oldSlot.amount.ToString();
                    }
                    return;
                }
                else if (Input.GetKey(KeyCode.LeftControl) && oldSlot.amount > 1)
                {
                    if (newSlot.item.maximumAmount != newSlot.amount)
                    {
                        newSlot.amount++;
                        newSlot.itemAmountText.text = newSlot.amount.ToString();

                        oldSlot.amount--;
                        oldSlot.itemAmountText.text = oldSlot.amount.ToString();
                    }
                    return;
                }
                else
                {
                    if (newSlot.amount + oldSlot.amount >= newSlot.item.maximumAmount)
                    {
                        int difference = newSlot.item.maximumAmount - newSlot.amount;
                        newSlot.amount = newSlot.item.maximumAmount;
                        newSlot.itemAmountText.text = newSlot.amount.ToString();

                        oldSlot.amount -= difference;
                        oldSlot.itemAmountText.text = oldSlot.amount.ToString();
                    }
                    else
                    {
                        newSlot.amount += oldSlot.amount;
                        newSlot.itemAmountText.text = newSlot.amount.ToString();
                        NullifySlotData();
                    }
                    return;
                }
            }
        }
        newSlot.item = oldSlot.item; // Заменяем значения newSlot на значения oldSlot
        newSlot.amount = oldSlot.amount;
        if (oldSlot.isEmpty == false)
        {
            newSlot.SetIcon(oldSlot.iconGO.sprite);
            if (oldSlot.item.maximumAmount != 1) newSlot.itemAmountText.text = oldSlot.amount.ToString(); else newSlot.itemAmountText.text = ""; // added this if statement for single items
        }
        else
        {
            newSlot.iconGO.color = new Color(1, 1, 1, 0);
            newSlot.iconGO.sprite = null;
            newSlot.itemAmountText.text = "";
        }
        newSlot.isEmpty = oldSlot.isEmpty;
        oldSlot.item = item; // Заменяем значения oldSlot на значения newSlot сохраненные в переменных
        oldSlot.amount = amount;
        if (isEmpty == false)
        {
            oldSlot.SetIcon(item.icon);
            if (item.maximumAmount != 1) oldSlot.itemAmountText.text = amount.ToString(); else oldSlot.itemAmountText.text = "";// added this if statement for single items
        }
        else
        {
            oldSlot.iconGO.color = new Color(1, 1, 1, 0);
            oldSlot.iconGO.sprite = null;
            oldSlot.itemAmountText.text = "";
        }
        oldSlot.isEmpty = isEmpty;
    }
}

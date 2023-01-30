using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class QuickslotInventory : MonoBehaviour //https://www.youtube.com/channel/UC2pH2qNfh2sEAeYEGs1k_Lg
{
    [Space]
    [Header("Скрипты")]
    public WeaponManager weaponManager;
    public InventoryManager inventoryManager;
    public Indicators indicators;
    public BuildingSystem buildingSystem;

    [Space]
    public Animator anim;

    [Space]
    [Header("Иконка слота")]
    public Sprite selectedSprite, notSelectedSprite;

    [Space]
    public Transform quickslotParent, allWeapons, itemContainer; // Объект у которого дети являются слотами и все предметы в руках

    [Space]
    [Header("Текущий слот")]
    public int currentQuickslotID = 0;
    private int currentSmenaSlotID = -1;
    public InventorySlot activeSlot = null;

    [Space]
    [Header("Кеширование")]
    public InventorySlot currentQSlot;
    public Image currentQSlotImage;

    void Update()
    {
        float mw = Input.GetAxis("Mouse ScrollWheel"); // Используем колесико мышки

        if (mw > 0.1) // Берем предыдущий слот и меняем его картинку на обычную
        {
            quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = notSelectedSprite; // Здесь добавляем что случится когда мы УБЕРАЕМ ВЫДЕЛЕНИЕ со слота (Выключить нужный нам предмет, поменять аниматор ...)

            if (currentQuickslotID >= quickslotParent.childCount - 1) currentQuickslotID = 0; else currentQuickslotID++;// Если крутим колесиком мышки вперед и наше число currentQuickslotID равно последнему слоту, то выбираем наш первый слот (первый слот считается нулевым) // Прибавляем к числу currentQuickslotID единичку
            SelectSlot(); // Берем предыдущий слот и меняем его картинку на "выбранную" // Здесь добавляем что случится когда мы ВЫДЕЛЯЕМ слот (Включить нужный нам предмет, поменять аниматор ...)
        }
        else if (mw < -0.1) // Берем предыдущий слот и меняем его картинку на обычную
        {
            quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = notSelectedSprite; // Здесь добавляем что случится когда мы УБЕРАЕМ ВЫДЕЛЕНИЕ со слота (Выключить нужный нам предмет, поменять аниматор ...)

            if (currentQuickslotID <= 0) currentQuickslotID = quickslotParent.childCount - 1; else currentQuickslotID--; // Если крутим колесиком мышки назад и наше число currentQuickslotID равно 0, то выбираем наш последний слот // Уменьшаем число currentQuickslotID на 1
            SelectSlot(); // Берем предыдущий слот и меняем его картинку на "выбранную" // Здесь добавляем что случится когда мы ВЫДЕЛЯЕМ слот (Включить нужный нам предмет, поменять аниматор ...)
        }

        for (int i = 0; i < quickslotParent.childCount; i++) // Используем цифры
        {
            if (Input.GetKeyDown((i + 1).ToString())) // если мы нажимаем на клавиши 1 по 5 то...
            {
                currentQSlotImage = quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>();

                if (currentQuickslotID == i) // проверяем если наш выбранный слот равен слоту который у нас уже выбран, то
                {
                    if (currentQSlotImage.sprite == notSelectedSprite) SelectSlot();// Ставим картинку "selected" на слот если он "not selected" или наоборот
                    else
                    {
                        currentQSlotImage.sprite = notSelectedSprite;
                        activeSlot = null;
                        HideItemsInHand();
                        HideBuildingBlock();
                    }
                }
                else // Иначе мы убираем свечение с предыдущего слота и светим слот который мы выбираем
                {
                    currentQSlotImage.sprite = notSelectedSprite;
                    currentQuickslotID = i;
                    SelectSlot();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse0)) // Используем предмет по нажатию на левую кнопку мыши
        {
            anim.SetBool("BowAttacke", true); //натягивание тетевы

            currentQSlotImage = quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>();
            currentQSlot = quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>();

            if (currentQSlot.item != null)
            {
                if (currentQSlot.item.isConsumeable && !inventoryManager.isOpened && !inventoryManager.isOpenedEsc && !inventoryManager.isOpenedMap && currentQSlotImage.sprite == selectedSprite)
                {
                    ChangeCharacteristics(currentQSlot); // Применяем изменения к здоровью (будущем к голоду и жажде) 
                    if (buildingSystem.CanPlace())
                    {
                        buildingSystem.PlaceBlock();
                        RemoveConsumableItem(currentQSlot);
                        ShowBuildingBlock();
                    }
                }
            }
            CheckItemInHand();
        }
    }

    private void SelectSlot()
    {
        anim.SetBool("Hit", false);

        quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = selectedSprite;
        activeSlot = quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>();

        ShowItemInHand();
        ShowBuildingBlock();
    }

    private void HideBuildingBlock() => buildingSystem.enabled = false;

    public void ShowBuildingBlock()
    {
        if (activeSlot == null || activeSlot.item == null)
        {
            buildingSystem.enabled = false;
            return;
        }
        if (activeSlot.item.itemType == ItemType.BuildingBlock)
        {
            buildingSystem.enabled = true;
            buildingSystem.currentBuildingBlock = activeSlot.item.itemPrefab;
            buildingSystem.ChangeBuildingBlock();
        }
        else buildingSystem.enabled = false;
    }

    private void RemoveConsumableItem(InventorySlot currentQSlot)
    {
        if (currentQSlot.amount <= 1) quickslotParent.GetChild(currentQuickslotID).GetComponentInChildren<DragAndDropItem>().NullifySlotData();
        else
        {
            currentQSlot.amount--;
            currentQSlot.itemAmountText.text = currentQSlot.amount.ToString();
        }
    }

    public void CheckItemInHand()
    {
        if (activeSlot != null) ShowItemInHand(); else HideItemsInHand();
    }

    private void ChangeCharacteristics(InventorySlot currentQSlot)
    {
        if (currentQSlot.item.changeHunger == 0 && currentQSlot.item.changeThirst == 0 && currentQSlot.item.changeHealth == 0) return;

        indicators.ChangeFoodAmount(currentQSlot.item.changeHunger);
        indicators.ChangeWaterAmount(currentQSlot.item.changeThirst);
        indicators.ChangeHealthAmount(currentQSlot.item.changeHealth);
        indicators.ChangeColdAmount(currentQSlot.item.changeCold);

        RemoveConsumableItem(currentQSlot);
    }

    private void ShowItemInHand()
    {
        HideItemsInHand();

        if (activeSlot.item == null) return; 

        if(currentQuickslotID != currentSmenaSlotID)
        {
            anim.SetBool("Smena", true);
            currentSmenaSlotID = currentQuickslotID;
        }

        if (activeSlot.item.animType != AnimType.Default)
        {
            if (activeSlot.item.animType == AnimType.Spear) anim.SetBool("Spear", true);
            if (activeSlot.item.animType == AnimType.Bow) anim.SetBool("Bow", true);
            if (activeSlot.item.animType == AnimType.BigSwords) anim.SetBool("BigSwords", true);
        }

        for (int i = 0; i < allWeapons.childCount; i++)
        {
            if (activeSlot.item.inHandName == allWeapons.GetChild(i).name)
            {
                allWeapons.GetChild(i).gameObject.SetActive(true); //allWeapons.GetChild(i).GetComponent<ItemDurability>().inventorySlot = activeSlot;
                if (allWeapons.GetChild(i).gameObject.GetComponent<Gun>() != null) weaponManager.currentGun = allWeapons.GetChild(i).gameObject.GetComponent<Gun>();
            }
        }
    }

    public void AnimHelper()
    {
        anim.SetBool("BigSwords", false);
        anim.SetBool("Spear", false);
        anim.SetBool("Bow", false);
    }

    private void HideItemsInHand()
    {
        for (int i = 0; i < allWeapons.childCount; i++) allWeapons.GetChild(i).gameObject.SetActive(false);
        AnimHelper();
    }

    public void MobailActiveSlot(int SlotID)
    {
        currentQuickslotID = SlotID;

        currentQSlotImage = quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>();
        currentQSlot = quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>();

        SelectSlotMobail(currentQSlotImage, currentQSlot);

        anim.SetBool("Bow", false);

        if (currentQSlot.item != null)
        {
            if (currentQSlot.item.isConsumeable && !inventoryManager.isOpened && currentQSlotImage.sprite == selectedSprite)
            {
                ChangeCharacteristics(currentQSlot); // Применяем изменения к здоровью (будущем к голоду и жажде) 
                if (buildingSystem.CanPlace())
                {
                    buildingSystem.PlaceBlock();
                    RemoveConsumableItem(currentQSlot);
                    ShowBuildingBlock();
                }
            }
        }
        CheckItemInHand();
    }

    private void SelectSlotMobail(Image currentQSlotImage, InventorySlot currentQSlot)
    {
        anim.SetBool("Hit", false);

        currentQSlotImage.sprite = selectedSprite;
        activeSlot = currentQSlot;

        ShowItemInHand();
        ShowBuildingBlock();
    }

    public void ActiveSlot1() => MobailActiveSlot(0);
    public void ActiveSlot2() => MobailActiveSlot(1);
    public void ActiveSlot3() => MobailActiveSlot(2);
    public void ActiveSlot4() => MobailActiveSlot(3);
    public void ActiveSlot5() => MobailActiveSlot(4);
    public void ActiveSlot6() => MobailActiveSlot(5);
    public void ActiveSlot7() => MobailActiveSlot(6);
}
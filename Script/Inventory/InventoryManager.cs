using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour // https://www.youtube.com/c/maximple, https://www.youtube.com/channel/UC2pH2qNfh2sEAeYEGs1k_Lg
{
    [Space]
    [Header("Скрипты и аниматор")]
    public CustomCharacterController customCharacterController;
    public MenuManagerm menuManagerm;
    public VerstAddItem verstAddItem;
    public FractionManager fractionManager;
    public CraftManager craftManager;
    public Money money;
    public Animator anim;

    [Space]
    [Header("Камера")]
    private Camera mainCamera;
    public CinemachineVirtualCamera CVC, CVC3;
    private CinemachinePOV pov, pov3;
    public float reachDistance = 8f, sensitivity;
    public LayerMask layerMask;

    [Space]
    [Header("Объекты")]
    public GameObject players;
    public GameObject craftPanel, UIBG, chat, crosshair, View1, View3, RenderModel, MapCamera;

    [Space]
    [Header("Позиции")]
    public Transform player;
    public Transform inventoryPanel, inventoryAndClothingPanel, ESCPanel, MapPanel, VerstacPanel, verstacLvl1, verstacLvl2, verstacLvl3, verstacLvl4, bioTex, quickslotPanel;

    [Space]
    [Header("Текст")]
    public TMP_Text pingText;

    [Space]
    [Header("Буллы")]
    public bool isOpened;
    public bool isOpenedEsc, isOpenedMap, ViewMode = true, isMobile;

    [Space]
    [Header("Листы")]
    public List<InventorySlot> slots = new List<InventorySlot>();
    public List<ClothAdder> _clothAdders = new List<ClothAdder>();

    private void Awake() => UIBG.SetActive(true);

    void Start()
    {
        fractionManager.FractionClose();
        mainCamera = Camera.main;
        MapCamera.transform.position = new Vector3(0, 1150, 200);

        if (customCharacterController.PC == false) isMobile = true; else isMobile = false;

        slots.AddRange(inventoryAndClothingPanel.GetComponentsInChildren<InventorySlot>());
        for (int i = 0; i < quickslotPanel.childCount; i++) if (quickslotPanel.GetChild(i).GetComponent<InventorySlot>() != null) slots.Add(quickslotPanel.GetChild(i).GetComponent<InventorySlot>());

        if (PlayerPrefs.HasKey("Sensitivity")) sensitivity = PlayerPrefs.GetFloat("Sensitivity"); else sensitivity = 1f;

        pov = CVC.GetCinemachineComponent<CinemachinePOV>();
        pov3 = CVC3.GetCinemachineComponent<CinemachinePOV>();

        pov.m_HorizontalAxis.m_MaxSpeed = sensitivity;
        pov.m_VerticalAxis.m_MaxSpeed = sensitivity;
        pov3.m_HorizontalAxis.m_MaxSpeed = sensitivity;
        pov3.m_VerticalAxis.m_MaxSpeed = sensitivity;

        EditCurcorOff();
    }

    public void ToInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            isOpened = !isOpened;
            craftManager.isOpened = false;
            isOpenedEsc = false;
            isOpenedMap = false;

            craftPanel.gameObject.SetActive(false);
            ESCPanel.gameObject.SetActive(false);
            MapPanel.gameObject.SetActive(false);
            VerstacPanel.gameObject.SetActive(false);
            RenderModel.SetActive(false);
            MapCamera.SetActive(false);

            DragAndDropItem[] dadi = FindObjectsOfType<DragAndDropItem>();
            foreach (DragAndDropItem slot in dadi) slot.ReturnBackToSlot();

            if (isOpened)
            {
                anim.SetBool("Hit", false);

                fractionManager.FractionClose();
                RenderModel.SetActive(true);
                MapCamera.SetActive(false);
                inventoryAndClothingPanel.gameObject.SetActive(true); // new line

                CameraLock();
            }
            else
            {
                RenderModel.SetActive(false);
                MapCamera.SetActive(false);
                inventoryAndClothingPanel.gameObject.SetActive(false); // new line

                CameraOnLock();
            }
        }
    }

    public void ToEcs()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isOpenedEsc = !isOpenedEsc;
            isOpened = false;
            isOpenedMap = false;
            craftManager.isOpened = false;

            RenderModel.SetActive(false);
            MapCamera.SetActive(false);
            craftPanel.SetActive(false);
            inventoryAndClothingPanel.gameObject.SetActive(false);
            MapPanel.gameObject.SetActive(false);
            VerstacPanel.gameObject.SetActive(false);

            if (isOpenedEsc)
            {
                //pingText.text = PhotonNetwork.GetPing().ToString();

                anim.SetBool("Hit", false);

                fractionManager.FractionClose();
                ESCPanel.gameObject.SetActive(true); // new line
                RenderModel.SetActive(false);
                MapCamera.SetActive(false);

                CameraLock();

                money.ScoreUpdate();
            }
            else
            {
                RenderModel.SetActive(false);
                MapCamera.SetActive(false);
                ESCPanel.gameObject.SetActive(false); // new line

                CameraOnLock();
            }
        }
    }

    public void ToMap()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            fractionManager.FractionClose();
            isOpenedMap = !isOpenedMap;
            isOpened = false;
            isOpenedEsc = false;
            craftManager.isOpened = false;

            RenderModel.SetActive(false);
            MapCamera.SetActive(false);
            craftPanel.gameObject.SetActive(false);
            ESCPanel.gameObject.SetActive(false);
            inventoryAndClothingPanel.gameObject.SetActive(false);
            VerstacPanel.gameObject.SetActive(false);

            if (isOpenedMap)
            {
                anim.SetBool("Hit", false);

                MapCamera.SetActive(true);
                MapPanel.gameObject.SetActive(true);

                CameraLock();
            }
            else
            {
                MapCamera.SetActive(false);
                MapPanel.gameObject.SetActive(false);

                CameraOnLock();
            }
        }
    }

    public void InputE()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.SphereCast(ray, 2, out hit, reachDistance, layerMask))
            {
                fractionManager.FractionClose();
                VerstacPanel.gameObject.SetActive(false);

                if (hit.collider.gameObject.CompareTag("Item"))
                {
                    Item itemscript = hit.collider.gameObject.GetComponent<Item>();
                    anim.SetBool("addItem", true);
                    AddItem(itemscript.item, itemscript.amount);

                    craftManager.currentCraftItem.FillItemDetails();

                    if(itemscript.optimize == false)
                    {
                        Destroy(hit.collider.gameObject);
                    }
                    else
                    {
                        hit.collider.gameObject.SetActive(false); //для кустов
                    }
                }
                else
                {
                    if (hit.collider.gameObject.CompareTag("FractionMoney"))
                    {
                        MoneyItem money = hit.collider.gameObject.GetComponent<MoneyItem>();
                        fractionManager.AddMoney(money);
                        anim.SetBool("addItem", true);

                        Destroy(hit.collider.gameObject);
                    }
                    else
                    {
                        if (hit.collider.gameObject.CompareTag("FractionNps"))
                        {
                            CameraLock();

                            FractionNPS fractionNps = hit.collider.gameObject.GetComponent<FractionNPS>();
                            fractionManager.currentFraction = fractionNps.NPSFraction;
                            if (fractionNps.myFraction.FractionReputation >= 0) fractionManager.FractionFunc();
                        }
                        else
                        {
                            if (hit.collider.gameObject.CompareTag("Verstac"))
                            {
                                verstacLvl1.gameObject.SetActive(true);
                                VerstacPanels();
                            }
                            else
                            {
                                verstacLvl1.gameObject.SetActive(false);
                                if (hit.collider.gameObject.CompareTag("Verstac2"))
                                {
                                    verstacLvl2.gameObject.SetActive(true);
                                    VerstacPanels();
                                }
                                else
                                {
                                    verstacLvl2.gameObject.SetActive(false);
                                    if (hit.collider.gameObject.CompareTag("Verstac3"))
                                    {
                                        verstacLvl3.gameObject.SetActive(true);
                                        VerstacPanels();
                                    }
                                    else
                                    {
                                        verstacLvl3.gameObject.SetActive(false);
                                        if (hit.collider.gameObject.CompareTag("Verstac4"))
                                        {
                                            verstacLvl4.gameObject.SetActive(true);
                                            VerstacPanels();
                                        }
                                        else
                                        {
                                            verstacLvl4.gameObject.SetActive(false);
                                            if (hit.collider.gameObject.CompareTag("BioTex"))
                                            {
                                                bioTex.gameObject.SetActive(true);
                                                VerstacPanels();
                                            }
                                            else
                                            {
                                                bioTex.gameObject.SetActive(false);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void VerstacPanels()
    {
        isOpenedMap = false;
        isOpened = false;
        isOpenedEsc = false;
        craftManager.isOpened = false;

        RenderModel.SetActive(false);
        MapCamera.SetActive(false);
        craftPanel.gameObject.SetActive(false);
        ESCPanel.gameObject.SetActive(false);
        inventoryAndClothingPanel.gameObject.SetActive(false);

        anim.SetBool("Hit", false);

        VerstacPanel.gameObject.SetActive(true);
        verstAddItem.StartFunc();

        CameraLock();
    }

    public void CameraLock()
    {
        UIBG.SetActive(true);
        crosshair.SetActive(false);

        pov.m_HorizontalAxis.m_InputAxisName = "";
        pov.m_VerticalAxis.m_InputAxisName = "";
        pov.m_HorizontalAxis.m_InputAxisValue = 0;
        pov.m_VerticalAxis.m_InputAxisValue = 0;
        pov3.m_HorizontalAxis.m_InputAxisName = "";
        pov3.m_VerticalAxis.m_InputAxisName = "";
        pov3.m_HorizontalAxis.m_InputAxisValue = 0;
        pov3.m_VerticalAxis.m_InputAxisValue = 0;

        Cursor.lockState = CursorLockMode.None; // Открепляем курсор от середине экрана
        Cursor.visible = true; // и делаем его невидимым
    }

    public void CameraOnLock()
    {
        UIBG.SetActive(false);
        crosshair.SetActive(true);

        pov.m_HorizontalAxis.m_InputAxisName = "Mouse X";
        pov.m_VerticalAxis.m_InputAxisName = "Mouse Y";
        pov3.m_HorizontalAxis.m_InputAxisName = "Mouse X";
        pov3.m_VerticalAxis.m_InputAxisName = "Mouse Y";

        Cursor.lockState = CursorLockMode.Locked; // Прекрепляем курсор к середине экрана
        Cursor.visible = false; // и делаем его видимым
    }

    public void RemoveItemFromSlot(int slotId)
    {
        InventorySlot slot = slots[slotId];
        if (slot.clothType != ClothType.None && !slot.isEmpty) foreach (ClothAdder clothAdder in _clothAdders) clothAdder.RemoveClothes(slot.item.clothingPrefab);
        slot.item = null;
        slot.isEmpty = true;
        slot.amount = 0;
        slot.iconGO.color = new Color(1, 1, 1, 0);
        slot.iconGO.sprite = null;
        slot.itemAmountText.text = "";
    }

    public void AddItemToSlot(ItemScriptableObject _item, int _amount, int slotId)
    {
        InventorySlot slot = slots[slotId];
        slot.item = _item;
        slot.isEmpty = false;
        slot.SetIcon(_item.icon);
        if (_amount <= _item.maximumAmount)
        {
            slot.amount = _amount;
            if (slot.item.maximumAmount != 1) slot.itemAmountText.text = slot.amount.ToString();
        }
        else
        {
            slot.amount = _item.maximumAmount;
            _amount -= _item.maximumAmount;
            if (slot.item.maximumAmount != 1) slot.itemAmountText.text = slot.amount.ToString();
        }
        if (slot.clothType != ClothType.None) foreach (ClothAdder clothAdder in _clothAdders) clothAdder.addClothes(slot.item.clothingPrefab);
    }

    public void AddItem(ItemScriptableObject _item, int _amount)
    {
        int amount = _amount;
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == _item) // Стакаем предметы вместе // В слоте уже имеется этот предмет
            {
                if (slot.amount + amount <= _item.maximumAmount)
                {
                    slot.amount += amount;
                    slot.itemAmountText.text = slot.amount.ToString();
                    return;
                }
                else
                {
                    amount -= _item.maximumAmount - slot.amount;
                    slot.amount = _item.maximumAmount;
                    slot.itemAmountText.text = slot.amount.ToString();
                }
                continue;
            }
        }
        bool allFull = true;
        foreach (InventorySlot inventorySlot in slots)
        {
            if (inventorySlot.isEmpty)
            {
                allFull = false;
                break;
            }
        }
        if (allFull)
        {
            Item items = Instantiate(_item.itemPrefab, player.position + Vector3.up + player.forward, Quaternion.identity).GetComponent<Item>();
            items.amount = _amount;
        }
        foreach (InventorySlot slot in slots)
        {
            if (amount <= 0) return;
            if (slot.isEmpty == true) // добавляем предметы в свободные ячейки
            {
                slot.item = _item; //slot.amount = amount;
                slot.isEmpty = false;
                slot.SetIcon(_item.icon);
                if (amount <= _item.maximumAmount)
                {
                    slot.amount = amount;
                    if (slot.item.maximumAmount != 1) slot.itemAmountText.text = slot.amount.ToString(); break;// added this if statement for single items
                }
                else
                {
                    slot.amount = _item.maximumAmount;
                    amount -= _item.maximumAmount;
                    if (slot.item.maximumAmount != 1) slot.itemAmountText.text = slot.amount.ToString();// added this if statement for single items
                }
                allFull = true;
                foreach (InventorySlot inventorySlot in slots)
                {
                    if (inventorySlot.isEmpty)
                    {
                        allFull = false;
                        break;
                    }
                }
                if (allFull)
                {
                    Item items = Instantiate(_item.itemPrefab, player.position + Vector3.up + player.forward, Quaternion.identity).GetComponent<Item>();
                    items.amount = _amount;
                    return;
                } //continue;
            }
        }
    }

    public void MobailItems()
    {
        anim.SetBool("Hit", false);

        Vector3 Ray_start_pos = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = mainCamera.ScreenPointToRay(Ray_start_pos);
        RaycastHit hit;

        if (Physics.SphereCast(ray, 2, out hit, reachDistance, layerMask))
        {
            fractionManager.FractionClose();
            VerstacPanel.gameObject.SetActive(false);

            if (hit.collider.gameObject.CompareTag("Item"))
            {
                Item itemscript = hit.collider.gameObject.GetComponent<Item>();
                anim.SetBool("addItem", true);
                AddItem(itemscript.item, itemscript.amount);

                craftManager.currentCraftItem.FillItemDetails();

                Destroy(hit.collider.gameObject);
            }
            else
            {
                if (hit.collider.gameObject.CompareTag("FractionMoney"))
                {
                    MoneyItem money = hit.collider.gameObject.GetComponent<MoneyItem>();
                    fractionManager.AddMoney(money);
                    anim.SetBool("addItem", true);

                    Destroy(hit.collider.gameObject);
                }
                else
                {
                    if (hit.collider.gameObject.CompareTag("FractionNps"))
                    {
                        CameraLock();

                        FractionNPS fractionNps = hit.collider.gameObject.GetComponent<FractionNPS>();
                        fractionManager.currentFraction = fractionNps.NPSFraction;
                        if (fractionNps.myFraction.FractionReputation >= 0) fractionManager.FractionFunc();
                    }
                    else
                    {
                        if (hit.collider.gameObject.CompareTag("Verstac"))
                        {
                            verstacLvl1.gameObject.SetActive(true);
                            VerstacPanels();
                        }
                        else
                        {
                            verstacLvl1.gameObject.SetActive(false);
                            if (hit.collider.gameObject.CompareTag("Verstac2"))
                            {
                                verstacLvl2.gameObject.SetActive(true);
                                VerstacPanels();
                            }
                            else
                            {
                                verstacLvl2.gameObject.SetActive(false);
                                if (hit.collider.gameObject.CompareTag("Verstac3"))
                                {
                                    verstacLvl3.gameObject.SetActive(true);
                                    VerstacPanels();
                                }
                                else
                                {
                                    verstacLvl3.gameObject.SetActive(false);
                                    if (hit.collider.gameObject.CompareTag("Verstac4"))
                                    {
                                        verstacLvl4.gameObject.SetActive(true);
                                        VerstacPanels();
                                    }
                                    else
                                    {
                                        verstacLvl4.gameObject.SetActive(false);
                                        if (hit.collider.gameObject.CompareTag("BioTex"))
                                        {
                                            bioTex.gameObject.SetActive(true);
                                            VerstacPanels();
                                        }
                                        else
                                        {
                                            bioTex.gameObject.SetActive(false);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void EditCurcorOn()
    {
        UIBG.SetActive(true);
        crosshair.SetActive(false);

        Cursor.lockState = CursorLockMode.None; // Прекрепляем курсор к середине экрана
        Cursor.visible = true; // и делаем его невидимым

        pov.m_HorizontalAxis.m_InputAxisName = "";
        pov.m_VerticalAxis.m_InputAxisName = "";
        pov.m_HorizontalAxis.m_InputAxisValue = 0;
        pov.m_VerticalAxis.m_InputAxisValue = 0;
        pov3.m_HorizontalAxis.m_InputAxisName = "";
        pov3.m_VerticalAxis.m_InputAxisName = "";
        pov3.m_HorizontalAxis.m_InputAxisValue = 0;
        pov3.m_VerticalAxis.m_InputAxisValue = 0;
    }

    public void EditCurcorOff()
    {
        UIBG.SetActive(false);
        isOpened = false;
        isOpenedEsc = false;
        isOpenedMap = false;
        craftManager.isOpened = false;

        MapCamera.SetActive(false);
        RenderModel.SetActive(false);
        crosshair.SetActive(true);
        craftPanel.SetActive(false);
        fractionManager.FractionClose();
        inventoryAndClothingPanel.gameObject.SetActive(false);
        ESCPanel.gameObject.SetActive(false);
        MapPanel.gameObject.SetActive(false);
        VerstacPanel.gameObject.SetActive(false);

        DragAndDropItem[] dadi = FindObjectsOfType<DragAndDropItem>();
        foreach (DragAndDropItem slot in dadi) slot.ReturnBackToSlot();

        if (isMobile == false)
        {
            Cursor.lockState = CursorLockMode.Locked; // Прекрепляем курсор к середине экрана
            Cursor.visible = false; // и делаем его невидимым
            pov.m_HorizontalAxis.m_InputAxisName = "Mouse X";
            pov.m_VerticalAxis.m_InputAxisName = "Mouse Y";
            pov3.m_HorizontalAxis.m_InputAxisName = "Mouse X";
            pov3.m_VerticalAxis.m_InputAxisName = "Mouse Y";
        }
    }

    public void EquipView1()
    {
        View1.SetActive(true);
        View3.SetActive(false);

        reachDistance = 8;
        ViewMode = false;
    }

    public void EquipView3()
    {
        View1.SetActive(false);
        View3.SetActive(true);

        reachDistance = 13;
        ViewMode = true;
    }
}
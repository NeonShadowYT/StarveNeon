using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using TMPro;

public class CraftManager : MonoBehaviour // https://www.youtube.com/c/maximple, https://www.youtube.com/channel/UC2pH2qNfh2sEAeYEGs1k_Lg
{
    [Space]
    [Header("Скрипты")]
    public InventoryManager inventoryManager;
    private CustomCharacterController customCharacterController;
    public CraftQueueManager craftQueueManager;

    [Space]
    [Header("Камера")]
    public CinemachineVirtualCamera CVC, CVC3;
    private CinemachinePOV pov, pov3;
    private float sensitivity;

    [Space]
    [Header("Текущая информация о крафте")]
    public FillCraftItemDetails currentCraftItem;

    [Space]
    [Header("Крафт предмета детали")]
    public Image craftItemImage;
    public TMP_Text craftItemName, craftItemDescription, craftItemDuration, craftItemAmount;

    [Space]
    [Header("Все крафты")]
    public List<CraftScriptableObject> allCrafts;

    [Space]
    [Header("Объекты")]
    public GameObject craftingPanel, inventoryAndClothingPanel, ESCPanel, MapPanel, VerstPanel, craftInfoPanel;
    public GameObject craftItemButtonPrefab;
    public GameObject UIBG;
    public GameObject crosshair;
    public GameObject RenderModel, MapCamera;

    [Space]
    [Header("Информация")]
    public Transform craftItemsPanel;
    public Button craftBtn;

    public bool isOpened;
    private bool isMobile = false;

    void Start()
    {
        customCharacterController = inventoryManager.customCharacterController;
        if (customCharacterController.PC == false) isMobile = true; else isMobile = false;

        GameObject craftItemButton = Instantiate(craftItemButtonPrefab, craftItemsPanel);
        FillCraftItemDetails fillCraftItemDetails = craftItemButton.GetComponent<FillCraftItemDetails>(); //закешировал для оптимизации

        craftItemButton.GetComponent<Image>().sprite = allCrafts[0].finalCraft.icon;
        fillCraftItemDetails.craftManager = this;
        fillCraftItemDetails.craftQueueManager = craftQueueManager;
        fillCraftItemDetails.currentCraftItem = allCrafts[0];
        fillCraftItemDetails.craftInfoPanel = craftInfoPanel;
        fillCraftItemDetails.inventoryManager = inventoryManager;

        fillCraftItemDetails.FillItemDetails();
        Destroy(craftItemButton);

        craftingPanel.gameObject.SetActive(false);

        if (PlayerPrefs.HasKey("Sensitivity")) sensitivity = PlayerPrefs.GetFloat("Sensitivity"); else sensitivity = 1f;

        pov = CVC.GetCinemachineComponent<CinemachinePOV>();
        pov3 = CVC3.GetCinemachineComponent<CinemachinePOV>();

        pov.m_HorizontalAxis.m_MaxSpeed = sensitivity;
        pov.m_VerticalAxis.m_MaxSpeed = sensitivity;
        pov3.m_HorizontalAxis.m_MaxSpeed = sensitivity;
        pov3.m_VerticalAxis.m_MaxSpeed = sensitivity;
        pov.m_HorizontalAxis.m_InputAxisName = "Mouse X";
        pov.m_VerticalAxis.m_InputAxisName = "Mouse Y";
        pov3.m_HorizontalAxis.m_InputAxisName = "Mouse X";
        pov3.m_VerticalAxis.m_InputAxisName = "Mouse Y";

        if (isMobile)
        {
            pov.m_HorizontalAxis.m_InputAxisName = "";
            pov.m_VerticalAxis.m_InputAxisName = "";
            pov3.m_HorizontalAxis.m_InputAxisName = "";
            pov3.m_VerticalAxis.m_InputAxisName = "";
        }
    }

    public void FillItemDetailsHelper() => currentCraftItem.FillItemDetails();

    public void ToCraft()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isOpened = !isOpened;
            inventoryManager.isOpened = false;
            inventoryManager.isOpenedEsc = false;
            inventoryManager.isOpenedMap = false;

            inventoryAndClothingPanel.SetActive(false);
            MapPanel.SetActive(false);
            ESCPanel.SetActive(false);
            RenderModel.SetActive(false);
            MapCamera.SetActive(false);

            if (isOpened)
            {
                RenderModel.SetActive(false);
                MapCamera.SetActive(false);
                craftingPanel.SetActive(true);
                VerstPanel.SetActive(false);
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

                Cursor.lockState = CursorLockMode.None; // Прекрепляем курсор к середине экрана
                Cursor.visible = true; // и делаем его невидимым
            }
            else
            {
                RenderModel.SetActive(false);
                MapCamera.SetActive(false);
                craftingPanel.SetActive(false);
                VerstPanel.SetActive(false);
                UIBG.SetActive(false);
                crosshair.SetActive(true);

                pov.m_HorizontalAxis.m_InputAxisName = "Mouse X";
                pov.m_VerticalAxis.m_InputAxisName = "Mouse Y";
                pov3.m_HorizontalAxis.m_InputAxisName = "Mouse X";
                pov3.m_VerticalAxis.m_InputAxisName = "Mouse Y";

                Cursor.lockState = CursorLockMode.Locked; // Прекрепляем курсор к середине экрана
                Cursor.visible = false; // и делаем его невидимым
            }
        }
    }

    public void LoadCraftItems(string craftType)
    {
        for (int i = 0; i < craftItemsPanel.childCount; i++) Destroy(craftItemsPanel.GetChild(i).gameObject);
        foreach (CraftScriptableObject cso in allCrafts)
        {
            if (cso.craftType.ToString().ToLower() == craftType.ToLower())
            {
                GameObject craftItemButton = Instantiate(craftItemButtonPrefab, craftItemsPanel);
                craftItemButton.GetComponent<Image>().sprite = cso.finalCraft.icon;
                FillCraftItemDetails fillCraftItemDetails = craftItemButton.GetComponent<FillCraftItemDetails>(); //закешировал для оптимизации

                fillCraftItemDetails.currentCraftItem = cso;
                fillCraftItemDetails.craftManager = this;
                fillCraftItemDetails.craftQueueManager = craftQueueManager;
                fillCraftItemDetails.craftInfoPanel = craftInfoPanel;
                fillCraftItemDetails.inventoryManager = inventoryManager;
            }
        }
    }
}
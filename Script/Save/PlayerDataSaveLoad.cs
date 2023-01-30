using UnityEngine;
public class PlayerDataSaveLoad : MonoBehaviour //https://www.youtube.com/channel/UC2pH2qNfh2sEAeYEGs1k_Lg
{
    [SerializeField] private MoneySlot texnoSlot;
    [SerializeField] private Indicators _indicators;
    [SerializeField] private CustomCharacterController _customCharacterController;
    [SerializeField] private InventoryManager _inventoryManager;

    private SceneDataSaveLoad sceneDataSaveLoad;

    public void SavePlayer()
    {
        BinarySavingSystem.SavePlayer(_indicators, _customCharacterController, _inventoryManager, texnoSlot);

        if (sceneDataSaveLoad == null)
        {
            sceneDataSaveLoad = FindObjectOfType<SceneDataSaveLoad>().GetComponent<SceneDataSaveLoad>();
        }
        sceneDataSaveLoad.SaveScene();
    }
    public void LoadPlayer()
    {
        if (sceneDataSaveLoad == null)
        {
            sceneDataSaveLoad = FindObjectOfType<SceneDataSaveLoad>().GetComponent<SceneDataSaveLoad>();
        }
        sceneDataSaveLoad.LoadScene();

        PlayerData data = BinarySavingSystem.LoadPlayer();

        _indicators.healthAmount = data.health;
        _indicators.waterAmount = data.water;
        _indicators.foodAmount = data.food;
        _indicators.сoldAmount = data.cold;

        texnoSlot.amount = data.texnoAmount;
        _indicators.daytime.Day = data.DayInt;
        _customCharacterController.transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);

        for (int i = 0; i < _inventoryManager.slots.Count; i++)
        {
            if (data.itemNames[i] != null)
            {
                _inventoryManager.RemoveItemFromSlot(i);
                ItemScriptableObject item = Resources.Load<ItemScriptableObject>($"ScriptableObjects/{data.itemNames[i]}");
                int itemAmount = data.itemAmounts[i];
                _inventoryManager.AddItemToSlot(item, itemAmount, i);
            }
            else _inventoryManager.RemoveItemFromSlot(i);
        }
    }
}

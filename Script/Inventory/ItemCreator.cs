using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Items/New Item")]
public class ItemCreator : ItemScriptableObject //https://www.youtube.com/channel/UC2pH2qNfh2sEAeYEGs1k_Lg
{
    private void Start() => itemType = ItemType.Food;
}

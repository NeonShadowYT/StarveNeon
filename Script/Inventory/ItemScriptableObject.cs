using UnityEngine;

public enum ItemType { Default, Food, Weapon, Instrument, BuildingBlock }
public enum AnimType { Default, Spear, BigSwords, Bow }
public enum ClothType { None, Head, Body, BodyArmor, Legs, Feet }
public enum SpeedAttacke { Medium, Low, Max }

public class ItemScriptableObject : ScriptableObject // https://www.youtube.com/c/maximple, https://www.youtube.com/channel/UC2pH2qNfh2sEAeYEGs1k_Lg
{
    [Space]
    [Header("Название предмета")]
    public string itemName;
    public string inHandName;

    [Space]
    [Header("Иконка")]
    public Sprite icon;

    [Space]
    [Header("Максимальное количество в слоте")]
    public int maximumAmount;

    [Space]
    [Header("Описание предмета")]
    public string itemDescription;

    [Space]
    [Header("Префаб предмета и одежды")]
    public GameObject itemPrefab;
    public GameObject clothingPrefab;

    [Space]
    [Header("Тип предмета")]
    public ItemType itemType;
    public AnimType animType = AnimType.Default;
    public ClothType clothType = ClothType.None;
    public SpeedAttacke speedAttacke = SpeedAttacke.Medium;

    [Space]
    [Header("Предмет тратится?")]
    public bool isConsumeable;
    public bool canBreak;

    [Space]
    [Header("Получаемые характеристики")]
    public float changeHealth;
    public float changeHunger, changeThirst, changeCold;

    [Space]
    public int AnLockScore;
}

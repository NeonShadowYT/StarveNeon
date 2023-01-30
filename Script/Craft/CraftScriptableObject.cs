using System.Collections.Generic;
using UnityEngine;

public enum CraftType { Common, Tools, Weapons, Armor, Buildings }

public class CraftScriptableObject : ScriptableObject // https://www.youtube.com/c/maximple, https://www.youtube.com/channel/UC2pH2qNfh2sEAeYEGs1k_Lg
{
    [Space]
    [Header("Тип крафта")]
    public CraftType craftType;

    [Space]
    [Header("Информация")]
    public ItemScriptableObject finalCraft;

    [Space]
    public int craftAmount;
    public int craftTime;

    [Space]
    [Header("Предметы для крафта")]
    public List<CraftResource> craftResources;
}

[System.Serializable]
public class CraftResource
{
    [Space]
    [Header("Предмет для крафта и его количество")]
    public ItemScriptableObject craftObject;
    public int craftObjectAmount;
}
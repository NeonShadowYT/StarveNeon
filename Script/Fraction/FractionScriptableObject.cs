using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FractionRace { Starver, GC, Ksencse, Oboroten }
public class FractionScriptableObject : ScriptableObject
{
    [Space]
    [Header("Раса фракции и её монеты")]
    public FractionRace fractionRace = FractionRace.Starver;
    public MoneyScriptableObject fractionMoney;

    [Space]
    [Header("Информация о фракции")]
    public Sprite FractionIcon;
    public string FractionName;

    public int FractionDayUpdate;
    public int FractionStartReputation;

    [Space]
    [Header("Товары")]
    public List<TovareList> Tovare;
}
[System.Serializable]
public class TovareList
{
    [Space]
    [Header("Товар и его цена")]
    public ItemScriptableObject TovareItem;
    public int TovarePrice;
}
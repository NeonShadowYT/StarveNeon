using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FractionRace { Starver, GC, Ksencse, Oboroten }
public class FractionScriptableObject : ScriptableObject
{
    [Space]
    [Header("���� ������� � � ������")]
    public FractionRace fractionRace = FractionRace.Starver;
    public MoneyScriptableObject fractionMoney;

    [Space]
    [Header("���������� � �������")]
    public Sprite FractionIcon;
    public string FractionName;

    public int FractionDayUpdate;
    public int FractionStartReputation;

    [Space]
    [Header("������")]
    public List<TovareList> Tovare;
}
[System.Serializable]
public class TovareList
{
    [Space]
    [Header("����� � ��� ����")]
    public ItemScriptableObject TovareItem;
    public int TovarePrice;
}
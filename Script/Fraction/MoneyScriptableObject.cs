using UnityEngine;

public enum MoneyType { GCMoney, KsencseMoney, OborotenMoney, Texno, BioTexno, Plants }
public class MoneyScriptableObject : ScriptableObject
{
    public MoneyType money;
    public GameObject moneyPrefab;
    public Sprite icon;
}

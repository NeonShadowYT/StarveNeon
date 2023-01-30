using System.Collections.Generic;
using UnityEngine;

public class FractionNPS : MonoBehaviour
{
    [Space]
    [Header("Скрипты")]
    public Fraction myFraction;
    public FractionScriptableObject NPSFraction;
    public MobStats mobStats;
    public AIManager aIManager;

    [Space]
    [Header("Оружие")]
    public List<GameObject> allWeaponObj;

    public void Startnps()
    {
        if (mobStats == null) mobStats = GetComponent<MobStats>();
        int randomNum = Random.Range(0, allWeaponObj.Count);
        GameObject myWeapon = allWeaponObj[randomNum];
        myWeapon.SetActive(true);

        CheckRep();
    }

    public void CheckRep()
    {
        if (myFraction.FractionReputation < 0) aIManager.AgreAI = true; else aIManager.AgreAI = false;
    }
}

using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    [Space]
    [Header("�������� �������")]
    public FractionManager fractionManager;

    [Space]
    [Header("��� ����� � ��������")]
    public List<MoneySlot> AllMoneySlot;

    public void CheckCurrentMoney(FractionScriptableObject currentFraction)
    {
        foreach (MoneySlot mos in AllMoneySlot)
        {
            if (mos.money == currentFraction.fractionMoney)
            {
                fractionManager.currentMoneySlot = mos;
                mos.gameObject.SetActive(true);
            }
            else mos.gameObject.SetActive(false);
        }
    }
    public void CurrentAddMoney(MoneyItem money)
    {
        foreach (MoneySlot mos in AllMoneySlot) if (mos.money == money.money) mos.amount += money.amount;
    }
}

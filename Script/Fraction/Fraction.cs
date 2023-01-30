using UnityEngine;
using System.Collections.Generic;

public class Fraction : MonoBehaviour
{
    [Space]
    [Header("���������� � �������")]
    public FractionScriptableObject MyFraction;
    public int _FractionReputation;
    public int FractionReputation
    {
        get { return _FractionReputation; }
        set
        {
            _FractionReputation = value;
            foreach (FractionNPS fractNps in fractionNps)
            {
                fractNps.CheckRep();
            }
        }
    }

    [Space]
    public GameObject[] spawnerNps;

    [Space]
    [Header("��������� �������")]
    public int currentLvl = 1;
    public GameObject currentFractionObj;

    public List<FractionUpdate> fractionUpdate;

    [Space]
    [Header("���")]
    private List<FractionNPS> fractionNps = new List<FractionNPS>();

    public void Start()
    {

        FractionReputation = MyFraction.FractionStartReputation; // ��������� ��������� ������� 

        foreach (GameObject spawner in spawnerNps)
        {
            fractionNps.AddRange(spawner.GetComponentsInChildren<FractionNPS>()); //��������� ���� ���
        }

        foreach (FractionUpdate fractup in fractionUpdate)
        {
            fractup.NewFractionObj.SetActive(false); // �������� ����������� ����
        }

        foreach (FractionNPS fractNps in fractionNps)
        {
            fractNps.myFraction = this; // ������������� ��� ��� ��� �������
            fractNps.Startnps(); // ����� ���������� ������ � �������� ���������
        }
    }

    public void UpdateLvlFract()
    {
        foreach (FractionUpdate fractup in fractionUpdate)
        {
            if (fractup.Lvl == (currentLvl + 1))
            {
                currentFractionObj.SetActive(false);
                fractup.NewFractionObj.SetActive(true);

                currentFractionObj = fractup.NewFractionObj;
                currentLvl = fractup.Lvl;

                return;
            }
        }
    }
}
[System.Serializable]
public class FractionUpdate
{
    [Space]
    [Header("���������� � ������")]
    public int Lvl;
    public int DayUpdate;

    public GameObject NewFractionObj;
}
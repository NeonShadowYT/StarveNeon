using UnityEngine;
using System.Collections.Generic;

public class Fraction : MonoBehaviour
{
    [Space]
    [Header("Информация о фракции")]
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
    [Header("Улучшение фракции")]
    public int currentLvl = 1;
    public GameObject currentFractionObj;

    public List<FractionUpdate> fractionUpdate;

    [Space]
    [Header("Нпс")]
    private List<FractionNPS> fractionNps = new List<FractionNPS>();

    public void Start()
    {

        FractionReputation = MyFraction.FractionStartReputation; // стартовая репутацию фракции 

        foreach (GameObject spawner in spawnerNps)
        {
            fractionNps.AddRange(spawner.GetComponentsInChildren<FractionNPS>()); //добавляем всех нпс
        }

        foreach (FractionUpdate fractup in fractionUpdate)
        {
            fractup.NewFractionObj.SetActive(false); // скрываем прокаченные лвлы
        }

        foreach (FractionNPS fractNps in fractionNps)
        {
            fractNps.myFraction = this; // устанавливаем для нпс эту фракцию
            fractNps.Startnps(); // выбор рандомного оручия и проверка репутации
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
    [Header("Информация о уровне")]
    public int Lvl;
    public int DayUpdate;

    public GameObject NewFractionObj;
}
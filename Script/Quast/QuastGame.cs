using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuastGame : MonoBehaviour
{
    public PassManager passManager;

    public List<QuastScriptableObject> AcceptQuastList;
    public List<int> quastAmount;

    public void Start()
    {
        Load();
    }

    public void Load()
    {
        QuastData data = BinarySavingSystem.LoadQuast();

        if(data.quastNames != null)
        {
            for (int i = 0; i < 5; i++)
            {
                QuastScriptableObject quastScriptableObject = Resources.Load<QuastScriptableObject>($"Quast/{data.quastNames[i]}");

                AcceptQuastList.Add(quastScriptableObject);
                quastAmount.Add(0);
            }
        }
    }

    public void AddQuastReward(int i)
    {
        passManager.ClaimGameReward(AcceptQuastList[i]);

        AcceptQuastList.Remove(AcceptQuastList[i]);
        quastAmount.Remove(quastAmount[i]);
    }
}
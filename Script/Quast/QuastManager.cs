using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuastManager : MonoBehaviour
{
    public List<QuastScriptableObject> QuastList;

    public List<QuastScriptableObject> AcceptQuastList;

    public PassManager passManager;

    public Transform QuastContent;
    public GameObject QuastButtonPrefab;

    void Start()
    {
        foreach (QuastScriptableObject quast in QuastList)
        {
            int random = Random.Range(0, 2);

            if (random == 1)
            {
                QuastButton quastButton = Instantiate(QuastButtonPrefab, QuastContent).GetComponent<QuastButton>();

                quastButton.quastManager = this;
                quastButton.quastScriptableObject = quast;

                quastButton.ButtonText.text = quast.QuastText;
                quastButton.RewardText.text = quast.PassMoneyReward.ToString();
            }
        }

        BinarySavingSystem.SaveQuast(this);
    }
}
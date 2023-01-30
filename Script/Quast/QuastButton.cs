using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuastButton : MonoBehaviour
{
    public QuastManager quastManager;

    public QuastScriptableObject quastScriptableObject;

    public TMP_Text ButtonText;
    public TMP_Text RewardText;

    public void QuastAccept()
    {
        quastManager.AcceptQuastList.Add(quastScriptableObject);

        BinarySavingSystem.SaveQuast(quastManager);

        gameObject.SetActive(false);
    }
}

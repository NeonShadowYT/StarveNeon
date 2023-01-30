using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public TMP_Text PlayerText;

    public void SetPlayer(string name)
    {
        PlayerText.text = name;
    }
}
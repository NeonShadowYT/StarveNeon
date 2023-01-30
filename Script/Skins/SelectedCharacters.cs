using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SelectedCharacters : MonoBehaviour
{
    public SelectedCharacters.Data data = new SelectedCharacters.Data();

    public MenuManagerm menuManagerm;

    public GameObject[] AllCharacters;
    public GameObject ArrowToLeft, ArrowToRight, ButtonBuyCharacter, ButtonSelectCharacter, TMP_TextSelectCharacter;

    private string statusCheck;

    private int check;
    private int i;

    public TMP_Text TextPrice, textMoney;

    [System.Serializable]
    public class Data
    {
        public string currentCharacter = "Starver";
        public List<string> haveCharacters = new List<string> { "Starver" };
        public int money;
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("SaveGame"))
        {
            data = JsonUtility.FromJson<SelectedCharacters.Data>(PlayerPrefs.GetString("SaveGame"));
        }
        else
        {
            data.money = 30;
            data.currentCharacter = "Starver";
            PlayerPrefs.SetString("SaveGame", JsonUtility.ToJson(data));
        }

        AllCharacters[i].SetActive(true);

        if (data.currentCharacter == AllCharacters[i].name)
        {
            ButtonBuyCharacter.SetActive(false);
            ButtonSelectCharacter.SetActive(false);
            TMP_TextSelectCharacter.SetActive(true);
        }
        else
        {
            if (data.currentCharacter != AllCharacters[i].name)
            {
                StartCoroutine(CheckHaveCharacter());
            }
        }

        if (i > 0)
        {
            ArrowToLeft.SetActive(true);
        }

        if (i == AllCharacters.Length)
        {
            ArrowToRight.SetActive(false);
        }

        if (data.money < 0)
        {
            data.money = 0;
        }

        textMoney.text = data.money.ToString();
    }

    public IEnumerator CheckHaveCharacter()
    {
        while(statusCheck != "Check")
        {
            if(data.haveCharacters.Count != check)
            {
                if (AllCharacters[i].name != data.haveCharacters[check])
                {
                    check++;
                }
                else if (AllCharacters[i].name == data.haveCharacters[check])
                {
                    TMP_TextSelectCharacter.SetActive(false);
                    ButtonBuyCharacter.SetActive(false);
                    ButtonSelectCharacter.SetActive(true);

                    check = 0;
                    statusCheck = "Check";
                }
            }
            else if (data.haveCharacters.Count == check)
            {
                ButtonSelectCharacter.SetActive(false);
                TMP_TextSelectCharacter.SetActive(false);

                SkinsItem skinsItem = AllCharacters[i].GetComponent<SkinsItem>();
                if (skinsItem.Exclusive == false)
                {
                    ButtonBuyCharacter.SetActive(true);
                    TextPrice.text = skinsItem.priceCharacter.ToString();
                }
                else
                {
                    ButtonBuyCharacter.SetActive(false);
                    TextPrice.text = "Ёксклюзивный скин";
                }

                check = 0;
                statusCheck = "Check";
            }
        }

        statusCheck = "";

        if (data.money < 0)
        {
            data.money = 0;
        }

        textMoney.text = data.money.ToString();

        yield return null;
    }

    public void ArrowRight()
    {
        if(i < AllCharacters.Length)
        {
            if (i == 0)
            {
                ArrowToLeft.SetActive(true);
            }

            AllCharacters[i].SetActive(false);
            i++;
            AllCharacters[i].SetActive(true);

            if (data.currentCharacter == AllCharacters[i].name)
            {
                ButtonBuyCharacter.SetActive(false);
                ButtonSelectCharacter.SetActive(false);
                TMP_TextSelectCharacter.SetActive(true);
            }
            else if (data.currentCharacter != AllCharacters[i].name)
            {
                StartCoroutine(CheckHaveCharacter());
            }

            if (i + 1 == AllCharacters.Length)
            {
                ArrowToRight.SetActive(false);
            }
        }

        if (data.money < 0)
        {
            data.money = 0;
        }

        textMoney.text = data.money.ToString();
    }

    public void ArrowLeft()
    {
        if (i < AllCharacters.Length)
        {
            AllCharacters[i].SetActive(false);
            i--;
            AllCharacters[i].SetActive(true);

            ArrowToRight.SetActive(true);

            if (data.currentCharacter == AllCharacters[i].name)
            {
                ButtonBuyCharacter.SetActive(false);
                ButtonSelectCharacter.SetActive(false);
                TMP_TextSelectCharacter.SetActive(true);
            }
            else if (data.currentCharacter != AllCharacters[i].name)
            {
                StartCoroutine(CheckHaveCharacter());
            }

            if (i == 0)
            {
                ArrowToLeft.SetActive(false);
            }
        }

        if (data.money < 0)
        {
            data.money = 0;
        }

        textMoney.text = data.money.ToString();
    }
    public void SelectCharacter()
    {
        data = JsonUtility.FromJson<SelectedCharacters.Data>(PlayerPrefs.GetString("SaveGame"));

        data.currentCharacter = AllCharacters[i].name;
        PlayerPrefs.SetString("SaveGame", JsonUtility.ToJson(data));

        ButtonSelectCharacter.SetActive(false);
        TMP_TextSelectCharacter.SetActive(true);

        if (data.money < 0)
        {
            data.money = 0;
        }

        textMoney.text = data.money.ToString();
    }
    public void BuyCharacter()
    {
        data = JsonUtility.FromJson<SelectedCharacters.Data>(PlayerPrefs.GetString("SaveGame"));

        if (data.money >= AllCharacters[i].GetComponent<SkinsItem>().priceCharacter)
        {
            data.money = data.money - AllCharacters[i].GetComponent<SkinsItem>().priceCharacter;
            data.haveCharacters.Add(AllCharacters[i].name);

            PlayerPrefs.SetString("SaveGame", JsonUtility.ToJson(data));

            ButtonBuyCharacter.SetActive(false);
            ButtonSelectCharacter.SetActive(true);
        }

        if (data.money < 0)
        {
            data.money = 0;
        }

        textMoney.text = data.money.ToString();
    }
}

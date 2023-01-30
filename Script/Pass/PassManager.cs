using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PassManager : MonoBehaviour
{
    public SelectedCharacters skindata;

    [Space]
    [Header("Текущий пасс")]
    public string passUrl = "";

    public string currentPass;
    private string latestPass;

    [Space]
    [Header("Текст")]
    public TMP_Text PassNameText;
    public TMP_Text PassMoneyText;
    public TMP_Text PassLvlText;

    public TMP_Text PassUpdatePriceText;

    [Space]
    [Header("Прогресс")]
    private int _PassMoney;
    public int PassMoney
    {
        get { return _PassMoney; }
        set
        {
            _PassMoney = value;
            PassMoneyText.text = _PassMoney.ToString();
            Save();
        }
    }
    private int _PassLvl;
    public int PassLvl
    {
        get { return _PassLvl; }
        set
        {
            _PassLvl = value;
            PassLvlText.text = _PassLvl.ToString();
            Save();
        }
    }

    [Space]
    [Header("След награда")]
    private GameObject NextRewardObj;
    private PassItemList NextPassItem;


    [Space]
    [Header("Другое")]
    public GameObject PassItemPrefab;
    public GameObject NoInternet;

    [Space]
    public Transform PassContent;

    [Space]
    [Header("Пассы")]
    public List<PassList> Pass;

    private int starverPass = 0;

    [Space]
    [Header("В игре")]
    public bool isGame;

    public void Start()
    {
        if (isGame == false)
        {
            if (PlayerPrefs.HasKey("PassStarver"))
            {
                starverPass = PlayerPrefs.GetInt("PassStarver");
                if (starverPass == 1)
                {
                    Load();
                }
                else
                {
                    Save();
                }
            }
            else
            {
                Save();
            }

            starverPass = 1;
            PlayerPrefs.SetInt("PassStarver", starverPass);

            StartCoroutine(LoadTxtData(passUrl));
        }
    }

    private IEnumerator LoadTxtData(string url)
    {
        UnityWebRequest loaded = new UnityWebRequest(url);
        loaded.downloadHandler = new DownloadHandlerBuffer();

        yield return loaded.SendWebRequest();
        latestPass = loaded.downloadHandler.text;

        Debug.Log("Текущий пасс = " + latestPass);
        CheckPass();
    }

    public void CheckPass()
    {
        if (latestPass == "")
        {
            NoInternet.SetActive(true); //Мальчик купи инет ;D
        }
        else
        {
            for (int i = 0; i < PassContent.childCount; i++) Destroy(PassContent.GetChild(i).gameObject);

            NoInternet.SetActive(false);
            if (currentPass != latestPass)
            {
                NewPassSetter();
                Load();
                CheckPass();
            }

            foreach (PassList pass in Pass)
            {
                if (pass.PassName == latestPass)
                {
                    PassNameText.text = latestPass;
  
                    foreach (PassItemList passItemList in pass.PassItemList)
                    {
                        if (passItemList.ItemLvl > PassLvl)
                        {
                            PassItemSpawner(passItemList);
                            if (passItemList.ItemLvl == 30) break;
                        }
                    }
                }
            }
        }
    }

    public void PassItemSpawner(PassItemList passItemList)
    {
        PassItem passItem = Instantiate(PassItemPrefab, PassContent).GetComponent<PassItem>();

        if (passItemList.ItemLvl == PassLvl + 1)
        {
            NextRewardChec(passItemList, passItem);
        }

        passItem.PassItemImage.sprite = passItemList.ItemImage;

        passItem.PassNameItems.text = passItemList.ItemName;
        passItem.PassItemLvl.text = passItemList.ItemLvl.ToString();
    }

    public void NextRewardChec(PassItemList passItemList, PassItem passItem)
    {
        NextPassItem = passItemList;
        NextRewardObj = passItem.gameObject;

        PassUpdatePriceText.text = NextPassItem.Price.ToString();
    }

    public void UpdatePass()
    {
        if(PassMoney >= NextPassItem.Price)
        {
            PassMoney -= NextPassItem.Price;
            PassLvl = NextPassItem.ItemLvl;

            ClaimReward();
        }
    }

    public void ClaimReward()
    {
        skindata.data = JsonUtility.FromJson<SelectedCharacters.Data>(PlayerPrefs.GetString("SaveGame"));

        if (NextPassItem.BonusTypePass == 1)
        {
            skindata.data.money += Random.Range(100, 500);
        }
        if (NextPassItem.BonusTypePass == 2)
        {
            // сделай
        }
        if (NextPassItem.BonusTypePass == 3)
        {
            skindata.data.haveCharacters.Add(NextPassItem.skinsName);
        }

        skindata.textMoney.text = skindata.data.money.ToString();
        PlayerPrefs.SetString("SaveGame", JsonUtility.ToJson(skindata.data));

        Save();
        CheckPass();
    }

    public void ClaimGameReward(QuastScriptableObject quastScriptableObject)
    {
        Load();
        PassMoney += quastScriptableObject.PassMoneyReward;
        Save();
    }

    public void NewPassSetter()
    {
        PassLvl = 0;
        currentPass = latestPass;
        Save();
    }

    public void Save()
    {
        BinarySavingSystem.SavePass(this);
    }

    public void Load()
    {
        PassData data = BinarySavingSystem.LoadPass();

        PassLvl = data.PassLvl;
        PassMoney = data.PassMoney;
        currentPass = data.PassName;
    }
}

[System.Serializable]
public class PassList
{
    [Space]
    [Header("Пасс и его штуки")]
    public string PassName;
    public List<PassItemList> PassItemList;
}

[System.Serializable]
public class PassItemList
{
    [Space]
    [Header("Пасс и его штуки")]
    public Sprite ItemImage;
    public int Price;

    [Space]
    [Header("Тип награды: 1 монеты для скинов, 2 монеты для кейсов, 3 скин игрока")]
    public int BonusTypePass;
    public string skinsName;

    [Space]
    public string ItemName;

    public int ItemLvl;
}
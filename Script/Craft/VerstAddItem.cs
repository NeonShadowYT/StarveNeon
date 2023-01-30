using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VerstAddItem : MonoBehaviour //https://www.youtube.com/channel/UC2pH2qNfh2sEAeYEGs1k_Lg
{
    [Space]
    [Header("Скрипты")]
    public VerstCraftHelper verstCraftHelper;
    public CraftManager craftManager;
    public MoneySlot texnoSlot;

    [Space]
    [Header("Информация")]
    public Image CraftIcon;
    public GameObject CraftAnLockButton;
    public TMP_Text CraftName, CraftDiscriphen, CraftScoreTreb, ScoreCraftText;

    public void UpdateCraftAnLock(ItemScriptableObject craftAnLock, int anlockScore)
    {
        CraftIcon.sprite = craftAnLock.icon;
        CraftName.text = craftAnLock.itemName;
        CraftDiscriphen.text = craftAnLock.itemDescription;
        CraftScoreTreb.text = anlockScore.ToString();

        CraftScoreTreb.gameObject.SetActive(true);
        CraftDiscriphen.gameObject.SetActive(true);
        CraftName.gameObject.SetActive(true);
        CraftIcon.gameObject.SetActive(true);
        CraftAnLockButton.SetActive(true);

        verstCraftHelper.currentVerstAnLock = 0;
    }

    public void AnLock(int anlockScore, CraftScriptableObject craftAnLock)
    {
        if (texnoSlot.amount >= anlockScore)
        {
            texnoSlot.amount -= anlockScore;
            craftManager.allCrafts.Add(craftAnLock);
            verstCraftHelper.currentVerstAnLock = 1;
        }

        CraftScoreTreb.gameObject.SetActive(false);
        CraftDiscriphen.gameObject.SetActive(false);
        CraftName.gameObject.SetActive(false);
        CraftIcon.gameObject.SetActive(false);
        CraftAnLockButton.SetActive(false);
    }

    public void StartFunc()
    {
        CraftScoreTreb.gameObject.SetActive(false);
        CraftDiscriphen.gameObject.SetActive(false);
        CraftName.gameObject.SetActive(false);
        CraftIcon.gameObject.SetActive(false);
        CraftAnLockButton.SetActive(false);
    }
}

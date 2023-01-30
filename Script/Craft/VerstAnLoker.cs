using UnityEngine;
using UnityEngine.UI;

public class VerstAnLoker : MonoBehaviour //https://www.youtube.com/channel/UC2pH2qNfh2sEAeYEGs1k_Lg
{
    [Space]
    [Header("Скрипты")]
    public VerstAddItem verstAddItem;
    public VerstCraftHelper verstCraftHelper;

    [Space]
    [Header("Крафт который откроется")]
    public CraftScriptableObject craftAnLock;
    public ItemScriptableObject itemScriptableObject;

    [Space]
    [Header("Сколько техно-монет нужно")]
    private int anlockScore;

    [Space]
    [Header("Информация")]
    public Image IconImag;
    public Button CurrentButtonAnLock;
    public GameObject NextButton1, NextButton2, NextButton3, NextButton4, NextButton5;

    public void Start()
    {
        anlockScore = itemScriptableObject.AnLockScore;
        IconImag.sprite = itemScriptableObject.icon;
    }

    public void Anlock()
    {
        verstCraftHelper.anlockScore = anlockScore;
        verstCraftHelper.craftAnLock = craftAnLock;
        verstCraftHelper.currentVerstAnLoker = this;

        verstAddItem.UpdateCraftAnLock(itemScriptableObject, anlockScore);
    }
}

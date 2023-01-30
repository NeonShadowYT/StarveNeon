using UnityEngine;
using UnityEngine.UI;

public class VerstAnLoker : MonoBehaviour //https://www.youtube.com/channel/UC2pH2qNfh2sEAeYEGs1k_Lg
{
    [Space]
    [Header("�������")]
    public VerstAddItem verstAddItem;
    public VerstCraftHelper verstCraftHelper;

    [Space]
    [Header("����� ������� ���������")]
    public CraftScriptableObject craftAnLock;
    public ItemScriptableObject itemScriptableObject;

    [Space]
    [Header("������� �����-����� �����")]
    private int anlockScore;

    [Space]
    [Header("����������")]
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

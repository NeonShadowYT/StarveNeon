using UnityEngine;
using UnityEngine.UI;

public class VerstCraftHelper : MonoBehaviour //https://www.youtube.com/channel/UC2pH2qNfh2sEAeYEGs1k_Lg
{
    [Space]
    [Header("Скрипты")]
    public VerstAddItem verstAddItem;
    public VerstAnLoker currentVerstAnLoker;

    [Space]
    [Header("Крафт который откроется")]
    public CraftScriptableObject craftAnLock;

    [Space]
    [Header("Сколько техно-монет нужно")]
    public int anlockScore;

    private int _currentVerstAnLock;
    public int currentVerstAnLock
    {
        get { return _currentVerstAnLock; }
        set
        {
            _currentVerstAnLock = value;
            if (_currentVerstAnLock == 1)
            {
                currentVerstAnLoker.CurrentButtonAnLock.interactable = false;

                if (currentVerstAnLoker.NextButton1 != null) currentVerstAnLoker.NextButton1.SetActive(true);
                if (currentVerstAnLoker.NextButton2 != null) currentVerstAnLoker.NextButton2.SetActive(true);
                if (currentVerstAnLoker.NextButton3 != null) currentVerstAnLoker.NextButton3.SetActive(true);
                if (currentVerstAnLoker.NextButton4 != null) currentVerstAnLoker.NextButton4.SetActive(true);
                if (currentVerstAnLoker.NextButton5 != null) currentVerstAnLoker.NextButton5.SetActive(true);
            }
        }
    }

    public void VerstEndFunc() => verstAddItem.AnLock(anlockScore, craftAnLock);
}

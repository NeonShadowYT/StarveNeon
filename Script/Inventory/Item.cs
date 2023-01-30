using UnityEngine;

public class Item : MonoBehaviour //https://www.youtube.com/channel/UC2pH2qNfh2sEAeYEGs1k_Lg
{
    [Space]
    [Header("Предмет")]
    public ItemScriptableObject item;

    [Space]
    [Header("Количество")]
    public int amount = 1;

    [Space]
    [Header("Оптимизированный для кустов")]
    public bool optimize;
}

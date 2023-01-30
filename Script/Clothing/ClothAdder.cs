using System.Collections.Generic;
using UnityEngine;
public class ClothAdder : MonoBehaviour //Ваш скрипт должен иметь название "ClothAdder" 
{
    [SerializeField] private List<GameObject> topPrefab, pantsPrefab, shoesPrefab, chestPlatePrefab, armorMaskPrefab, _equipedClothes;
    [SerializeField] private SkinnedMeshRenderer playerSkin;
    public Indicators indicators;
    public bool once = false;
    public GameObject OldclothObj;
    void Start() => _equipedClothes = new List<GameObject>();
    public void addClothes(GameObject clothPrefab)
    {
        GameObject clothObj = Instantiate(clothPrefab, playerSkin.transform.parent);
        if (OldclothObj != null)
        {
            Destroy(OldclothObj);
            _equipedClothes = new List<GameObject>();
        }
        OldclothObj = clothObj;
        _equipedClothes.Add(clothObj);
    }
    public void RemoveClothes(GameObject searchedClothObject)
    {
        foreach (GameObject clothObj in _equipedClothes)
        {
            if (clothObj.name.Contains(searchedClothObject.name))
            {
                indicators.Armor = 0;
                indicators.ColdArmor = 0;
                indicators.Water = 0;
                indicators.Speed = 0;
                indicators.Infected = 0;
                indicators.JumpPower = 0;
                indicators.StormArmor = 0;
                indicators.ArmorPointText.text = indicators.Armor.ToString();
                indicators.ColdArmorPointText.text = indicators.ColdArmor.ToString();
                indicators.InfectedPointText.text = indicators.Infected.ToString();
                indicators.SpeedPointText.text = indicators.Speed.ToString();
                OldclothObj = clothObj;
                _equipedClothes.Remove(clothObj);
                Destroy(clothObj);
                if (OldclothObj != null)
                {
                    Destroy(OldclothObj);
                    _equipedClothes = new List<GameObject>();
                }
                once = true;
                return;
            }
        }
        
    }
}

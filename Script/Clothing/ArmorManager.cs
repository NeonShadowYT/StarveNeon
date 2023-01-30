using UnityEngine;
public class ArmorManager : MonoBehaviour
{
    public Animator anim;
    public Indicators indicators;
    public int ArmorPoints, ColdArmorPoints, WaterPoints, SpeedPoints, InfectedPoints, JumpPowerPoints, StormArmor;
    void Start() => indicators = GameObject.Find("Indicators").GetComponent<Indicators>();
    public void EquipArmor()
    {
        indicators.Armor = ArmorPoints;
        indicators.ColdArmor = ColdArmorPoints;
        indicators.Water = WaterPoints;
        indicators.Speed = SpeedPoints;
        indicators.StormArmor = StormArmor;
        indicators.Infected = InfectedPoints;
        indicators.JumpPower = JumpPowerPoints;
        indicators.ArmorPointText.text = indicators.Armor.ToString();
        indicators.ColdArmorPointText.text = indicators.ColdArmor.ToString();
        indicators.InfectedPointText.text = indicators.Infected.ToString();
        indicators.SpeedPointText.text = indicators.Speed.ToString();
    }
}

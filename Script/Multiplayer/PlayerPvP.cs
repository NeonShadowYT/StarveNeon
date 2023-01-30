using UnityEngine;
public class PlayerPvP : MonoBehaviour
{
    public PlayerPvP playerPvP;
    public Indicators indicators;
    public GameObject HitFX;
    public void Start()
    {
        //if (_photonView.IsMine) Destroy(playerPvP);
    }
    public void HitForMe(float damage)
    {
        if (indicators.Armor > 0)
        {
            if (indicators.Armor == 1) indicators.healthAmount -= (damage - 1);
            if (indicators.Armor == 2) indicators.healthAmount -= (damage - 2);
            if (indicators.Armor == 3) indicators.healthAmount -= (damage - 3);
            if (indicators.Armor == 4) indicators.healthAmount -= (damage - 4);
            if (indicators.Armor == 5) indicators.healthAmount -= (damage - 5);
            if (indicators.Armor == 6) indicators.healthAmount -= (damage - 6);
        }
        else indicators.healthAmount -= damage;
        indicators.damageEffect.SetActive(true);
    }
}

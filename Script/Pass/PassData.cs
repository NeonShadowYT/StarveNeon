using UnityEngine;
[System.Serializable] // дает возможность сохранить этот класс в файл
public class PassData //https://www.youtube.com/channel/UC2pH2qNfh2sEAeYEGs1k_Lg
{
    public int PassLvl;
    public int PassMoney;
    public string PassName;

    public PassData(PassManager passManager)
    {
        PassLvl = passManager.PassLvl;
        PassMoney = passManager.PassMoney;
        PassName = passManager.currentPass;
    }
}
using UnityEngine; //using EnvSpawn;
public class ResourceHealth : MonoBehaviour //https://www.youtube.com/channel/UC2pH2qNfh2sEAeYEGs1k_Lg
{
    public ItemScriptableObject resourceType;
    public GameObject hitFix;
    public int Lvl;
    public bool rock = true, tree = false, cust = false;
    public void AudioDamage() //resourceSpawer.GetComponent<EnviroSpawn_CS>().Generate();
    {
        if (tree == true) SoundManager.snd.PlaySounds3();
        if (rock == true) SoundManager.snd.PlaySounds4();
        if (cust == true) SoundManager.snd.PlaySounds6();
    }
}

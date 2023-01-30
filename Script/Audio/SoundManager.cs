using UnityEngine;
public class SoundManager : MonoBehaviour //https://www.youtube.com/channel/UC2pH2qNfh2sEAeYEGs1k_Lg
{
    public static SoundManager snd;
    private AudioSource audioSrc;
    private AudioClip[] Sounds;
    private int randomSounds;
    void Start()
    {
        snd = this;
        audioSrc = GetComponent<AudioSource>();
        Sounds = Resources.LoadAll<AudioClip>("Sounds");
    }
    public void PlaySounds() //ходьба
    {
        randomSounds = Random.Range(0, 0);
        audioSrc.pitch = Random.Range(0.9f, 1.1f);
        audioSrc.PlayOneShot(Sounds[randomSounds]);
    }
    public void PlaySounds2() //удар воздух
    {
        randomSounds = Random.Range(1, 1);
        audioSrc.pitch = Random.Range(0.8f, 1.2f);
        audioSrc.PlayOneShot(Sounds[randomSounds]);
    }
    public void PlaySounds3() //дерево
    {
        randomSounds = Random.Range(2, 4);
        audioSrc.pitch = Random.Range(0.9f, 1.1f);
        audioSrc.PlayOneShot(Sounds[randomSounds]);
    }
    public void PlaySounds4() //камень
    {
        randomSounds = Random.Range(5, 6);
        audioSrc.pitch = Random.Range(0.9f, 1.1f);
        audioSrc.PlayOneShot(Sounds[randomSounds]);
    }
    public void PlaySounds5() //хаванье
    {
        randomSounds = Random.Range(11, 11);
        audioSrc.pitch = Random.Range(0.9f, 1.1f);
        audioSrc.PlayOneShot(Sounds[randomSounds]);
    }
    public void PlaySounds6() //куст
    {
        randomSounds = Random.Range(14, 14);
        audioSrc.pitch = Random.Range(0.9f, 1.1f);
        audioSrc.PlayOneShot(Sounds[randomSounds]);
    }
    public void PlaySounds7()
    {
        randomSounds = Random.Range(15, 15);
        audioSrc.pitch = Random.Range(0.9f, 1.1f);
        audioSrc.PlayOneShot(Sounds[randomSounds]);
    }
}

using UnityEngine;

public class WeaponManager : MonoBehaviour //https://www.youtube.com/channel/UC2pH2qNfh2sEAeYEGs1k_Lg
{
    [Space]
    [Header("Звук перезарядки")]
    public AudioSource audioSource;

    [Space]
    [Header("Аниматор для перезарядки")]
    public Animator anim;

    [Space]
    [Header("Текущее оружие")]
    public Gun currentGun;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && currentGun != null) currentGun.Shoot();
    }

    public void OnReload()
    {
        if (currentGun != null) currentGun.Reload();
    }

    public void AudioReload()
    {
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(currentGun.reloadAudio);
    }

    public void EndReload()
    {
        currentGun.once = false;
        anim.SetBool("Reload", false);
        currentGun.Amount = currentGun.realBilletAmmount;
    }
}

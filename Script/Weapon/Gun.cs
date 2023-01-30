using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour //https://www.youtube.com/channel/UC2pH2qNfh2sEAeYEGs1k_Lg
{
    [Space]
    [Header("Оружие")]
    public GunType gunType; // тип оружия , влияет на перезарядку

    [Space]
    [Header("Оптимизация")]
    public LayerMask layerMask; // оптимизация
    public float distanceFire = 50f; // дистанция

    [Space]
    [Header("Разное")]
    public InventoryManager inventoryManager; // инвентарь для перезарядки
    public Camera mainCamera;
    public Animator anim; // аниматор

    public float damage = 20f; // урон

    public float fireRate = 1f; // скорострельность
    private float nextFire = 0f; // время до следующего выстрела

    public int BulletRazbros = 2; // сила разброса

    private float currentDamage; // текущий урон
    private float currentDistance; // дистанция которую пролетела пуля до цели

    private PlayerPvP hitPlayer; // игрок в которого попали
    private MobStats hitMob; // моб в которого попали

    [Space]
    [Header("Партиклы")]
    public ParticleSystem muzzleFlash; // эфект выстрела
    public GameObject muzzleFlashObj; // обьект выстрела

    [Space]
    [Header("Патроны")]
    public int billetAmmount; // количество патрон в обойме
    public int realBilletAmmount; // реальная обойма
    private int CurrentAmount; // текущее кол-во патронов
    public int Amount // патроны
    {
        get { return CurrentAmount; }
        set
        {
            CurrentAmount = value; // текущее кол-во патронов равно количество патронов
            Reload(); // если патронов 0 перезаряжаемся
        }
    }

    public ItemScriptableObject bulletItem; // предмет пули которая нужна оружию
    public bool once;

    [Space]
    [Header("Звуки")]
    public AudioClip shotAudio; // звук выстрела
    public AudioClip reloadAudio; // звук перезарядки

    public AudioSource audioSource; // источник выстрела

    public void Shoot() // стрельба
    {
        if (Time.time >= nextFire && Amount >= 1)
        {
            nextFire = Time.time + 1f / fireRate;

            Amount -= 1; // вычитаем патрон
            currentDamage = damage; // текущий урон равен урону оружия

            audioSource.pitch = Random.Range(0.9f, 1.1f); // рандомизируем звук
            audioSource.PlayOneShot(shotAudio); // производим звук

            muzzleFlashObj.SetActive(true); // включаем эфект
            muzzleFlash.Play(); // проигрываем эфект выстрела

            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(new Vector2((Screen.width / 2) + Random.Range(BulletRazbros, -BulletRazbros), (Screen.height / 2) + Random.Range(BulletRazbros, -BulletRazbros)));

            if (Physics.Raycast(ray, out hit, distanceFire, layerMask))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    if (hit.collider.GetComponent<PlayerPvP>() != null) // если на том во что попал игрок есть скрипт игрока
                    {
                        hitPlayer = hit.collider.GetComponent<PlayerPvP>(); // получаем скрипт игрока
                        hitPlayer.HitForMe(currentDamage); // если игрок в которого попали не мы то наносим урон
                        Instantiate(hitPlayer.HitFX, hit.point, Quaternion.LookRotation(hit.normal)); // спавним эфект крови
                    }
                }
                else if (hit.collider.CompareTag("Mob")) // если нет то
                {
                    if (hit.collider.GetComponent<MobStats>() != null) // если мы попали в моба
                    {
                        hitMob = hit.collider.GetComponent<MobStats>(); // получаем скрипт моба
                        hitMob.HitForMe(currentDamage); // наносим урон
                        Instantiate(hitMob.mobScriptableObject.hitFIX, hit.point, Quaternion.LookRotation(hit.normal)); // спавним эфект крови
                    }
                }
            }
        }
    }

    public void Reload()
    {
        if (CurrentAmount <= 0 && once == false) // если патронов 0 перезаряжаемся
        {
            once = true;
            int amountBulletInventory = 0;

            foreach (InventorySlot slot in inventoryManager.slots)
            {
                if (amountBulletInventory >= 1)
                {
                    realBilletAmmount = amountBulletInventory;
                    anim.SetBool("Reload", true);
                }
                else
                {
                    if (slot.item == bulletItem)
                    {
                        if (billetAmmount > slot.amount)
                        {
                            amountBulletInventory += slot.amount;
                            slot.GetComponentInChildren<DragAndDropItem>().NullifySlotData();
                        }
                        else
                        {
                            slot.amount -= billetAmmount;
                            amountBulletInventory = billetAmmount;
                            if (slot.amount <= 0) slot.GetComponentInChildren<DragAndDropItem>().NullifySlotData(); else slot.itemAmountText.text = slot.amount.ToString();
                        }
                    }
                }
            }
        }
    }
}

public enum GunType // тип оружия
{
    Gun, // перезарядка обоймы
    Rifle // перезарядка обоймы и анимация перед след выстрелом
}
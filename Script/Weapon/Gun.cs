using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour //https://www.youtube.com/channel/UC2pH2qNfh2sEAeYEGs1k_Lg
{
    [Space]
    [Header("������")]
    public GunType gunType; // ��� ������ , ������ �� �����������

    [Space]
    [Header("�����������")]
    public LayerMask layerMask; // �����������
    public float distanceFire = 50f; // ���������

    [Space]
    [Header("������")]
    public InventoryManager inventoryManager; // ��������� ��� �����������
    public Camera mainCamera;
    public Animator anim; // ��������

    public float damage = 20f; // ����

    public float fireRate = 1f; // ����������������
    private float nextFire = 0f; // ����� �� ���������� ��������

    public int BulletRazbros = 2; // ���� ��������

    private float currentDamage; // ������� ����
    private float currentDistance; // ��������� ������� ��������� ���� �� ����

    private PlayerPvP hitPlayer; // ����� � �������� ������
    private MobStats hitMob; // ��� � �������� ������

    [Space]
    [Header("��������")]
    public ParticleSystem muzzleFlash; // ����� ��������
    public GameObject muzzleFlashObj; // ������ ��������

    [Space]
    [Header("�������")]
    public int billetAmmount; // ���������� ������ � ������
    public int realBilletAmmount; // �������� ������
    private int CurrentAmount; // ������� ���-�� ��������
    public int Amount // �������
    {
        get { return CurrentAmount; }
        set
        {
            CurrentAmount = value; // ������� ���-�� �������� ����� ���������� ��������
            Reload(); // ���� �������� 0 ��������������
        }
    }

    public ItemScriptableObject bulletItem; // ������� ���� ������� ����� ������
    public bool once;

    [Space]
    [Header("�����")]
    public AudioClip shotAudio; // ���� ��������
    public AudioClip reloadAudio; // ���� �����������

    public AudioSource audioSource; // �������� ��������

    public void Shoot() // ��������
    {
        if (Time.time >= nextFire && Amount >= 1)
        {
            nextFire = Time.time + 1f / fireRate;

            Amount -= 1; // �������� ������
            currentDamage = damage; // ������� ���� ����� ����� ������

            audioSource.pitch = Random.Range(0.9f, 1.1f); // ������������� ����
            audioSource.PlayOneShot(shotAudio); // ���������� ����

            muzzleFlashObj.SetActive(true); // �������� �����
            muzzleFlash.Play(); // ����������� ����� ��������

            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(new Vector2((Screen.width / 2) + Random.Range(BulletRazbros, -BulletRazbros), (Screen.height / 2) + Random.Range(BulletRazbros, -BulletRazbros)));

            if (Physics.Raycast(ray, out hit, distanceFire, layerMask))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    if (hit.collider.GetComponent<PlayerPvP>() != null) // ���� �� ��� �� ��� ����� ����� ���� ������ ������
                    {
                        hitPlayer = hit.collider.GetComponent<PlayerPvP>(); // �������� ������ ������
                        hitPlayer.HitForMe(currentDamage); // ���� ����� � �������� ������ �� �� �� ������� ����
                        Instantiate(hitPlayer.HitFX, hit.point, Quaternion.LookRotation(hit.normal)); // ������� ����� �����
                    }
                }
                else if (hit.collider.CompareTag("Mob")) // ���� ��� ��
                {
                    if (hit.collider.GetComponent<MobStats>() != null) // ���� �� ������ � ����
                    {
                        hitMob = hit.collider.GetComponent<MobStats>(); // �������� ������ ����
                        hitMob.HitForMe(currentDamage); // ������� ����
                        Instantiate(hitMob.mobScriptableObject.hitFIX, hit.point, Quaternion.LookRotation(hit.normal)); // ������� ����� �����
                    }
                }
            }
        }
    }

    public void Reload()
    {
        if (CurrentAmount <= 0 && once == false) // ���� �������� 0 ��������������
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

public enum GunType // ��� ������
{
    Gun, // ����������� ������
    Rifle // ����������� ������ � �������� ����� ���� ���������
}
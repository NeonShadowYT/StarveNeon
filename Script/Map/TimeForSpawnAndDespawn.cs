using System.Collections;
using UnityEngine;

public class TimeForSpawnAndDespawn : MonoBehaviour //https://www.youtube.com/channel/UC2pH2qNfh2sEAeYEGs1k_Lg
{
    [Space]
    [Header("�����")]
    public bool spawn = false, despawn = false, Optimize; 

    [Space]
    [Header("�����")]
    public GameObject[] enemy;
    public Transform[] spawnPoint;
    private int rand, randPosition;

    [Space]
    public GameObject[] OptimizeItemObj;

    [Space]
    [Header("��������")]
    public GameObject forDestroy; // �����
    public Animator anim; // �������� ��� ������ � ����������
    public bool noAnim = false;

    [Space]
    [Header("�����")]
    private float _timeBtwSpawns;
    public int dynamicProric;
    public float startTimeBtwSpawns;
    public float timeBtwSpawns
    {
        get { return _timeBtwSpawns; }
        set
        {
            _timeBtwSpawns = value;
            if (_timeBtwSpawns <= 0)
            {
                if(spawn || despawn)
                {
                    if (spawn == true)
                    {
                        rand = Random.Range(0, enemy.Length);
                        randPosition = Random.Range(0, spawnPoint.Length);
                        Instantiate(enemy[rand], spawnPoint[randPosition].transform.position, Quaternion.identity);
                        timeBtwSpawns = startTimeBtwSpawns;
                    }
                    if (despawn == true) if (noAnim == false) anim.SetBool("Death", true); else Destroy(forDestroy); // ���������� ������ // ��������� �������� �������� ������ ( � ����� �������� ������ �� ����������� ������ )
                }
                else if (Optimize) //��� ������
                {
                    rand = Random.Range(0, OptimizeItemObj.Length);
                    OptimizeItemObj[rand].SetActive(true);
                    timeBtwSpawns = startTimeBtwSpawns;
                }
            }
        }
    }

    void Start()
    {
        timeBtwSpawns = startTimeBtwSpawns;
        if (noAnim == false) anim = GetComponent<Animator>(); // �������� �������� ��� ������ � ����������
    }

    void OnTriggerStay(Collider col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            timeBtwSpawns -= Time.deltaTime;
        }
    }
}
using System.Collections;
using UnityEngine;

public class TimeForSpawnAndDespawn : MonoBehaviour //https://www.youtube.com/channel/UC2pH2qNfh2sEAeYEGs1k_Lg
{
    [Space]
    [Header("Режим")]
    public bool spawn = false, despawn = false, Optimize; 

    [Space]
    [Header("Спавн")]
    public GameObject[] enemy;
    public Transform[] spawnPoint;
    private int rand, randPosition;

    [Space]
    public GameObject[] OptimizeItemObj;

    [Space]
    [Header("Удаление")]
    public GameObject forDestroy; // костёр
    public Animator anim; // аниматор для работы с анимациями
    public bool noAnim = false;

    [Space]
    [Header("Время")]
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
                    if (despawn == true) if (noAnim == false) anim.SetBool("Death", true); else Destroy(forDestroy); // уничтажаем обьект // запускаем анимацию угасания костра ( в конце анимации сигнал на уничтожение костра )
                }
                else if (Optimize) //для кустов
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
        if (noAnim == false) anim = GetComponent<Animator>(); // получаем аниматор для работы с анимациями
    }

    void OnTriggerStay(Collider col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            timeBtwSpawns -= Time.deltaTime;
        }
    }
}
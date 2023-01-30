using System.Collections;
using UnityEngine;
public class Spawner : MonoBehaviour //https://www.youtube.com/channel/UC2pH2qNfh2sEAeYEGs1k_Lg
{
    public GameObject[] enemy;
    public Transform[] spawnPoint;
    private int rand, randPosition;
    public float startTimeBtwSpawns;
    private float _timeBtwSpawns;
    private float timeBtwSpawns
    {
        get { return _timeBtwSpawns; }
        set
        {
            _timeBtwSpawns = value;
            if (_timeBtwSpawns <= 0)
            {
                rand = Random.Range(0, enemy.Length);
                randPosition = Random.Range(0, spawnPoint.Length);
                Instantiate(enemy[rand], spawnPoint[randPosition].transform.position, Quaternion.identity);
                timeBtwSpawns = startTimeBtwSpawns;
            }
        }
    }
    void Start() => timeBtwSpawns = startTimeBtwSpawns;
    void LateUpdate() => timeBtwSpawns -= Time.deltaTime;
}

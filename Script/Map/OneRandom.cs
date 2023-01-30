using System.Collections;
using UnityEngine;
public class OneRandom : MonoBehaviour
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
                Destroy(forDestroy);
            }
        }
    }
    public GameObject forDestroy;
    void Start() => timeBtwSpawns = startTimeBtwSpawns;
    void LateUpdate() => timeBtwSpawns -= Time.deltaTime;
}

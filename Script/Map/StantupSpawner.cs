using UnityEngine;

public class StantupSpawner : MonoBehaviour
{
    [Space]
    [Header("�����")]
    public GameObject PlayerPrefab;
    private int Multiplayer;

    [Space]
    [Header("������������ ���������")]
    public Transform[] Spawn;
    private int randPosition;

    void Awake()
    {
        if (PlayerPrefs.HasKey("Multiplayer"))
        {
            Multiplayer = PlayerPrefs.GetInt("Multiplayer");
        }
        else
        {
            Multiplayer = 0;
        }

        if(Multiplayer != 1)
        {
            randPosition = Random.Range(0, Spawn.Length);
            Instantiate(PlayerPrefab, Spawn[randPosition].transform.position, Quaternion.identity);
        }
    }
}
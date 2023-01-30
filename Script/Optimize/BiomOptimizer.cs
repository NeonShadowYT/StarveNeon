using UnityEngine;

public class BiomOptimizer : MonoBehaviour
{
    [Space]
    [Header("Обьект внутри биома")]
    public GameObject BiomView;

    public bool Forest;

    void Start()
    {
        if (Forest == false) BiomView.SetActive(false);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player")) BiomView.SetActive(true);
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Player")) BiomView.SetActive(false);
    }
}

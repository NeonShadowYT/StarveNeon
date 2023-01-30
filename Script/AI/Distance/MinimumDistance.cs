using UnityEngine;

public class MinimumDistance : MonoBehaviour
{
    [Space]
    [Header("Инфа о АИ")]
    public AIManager aIManager; // аи менеджер

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (aIManager.AgreAI == true)
            {
                aIManager._animator.SetBool("Damage", true); // если мы достаём до игрока и ии агресивный то атакуем
                aIManager.AIStop();
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            aIManager._animator.SetBool("Damage", false); // если мы не достаём до игрока то не атакуем
            aIManager.AIWalk();
        }
    }
}
using UnityEngine;

public class AnimUpdater : MonoBehaviour
{
    [Space]
    [Header("Инфа о АИ")]
    public AIManager AIManager; // аи менеджер

    public void OnTriggerStay(Collider col)
    {
        if (col.gameObject.CompareTag("Player")) AIManager._animator.SetFloat("Speed", AIManager._navMeshAgent.velocity.magnitude / AIManager.mobScriptableObject.movementSpeed);  // если игрок в дистанции видимости анимации то делаем анимацию
    }
}
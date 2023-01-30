using UnityEngine;
using UnityEngine.AI;

public class Optimizer : MonoBehaviour
{
    [Space]
    [Header("Инфа о АИ")]
    public AIManager aIManager;

    private NavMeshAgent _navMeshAgent;
    private AudioSource audios;
    private Animator _animator;

    [Space]
    [Header("Моделька моба")]
    public GameObject MobView;

    void Start()
    {
        _navMeshAgent = aIManager._navMeshAgent;
        audios = aIManager.audios;
        _animator = aIManager._animator;

        aIManager.AImover = false;

        _animator.enabled = false;
        audios.enabled = false;
        _navMeshAgent.enabled = false;

        MobView.SetActive(false);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            aIManager.AImover = true;

            _animator.enabled = true;
            audios.enabled = true;
            _navMeshAgent.enabled = true;

            MobView.SetActive(true);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            aIManager.AImover = false;

            _animator.enabled = false;
            audios.enabled = false;
            _navMeshAgent.enabled = false;

            MobView.SetActive(false);
        }
    }
}

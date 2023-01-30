using UnityEngine;
using UnityEngine.AI;

public class AIManager : MonoBehaviour // https://www.youtube.com/c/maximple, https://www.youtube.com/channel/UC2pH2qNfh2sEAeYEGs1k_Lg
{
    public MobScriptableObject mobScriptableObject;

    [Space]
    [Header("Тип моба")]
    public bool AgreAI;
    public bool DamageForAgreAI;
    public bool HealthForPanicAI;
    public bool RegenerationAI;
    public bool PanicAI;
    public bool ShotAI;
    public bool CompanionAI;
    public bool Sandworm;
    public bool FractionAI;
    public bool PatrolAI;

    [Space]
    [Header("Скрипты и другое")]
    public MobStats mobStats;
    public FractionNPS FractionNPS;
    public Daytime daytime;
    public NavMeshAgent _navMeshAgent;
    public Animator _animator; // аниматор для анимаций

    public float timeBetweenShots, waitTime;
    public float nextShotTime; //время между выстрелами

    [Space]
    [Header("Позиция игрока и моба")]
    public Transform player;
    public Transform mTransform;

    [Space]
    public Transform[] patrolPoints;
    public int currentPointIndex;

    [Space]
    [Header("Звук")]
    public AudioSource audios; // где будет проигрываться звук

    [Space]
    [Header("Объекты")]
    public GameObject BossEffect, BossObject;

    [Space]
    [Header("Булы")]
    public bool AImover;
    public bool once, onceDamage = true, onceAttacke = true, oncePanic = true;

    public void Awake()
    {
        if (mobStats == null) mobStats = GetComponent<MobStats>();
        if (FractionNPS == null) FractionNPS = GetComponent<FractionNPS>();
        if (_navMeshAgent == null) _navMeshAgent = GetComponent<NavMeshAgent>();
        if (_animator == null) _animator = GetComponent<Animator>();
        if (mTransform == null) mTransform = GetComponent<Transform>();
        if (audios == null) audios = GetComponent<AudioSource>();
        if (mobScriptableObject == null) mobScriptableObject = mobStats.mobScriptableObject;

        _navMeshAgent.speed = mobScriptableObject.movementSpeed;

        InvokeRepeating(nameof(MoveAnimal), mobScriptableObject.changePositionTime, mobScriptableObject.changePositionTime);
    }

    private void MoveAnimal()
    {
        if (AImover == true)
        {
            _navMeshAgent.destination = RandomNavSphere(mobScriptableObject.moveDistance);

            if(player != null)
            {
                if (PatrolAI == true)
                {
                    if (currentPointIndex + 1 < patrolPoints.Length)
                    {
                        currentPointIndex++;
                    }
                    else
                    {
                        currentPointIndex = 0;
                    }
                }
                else
                {
                    if (CompanionAI == true)
                    {
                        if ((mTransform.position - player.position).sqrMagnitude > 20f * 20f)
                        {
                            _navMeshAgent.destination = player.position;
                        }
                    }
                }
            }
        }
    }

    public void AIWalk()
    {
        _navMeshAgent.speed = mobScriptableObject.movementSpeed;
    }

    public void AIStop()
    {
        _navMeshAgent.speed = 0;
    }

    public void AnimalAudio()
    {
        audios.pitch = Random.Range(0.8f, 1.2f);
        audios.PlayOneShot(mobScriptableObject.din);
    }

    public void Destroys() => Destroy(gameObject);

    Vector3 RandomNavSphere(float distance)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;
        randomDirection += mTransform.position;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, distance, -1);
        return navHit.position;
    }

    public void MobRewEnd()
    {
        AIWalk();
        _animator.SetBool("ViewPlayer", false);
    }
}
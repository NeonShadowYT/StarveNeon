using UnityEngine;
using UnityEngine.AI;

//https://www.youtube.com/channel/UC2pH2qNfh2sEAeYEGs1k_Lg
public class AgreAI : MonoBehaviour
{
    public AudioClip din;
    public AudioSource audios;

    [Header("Target")]
    //public GameObject Player;
    public Transform myPlayer;

    [Header("Distance")]
    public float radiusOfAi = 80;
    public float radiusOfView = 20;
    public float attackDistance = 1.5f;

    [Header("Animations")]
    public string nameAnimIdle;
    public string nameAnimWalk;
    public string nameAnimAttack;

    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    public Rigidbody rig;

    public float distances;

    public bool noRigidbody = true;
    public bool AI = false;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float changePositionTime = 5f;
    [SerializeField] private float moveDistance = 10f;

    private void Start()
    {
        audios = GetComponent<AudioSource>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = movementSpeed;
        _animator = GetComponent<Animator>();
        InvokeRepeating(nameof(MoveAnimal), changePositionTime, changePositionTime);
        myPlayer = GameObject.Find("Player").transform;
    }
    private void Run()
    {
        if (Vector3.Distance(transform.position, myPlayer.position) < radiusOfAi)
        {
            AI = true;
            distances = Vector3.Distance(myPlayer.position, transform.position);
            _animator.SetFloat("Speed", _navMeshAgent.velocity.magnitude / movementSpeed);

            if (noRigidbody == false)
            {
                rig.isKinematic = false;
            }
            if (distances < radiusOfView)
            {
                _navMeshAgent.destination = myPlayer.position;
            }
        }
        else
        {
            AI = false;
            if (noRigidbody == false)
            {
                rig.isKinematic = true;
            }
        }
    }

   Vector3 RandomNavSphere(float distance)
   {

        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;

        randomDirection += transform.position;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, -1);

        return navHit.position;

   }


    private void MoveAnimal()
    {
        if (AI == true)
        {
            _navMeshAgent.SetDestination(RandomNavSphere(moveDistance));
        }
    }

    public void AnimalAudio()
    {
        audios.PlayOneShot(din);
    }
}

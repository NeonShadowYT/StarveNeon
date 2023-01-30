using UnityEngine;
using UnityEngine.AI;

//https://www.youtube.com/channel/UC2pH2qNfh2sEAeYEGs1k_Lg
public class AnimalAI : MonoBehaviour
{
   public AudioClip din;
   public AudioSource audios;
   private NavMeshAgent _navMeshAgent;
   private Animator _animator;
   [SerializeField] private float movementSpeed;
   [SerializeField] private float changePositionTime = 5f;
   [SerializeField] private float moveDistance = 10f;

    private void Start()
    {
        audios = GetComponent<AudioSource>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = movementSpeed;
        _animator = GetComponent<Animator>();
        InvokeRepeating(nameof(MoveAnimal),changePositionTime,changePositionTime);
    }

    private void Update()
    {

        _animator.SetFloat("Speed", _navMeshAgent.velocity.magnitude / movementSpeed);
    }

   Vector3 RandomNavSphere (float distance) 
   {
      Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;
           
      randomDirection += transform.position;
           
      NavMeshHit navHit;
           
      NavMesh.SamplePosition (randomDirection, out navHit, distance, -1);
           
      return navHit.position;
   }

   private void MoveAnimal()
   {
        _navMeshAgent.SetDestination(RandomNavSphere(moveDistance));
    }

   public void AnimalAudio()
   {
      audios.PlayOneShot(din);
   }
}

using UnityEngine;
using UnityEngine.AI;

public class ViewDistance : MonoBehaviour
{
    [Space]
    [Header("Инфа о АИ")]
    public AIManager aIManager;  // аи менеджер

    private float minimunDistance = 4f;
    private bool once;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            aIManager.player = col.transform;
            aIManager.AIStop();
            aIManager._animator.SetBool("ViewPlayer", true);

            if (aIManager.Sandworm == true) // если дистанция к игроку меньше минимальной дистанции на которую можно подойти
            {
                aIManager.BossEffect.SetActive(false);
                aIManager.BossObject.SetActive(true);
            }
        }
    }

    void OnTriggerStay(Collider col) // каждый кадр когда игрок зайдёт в зону видимости
    {
        if (col.gameObject.CompareTag("Player")) // если игрок зашол в зону видимости
        {
            if (aIManager.AgreAI == true)
            {
                aIManager._navMeshAgent.destination = aIManager.player.position; // и мы агресивный ии то идём к нему
            }
            else // если мы не агресивный ии
            {
                if (aIManager.PanicAI == true) // если мы пугливый ии
                {
                    PanicLogic();

                    aIManager.oncePanic = true; // включаем панику
                }
            }

            if(aIManager.PatrolAI == true)
            {
                if (aIManager.mTransform.position != aIManager.patrolPoints[aIManager.currentPointIndex].position)
                {
                    aIManager._navMeshAgent.destination = aIManager.patrolPoints[aIManager.currentPointIndex].position; // и мы агресивный ии то идём к нему
                }
            }

            if (aIManager.ShotAI == true) // если мы стреляющий ии
            {
                if (aIManager.FractionAI == true)
                {
                    if(aIManager.AgreAI == true)
                    {
                        Shoots();
                        if ((aIManager.mTransform.position - aIManager.player.position).sqrMagnitude < 20f * 20f) PanicLogic();
                    }
                }
                else
                {
                    Shoots();
                }
            }
        }
    }

    public void PanicLogic()
    {
        aIManager._navMeshAgent.destination = Vector3.MoveTowards(aIManager.mTransform.position, aIManager.player.position, -aIManager.mobScriptableObject.movementSpeed * Time.deltaTime * 2f); //_navMeshAgent.destination -= player.position; //убегаем от игрока
    }

    public void Shoots()
    {
        if (Time.time > aIManager.nextShotTime)
        {
            Cartridge сartridge = Instantiate(aIManager.mobScriptableObject.сartridge, aIManager.mTransform.position, Quaternion.identity).GetComponent<Cartridge>(); //спавним пулю
            сartridge.targetPosition = aIManager.player.position;
            aIManager.nextShotTime = Time.time + aIManager.timeBetweenShots; // задаём время
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            aIManager.player = null;
            aIManager.AIWalk();
            aIManager._animator.SetBool("ViewPlayer", false);

            if (aIManager.PanicAI == true) // если дистанция к игроку меньше минимальной дистанции на которую можно подойти
            {
                aIManager.oncePanic = false;
            }
            else
            {
                if (aIManager.Sandworm == true) // если дистанция к игроку меньше минимальной дистанции на которую можно подойти
                {
                    aIManager.BossEffect.SetActive(true);
                    aIManager.BossObject.SetActive(false);
                }
            }
        }
    }
}

using UnityEngine;
using UnityEngine.AI;

public class ViewDistance : MonoBehaviour
{
    [Space]
    [Header("���� � ��")]
    public AIManager aIManager;  // �� ��������

    private float minimunDistance = 4f;
    private bool once;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            aIManager.player = col.transform;
            aIManager.AIStop();
            aIManager._animator.SetBool("ViewPlayer", true);

            if (aIManager.Sandworm == true) // ���� ��������� � ������ ������ ����������� ��������� �� ������� ����� �������
            {
                aIManager.BossEffect.SetActive(false);
                aIManager.BossObject.SetActive(true);
            }
        }
    }

    void OnTriggerStay(Collider col) // ������ ���� ����� ����� ����� � ���� ���������
    {
        if (col.gameObject.CompareTag("Player")) // ���� ����� ����� � ���� ���������
        {
            if (aIManager.AgreAI == true)
            {
                aIManager._navMeshAgent.destination = aIManager.player.position; // � �� ���������� �� �� ��� � ����
            }
            else // ���� �� �� ���������� ��
            {
                if (aIManager.PanicAI == true) // ���� �� �������� ��
                {
                    PanicLogic();

                    aIManager.oncePanic = true; // �������� ������
                }
            }

            if(aIManager.PatrolAI == true)
            {
                if (aIManager.mTransform.position != aIManager.patrolPoints[aIManager.currentPointIndex].position)
                {
                    aIManager._navMeshAgent.destination = aIManager.patrolPoints[aIManager.currentPointIndex].position; // � �� ���������� �� �� ��� � ����
                }
            }

            if (aIManager.ShotAI == true) // ���� �� ���������� ��
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
        aIManager._navMeshAgent.destination = Vector3.MoveTowards(aIManager.mTransform.position, aIManager.player.position, -aIManager.mobScriptableObject.movementSpeed * Time.deltaTime * 2f); //_navMeshAgent.destination -= player.position; //������� �� ������
    }

    public void Shoots()
    {
        if (Time.time > aIManager.nextShotTime)
        {
            Cartridge �artridge = Instantiate(aIManager.mobScriptableObject.�artridge, aIManager.mTransform.position, Quaternion.identity).GetComponent<Cartridge>(); //������� ����
            �artridge.targetPosition = aIManager.player.position;
            aIManager.nextShotTime = Time.time + aIManager.timeBetweenShots; // ����� �����
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            aIManager.player = null;
            aIManager.AIWalk();
            aIManager._animator.SetBool("ViewPlayer", false);

            if (aIManager.PanicAI == true) // ���� ��������� � ������ ������ ����������� ��������� �� ������� ����� �������
            {
                aIManager.oncePanic = false;
            }
            else
            {
                if (aIManager.Sandworm == true) // ���� ��������� � ������ ������ ����������� ��������� �� ������� ����� �������
                {
                    aIManager.BossEffect.SetActive(true);
                    aIManager.BossObject.SetActive(false);
                }
            }
        }
    }
}

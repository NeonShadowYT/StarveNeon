using UnityEngine;

public class AnimUpdater : MonoBehaviour
{
    [Space]
    [Header("���� � ��")]
    public AIManager AIManager; // �� ��������

    public void OnTriggerStay(Collider col)
    {
        if (col.gameObject.CompareTag("Player")) AIManager._animator.SetFloat("Speed", AIManager._navMeshAgent.velocity.magnitude / AIManager.mobScriptableObject.movementSpeed);  // ���� ����� � ��������� ��������� �������� �� ������ ��������
    }
}
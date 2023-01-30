using UnityEngine;

public class MinimumDistance : MonoBehaviour
{
    [Space]
    [Header("���� � ��")]
    public AIManager aIManager; // �� ��������

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (aIManager.AgreAI == true)
            {
                aIManager._animator.SetBool("Damage", true); // ���� �� ������ �� ������ � �� ���������� �� �������
                aIManager.AIStop();
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            aIManager._animator.SetBool("Damage", false); // ���� �� �� ������ �� ������ �� �� �������
            aIManager.AIWalk();
        }
    }
}
using UnityEngine;

public class Cartridge : MonoBehaviour
{
    [Space]
    [Header("�������� ����")]
    public float damage; // ���� ����

    [Space]
    [Header("�������� ����")]
    public float speed; // �������� ����

    [Space]
    [Header("��������")]
    public Animator _animator; // �������� ��� ��������

    [Space]
    [Header("������� ������ � ����")]
    public Transform mTransform;
    public Vector3 targetPosition; // ������� ������

    void LateUpdate()
    {
        mTransform.position = Vector3.MoveTowards(mTransform.position, targetPosition, speed * Time.deltaTime); //������� ���� � ������
        if (mTransform.position == targetPosition) _animator.SetBool("Death", true); //���������� ���� //���� ���� ��������� � ������
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player")) col.gameObject.GetComponent<CustomCharacterController>().MobHitAttacke(damage); // ������� ����

        speed = 0;
        _animator.SetBool("Death", true); //���������� ����
    }

    public void Destroys() => Destroy(gameObject);
}
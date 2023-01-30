using UnityEngine;

public class Cartridge : MonoBehaviour
{
    [Space]
    [Header("Скорость пули")]
    public float damage; // урон пули

    [Space]
    [Header("Скорость пули")]
    public float speed; // скорость пули

    [Space]
    [Header("Анимации")]
    public Animator _animator; // аниматор для анимаций

    [Space]
    [Header("Позиция игрока и пули")]
    public Transform mTransform;
    public Vector3 targetPosition; // позиция игрока

    void LateUpdate()
    {
        mTransform.position = Vector3.MoveTowards(mTransform.position, targetPosition, speed * Time.deltaTime); //двигаем пулю к игроку
        if (mTransform.position == targetPosition) _animator.SetBool("Death", true); //уничтожаем пулю //если пуля прилетела в игрока
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player")) col.gameObject.GetComponent<CustomCharacterController>().MobHitAttacke(damage); // наносим урон

        speed = 0;
        _animator.SetBool("Death", true); //уничтожаем пулю
    }

    public void Destroys() => Destroy(gameObject);
}
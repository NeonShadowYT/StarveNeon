using UnityEngine;

public class TurnController : MonoBehaviour //https://www.youtube.com/c/maximple
{
    private Animator _animator;
    private float slowMouseX;

    void Start() => _animator = GetComponent<Animator>();

    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        slowMouseX = Mathf.Lerp(slowMouseX, mouseX, 10 * Time.deltaTime);

        _animator.SetFloat("MouseX", slowMouseX);
    }
}

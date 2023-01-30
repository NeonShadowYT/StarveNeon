using UnityEngine;

public class MyFloater : MonoBehaviour // https://www.youtube.com/c/maximple, https://www.youtube.com/channel/UC2pH2qNfh2sEAeYEGs1k_Lg
{
    private Rigidbody rigidbody;
    private Transform mTransform;

    [Header("Характеристики плавающего предмета")]
    [Space]
    public float floatUpSpeed = 1f, floatUpSpeedLimit = 1.15f;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        mTransform = GetComponent<Transform>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 4)
        {
            float difference = (other.transform.position.y - mTransform.position.y) * floatUpSpeed;
            rigidbody.AddForce(new Vector3(0f, Mathf.Clamp((Mathf.Abs(Physics.gravity.y) * difference), 0, Mathf.Abs(Physics.gravity.y) * floatUpSpeedLimit), 0f), ForceMode.Acceleration);
            rigidbody.drag = 0.99f;
            rigidbody.angularDrag = 0.8f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 4)
        {
            rigidbody.drag = 0f;
            rigidbody.angularDrag = 0f;
        }
    }
}
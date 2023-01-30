using UnityEngine;
public class Arrow : MonoBehaviour
{
    public Rigidbody Rigidbody;
    public Transform mTransform;
    public Transform RopeTransform;
    public void SetTransformArrow()
    {
        mTransform.parent = RopeTransform;
        mTransform.position = RopeTransform.position;
        Rigidbody.isKinematic = true;
    }
    public void Shot(float velocity)
    {
        mTransform.parent = null;
        Rigidbody.isKinematic = false;
        Rigidbody.velocity = mTransform.forward * velocity * 20;
    }
}
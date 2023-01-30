using UnityEngine;

public class Falling : MonoBehaviour
{
    public Rigidbody rig;
    public Indicators indicators;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Terrain")) if (rig.velocity.y < -15) indicators.healthAmount -= 100;
    }
}

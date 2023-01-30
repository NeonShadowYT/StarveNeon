using Unity.Mathematics;
using UnityEngine;
public class InspectableObject : MonoBehaviour
{
    public Vector3 spawnPositionOffset;
    public Vector3 spawnRotationOffset;
    public Vector2 minMaxZoom = new Vector2(0.5f,2);
    public float defaultZoomValue = 1f;
}

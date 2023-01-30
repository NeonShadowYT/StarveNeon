using System.Collections.Generic;
using UnityEngine;
public class Doorframe : EdgeBlock, IHaveConnections, ICheckColliders
{
    [SerializeField] private GameObject doorConnection;
    protected override void Start()
    {
        foreach (GameObject ceilingConnection in ceilingConnections) ceilingConnection.SetActive(false);
        doorConnection.SetActive(false);
        base.Start();
    }
    public void CheckColliders()
    {
        detectedColliders = new List<Collider>();
        Collider[] currentColliders = Physics.OverlapBox(transform.position, new Vector3(0.1f,0.25f,1f), transform.rotation, ~LayerMask.GetMask("FoundationConnection", "EdgeConnection", "CeilingConnection", "DoorConnection"));
        detectedColliders.AddRange(currentColliders);
    }
    public void TurnOnConnections()
    {
        base.TurnOnConnections();
        doorConnection.SetActive(true);
    }
}
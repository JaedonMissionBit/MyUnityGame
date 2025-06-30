using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class ResetTrigger : MonoBehaviour
{
    public string playertag = "Player";
    public Transform startPos;
    private Vector3 spawnPoint;
    [SerializeField] private CinemachineFreeLook vcam; 

    void Start()
    {
       spawnPoint = startPos.position; 
    }
void OnTriggerEnter(Collider other)
{
    if (other.gameObject.CompareTag(playertag))
    {
        // Teleport transform:
        other.gameObject.transform.root.position = spawnPoint;
        vcam.OnTargetObjectWarped(other.gameObject.transform, spawnPoint);

        // Reset physics velocity if Rigidbody exists:
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        
    }
}

}

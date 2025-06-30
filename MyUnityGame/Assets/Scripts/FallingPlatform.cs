using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public string playertag = "Player";
    private Rigidbody rb;
    private Vector3 startPos;
    private Quaternion rot;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("FallingPlatform requires a Rigidbody.");
            enabled = false;
            return;
        }
        startPos = transform.position;
        rot = transform.rotation;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(playertag))
        {
            rb.isKinematic = false;
        }
    }

    [ContextMenu("Reset Platform")]
    public void ResetPlatform()
    {
        transform.SetPositionAndRotation(startPos, rot);
        rb.isKinematic = true;
    }

    [ContextMenu("testing platforms")]
    public void TestFall()
    {
        rb.isKinematic = false;
    }
}

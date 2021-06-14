using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow : MonoBehaviour
{
    private bool syncRotation = true;
    private Rigidbody rig;

    void Awake()
    {
        rig = transform.GetComponent<Rigidbody>();

        if (transform.parent == null)
            Debug.LogError("Arrows must have be a child object of camera at the start of the game.");
    }

    void Update()
    {
        if (transform.parent != null && !syncRotation)
            syncRotation = true;

        if (transform.parent == null && rig.velocity.magnitude > 1.2f && syncRotation)
            transform.forward = rig.velocity.normalized;
    }

    void OnCollisionEnter(Collision other)
    {
        syncRotation = false;
    }
}

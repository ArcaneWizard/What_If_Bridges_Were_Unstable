using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using UnityEngine;
using UnityEngine.XR;

public class Movement : MonoBehaviour
{
    private Rigidbody rig;

    public Camera camera;
    private int x, z = 0;

    public float moveSpeed = 10;
    public Vector2 jumpForce = new Vector2(0, 200f);
    public float sensitivity = 150f;
    private float xRotation = 0;

    void Awake()
    {
        rig = transform.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        //keyboard movement 
        x = z = 0;

        if (Input.GetKey(KeyCode.W))
            z++;
        if (Input.GetKey(KeyCode.S))
            z--;
        if (Input.GetKey(KeyCode.A))
            x--;
        if (Input.GetKey(KeyCode.D))
            x++;

        Vector3 newRigVelocity = Quaternion.Euler(0, transform.eulerAngles.y, 0) * new Vector3(x * moveSpeed, rig.velocity.y, z * moveSpeed);
        float xChange = Mathf.Abs(newRigVelocity.x) > 0.4f ? newRigVelocity.x : 0;
        float zChange = Mathf.Abs(newRigVelocity.z) > 0.4f ? newRigVelocity.z : 0;

        rig.velocity = new Vector3(xChange, rig.velocity.y, zChange);
        camera.transform.position = transform.position + new Vector3(0, 3.2f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        //use mouse to look around / turn camera
        lookAroundCode();

        //jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("jumped once");
            rig.AddForce(jumpForce);
        }

    }

    private void lookAroundCode()
    {

        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * 2;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * 2;

        Vector3 rot = camera.transform.localEulerAngles;
        float desiredX = rot.y + mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        camera.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
        transform.localRotation = Quaternion.Euler(0, desiredX, 0);
    }
}

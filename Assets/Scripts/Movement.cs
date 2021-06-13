using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using UnityEngine;
using UnityEngine.XR;

public class Movement : MonoBehaviour
{
    private Rigidbody rig;
    private AudioSource audio;

    public GameObject[] feet;

    public Camera camera;
    private int x, z = 0;

    public float moveSpeed = 10;
    public Vector2 jumpForce = new Vector2(0, 200f);
    public float sensitivity = 150f;
    private float xRotation = 0;

    private bool grounded = false;

    void Awake()
    {
        rig = transform.GetComponent<Rigidbody>();
        audio = transform.GetComponent<AudioSource>();
        audio.Pause();
    }

    void Start()
    {
        StartCoroutine(groundCheck());
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

        if ((x != 0 || z != 0) && !audio.isPlaying && grounded)
        {
            audio.Play();
        }
        else if (x == 0 && z == 0 || !grounded)
            audio.Pause();
    }

    // Update is called once per frame
    void Update()
    {
        //use mouse to look around / turn camera
        lookAroundCode();

        //jump
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
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

    private IEnumerator groundCheck()
    {
        grounded = isGrounded();
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(groundCheck());
    }

    private bool isGrounded()
    {
        RaycastHit hit, hit2, hit3, hit4;

        Physics.Raycast(feet[0].transform.position, Vector3.down, out hit, 0.6f);
        Physics.Raycast(feet[1].transform.position, Vector3.down, out hit2, 0.6f);
        Physics.Raycast(feet[2].transform.position, Vector3.down, out hit3, 0.6f);
        Physics.Raycast(feet[3].transform.position, Vector3.down, out hit4, 0.6f);

        if (hit.point == Vector3.zero && hit2.point == Vector3.zero && hit3.point == Vector3.zero && hit4.point == Vector3.zero)
        {
            return false;
        }
        else
            return true;
    }
}

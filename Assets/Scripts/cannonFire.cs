using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cannonFire : MonoBehaviour
{
    public Transform[] launchPoints;
    public GameObject cannonball;
    public Transform Player;

    public Vector3 multiplier = new Vector3(100, 100, 100);

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            foreach (Transform launchPoint in launchPoints)
            {
                GameObject cB = Instantiate(cannonball, launchPoint.position, Quaternion.Euler(0, 0, 0));
                Vector3 dir = (Player.position - launchPoints[2].position).normalized;
                cB.transform.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                cB.transform.GetComponent<Rigidbody>().AddForce(new Vector3(dir.x * multiplier.x, multiplier.y, dir.z * multiplier.z));
            }
        }
    }
}

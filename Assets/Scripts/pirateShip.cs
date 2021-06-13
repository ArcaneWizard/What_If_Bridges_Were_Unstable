using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pirateShip : MonoBehaviour
{
    private Vector3 pirateTarget;
    private Rigidbody rig;

    private float speed = 3.5f;
    private float rotationSpeed = 1f;

    private float distance;
    public Vector3 nextPosition;

    // Start is called before the first frame update
    void Awake()
    {
        rig = transform.GetComponent<Rigidbody>();

        pirateTarget = new Vector3(transform.parent.position.x, transform.position.y, transform.parent.position.z);
        distance = (pirateTarget - transform.position).magnitude;

        float newDistance = Random.Range(0, distance);
        Vector2 newOffset = Random.insideUnitCircle.normalized * newDistance;
        nextPosition = new Vector3(pirateTarget.x + newOffset.x, transform.position.y, pirateTarget.z + newOffset.y);
        transform.LookAt(nextPosition - transform.position);

        speed = Random.Range(3.5f, 6f);
    }

    // Update is called once per frame
    void Update()
    {
        if ((transform.position - nextPosition).magnitude > 4f)
        {
            Vector3 dir = (nextPosition - transform.position).normalized;
            rig.velocity = new Vector3(dir.x * speed, 0, dir.z * speed);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotationSpeed);
        }
        else
            calculateNextPosition();

        if ((transform.position - pirateTarget).magnitude < 10f)
        {
            playerHP.hp -= 20;
            Destroy(gameObject);
        }
    }

    private Vector3 calculateNextPosition()
    {
        float newDistance = Random.Range(0, distance);
        Vector2 newOffset = Random.insideUnitCircle.normalized * newDistance;
        nextPosition = new Vector3(pirateTarget.x + newOffset.x, transform.position.y, pirateTarget.z + newOffset.y);
        return nextPosition;
    }
}

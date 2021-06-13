using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    [Header("Arrow Settings")]
    public Transform arrowSpawnPoint;
    public Transform[] arrows;
    public Vector2 force = new Vector2(0, 0);

    private int arrowIndex = 0;
    private Transform camera;

    public Image reloadBar;
    public float reloadTime = 1.0f;
    private bool isReloading = false;

    [Header("Bow Power Settings")]
    public Image powerBar;
    public float powerDivider = 1.9f;
    private float timer = 0;
    private float power = 0;

    void Awake()
    {
        camera = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        //hold left click and release to shoot bow
        if (Input.GetMouseButtonDown(0))
            timer = 0;
        if (Input.GetMouseButton(0))
            timer += Time.deltaTime;
        if (Input.GetMouseButtonUp(0) && !isReloading)
        {
            power = Mathf.Min(timer, 1.7f);
            StartCoroutine(shootArrow());
        }

        powerBar.fillAmount = timer / 1.7f;
    }

    private IEnumerator shootArrow()
    {
        Rigidbody rig = arrows[arrowIndex].GetComponent<Rigidbody>();
        Vector3 arrowForce = transform.parent.forward * force.x + transform.parent.up * force.y;

        //unfreeze arrow, reset its position, detach it from following the player's FOV
        rig.constraints = RigidbodyConstraints.None;
        rig.velocity = new Vector3(0, 0, 0);
        arrows[arrowIndex].parent = null;

        //enable the arrow's collider and shoot it with force
        arrows[arrowIndex].transform.GetChild(0).gameObject.SetActive(true);
        rig.AddForce(arrowForce * power / powerDivider);

        //reload for the next arrow
        isReloading = true;
        for (int i = 1; i <= 10; i++)
        {
            reloadBar.fillAmount = (float)i / 10f;
            yield return new WaitForSeconds(reloadTime / 10f);
        }

        arrowIndex = ++arrowIndex % arrows.Length;
        rig = arrows[arrowIndex].GetComponent<Rigidbody>();

        //disable the next arrow's collider and movement
        arrows[arrowIndex].transform.GetChild(0).gameObject.SetActive(false);
        rig.constraints = RigidbodyConstraints.FreezeAll;
        rig.velocity = new Vector3(0, 0, 0);

        //make it follow the player's FOV as they look around, teleport it to the bow with the proper rotation
        arrows[arrowIndex].parent = camera;
        rig.transform.localScale = new Vector3(1, 1, 1);
        arrows[arrowIndex].transform.position = arrowSpawnPoint.position;
        arrows[arrowIndex].transform.localRotation = Quaternion.Euler(0, 0, 0);
        arrows[arrowIndex].gameObject.SetActive(true);

        //confirm the player isn't reloading anymore
        isReloading = false;
    }
}

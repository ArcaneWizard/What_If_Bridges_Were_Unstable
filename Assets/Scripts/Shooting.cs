using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    [Header("Arrow Settings")]
    public Transform arrowSpawnPoint;
    public Transform[] arrows;
    public Vector3 force = new Vector3(0, 0, 0);

    public int arrowIndex = 0;
    private Rigidbody arrowRig;
    private Transform camera;

    public Image reloadBar;
    public float reloadTime = 1.0f;
    private bool isReloading = false;

    [Header("Bow Power Settings")]
    public Image powerBar;
    public float powerDivider = 1.9f;
    private float timer = 0;
    private float power = 0;

    private AudioSource audio;
    private Animator animator;

    void Awake()
    {
        camera = transform.parent;
        audio = transform.GetComponent<AudioSource>();
        animator = transform.GetComponent<Animator>();

        arrowRig = arrows[arrowIndex].GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //hold left click and release to shoot bow
        if (Input.GetMouseButtonDown(0))
        {
            timer = 0;

            //start winding bow animation 
            animator.SetInteger("Stage", 1);
        }
        if (Input.GetMouseButton(0))
        {
            timer += Time.deltaTime;
            //drag the arrow back over time as the bow is winding for a shot 
            if (arrows[arrowIndex].localPosition.z > 1.461f)
                arrows[arrowIndex].Translate(new Vector3(-0.011f, 0, -0.147f) * Time.deltaTime * 4);
        }
        if (Input.GetMouseButtonUp(0) && !isReloading)
        {
            power = Mathf.Min(timer, 1.7f);
            StartCoroutine(shootArrow());

            audio.Play();
            animator.SetInteger("Stage", 2);
        }

        powerBar.fillAmount = timer / 1.7f;
    }

    private IEnumerator shootArrow()
    {
        arrowRig = arrows[arrowIndex].GetComponent<Rigidbody>();

        //since the bow is to the left of the cross hair, apply a small force to the left too
        Vector3 arrowForce = transform.parent.forward * force.x + transform.parent.up * force.y
        - transform.parent.right * force.z * (powerDivider / power);

        //unfreeze arrow, reset its position, detach it from following the player's FOV
        arrowRig.constraints = RigidbodyConstraints.None;
        arrowRig.velocity = new Vector3(0, 0, 0);
        arrows[arrowIndex].parent = null;

        //enable the arrow's collider and shoot it with force
        arrows[arrowIndex].transform.GetChild(0).gameObject.SetActive(true);
        arrowRig.AddForce(arrowForce * power / powerDivider);

        //reload for the next arrow
        isReloading = true;
        for (int i = 1; i <= 10; i++)
        {
            reloadBar.fillAmount = (float)i / 10f;
            yield return new WaitForSeconds(reloadTime / 10f);
        }

        arrowIndex = ++arrowIndex % arrows.Length;
        arrowRig = arrows[arrowIndex].GetComponent<Rigidbody>();

        //disable the next arrow's collider and movement
        arrows[arrowIndex].transform.GetChild(0).gameObject.SetActive(false);
        arrowRig.constraints = RigidbodyConstraints.FreezeAll;
        arrowRig.velocity = new Vector3(0, 0, 0);

        //make it follow the player's FOV as they look around, teleport it to the bow with the proper rotation
        arrows[arrowIndex].parent = camera;
        arrowRig.transform.localScale = new Vector3(1, 1, 1);
        arrows[arrowIndex].transform.position = arrowSpawnPoint.position;
        arrows[arrowIndex].transform.localRotation = Quaternion.Euler(0, 0, 0);
        arrows[arrowIndex].gameObject.SetActive(true);

        //confirm the player isn't reloading anymore
        isReloading = false;
    }
}

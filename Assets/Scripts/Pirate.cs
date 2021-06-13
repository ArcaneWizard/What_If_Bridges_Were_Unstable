using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pirate : MonoBehaviour
{
    private int health = 2;
    public Material injured;

    private Material[] og = new Material[30];
    private Transform ship;

    void Awake()
    {
        if (tag != "Large Pirate Ship")
        {
            ship = transform.GetChild(0).transform.GetChild(0);
            for (int i = 0; i < ship.transform.childCount; i++)
            {
                og[i] = ship.transform.GetChild(i).transform.GetComponent<Renderer>().material;
            }
        }

        if (tag == "Large Pirate Ship")
            health = 5;
    }

    public IEnumerator tookDamage()
    {
        health--;
        if (health >= 0)
            transform.GetComponent<AudioSource>().Play();

        if (health == 1)
            transform.GetComponent<Rigidbody>().mass = 5;

        if (gameObject.tag != "Large Pirate Ship")
        {
            for (int i = 0; i < ship.transform.childCount; i++)
            {
                ship.transform.GetChild(i).transform.GetComponent<Renderer>().material = injured;
            }

            yield return new WaitForSeconds(0.4f);

            if (health == 1)
            {
                for (int i = 0; i < ship.transform.childCount; i++)
                {
                    ship.transform.GetChild(i).transform.GetComponent<Renderer>().material = og[i];
                }
            }
        }
        else
        {
            print("yeah");

        }

        if (health <= 0)
            Destroy(gameObject, 2.0f);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 11)
        {
            StartCoroutine(tookDamage());
        }
    }
}

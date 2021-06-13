using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pirate : MonoBehaviour
{
    public Material injured;
    private int health = 2;

    public void tookDamage()
    {
        print("hit" + ", " + health);
        health--;

        if (health == 1)
        {
            for (int i = 0; i <= 1; i++)
            {
                transform.GetChild(i).transform.GetComponent<Renderer>().material = injured;
            }
        }

        if (health == 0)
            Destroy(gameObject);
    }
}

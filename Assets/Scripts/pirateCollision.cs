using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pirateCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision col)
    {
        print("hit");
        //        StartCoroutine(transform.parent.transform.parent.transform.parent.GetComponent<Pirate>().tookDamage());
        //col.transform.GetChild(0).gameObject.SetActive(false);
    }
}

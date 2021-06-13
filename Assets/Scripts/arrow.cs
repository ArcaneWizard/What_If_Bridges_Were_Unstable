using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow : MonoBehaviour
{
    bool once;

    void Update()
    {
        if (transform.position.y < -104f && !once)
        {
            once = true;
            StartCoroutine(playRippleSound());
        }
    }

    private IEnumerator playRippleSound()
    {
        gameObject.transform.Translate(Vector3.down * 20f);
        //transform.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(1f);
        once = false;
        gameObject.SetActive(false);
    }
}

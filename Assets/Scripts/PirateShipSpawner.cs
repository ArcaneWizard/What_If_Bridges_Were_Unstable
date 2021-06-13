using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PirateShipSpawner : MonoBehaviour
{
    public Text narrator;
    public Text narrator2;
    public float narrationSpeed = 20f;
    public float pauseBtwnMessages = 2.2f;
    public float pauseAfterClearingMsg = 0.4f;

    private bool narrating = false;
    private Queue<string> messages = new Queue<string>();


    // Start is called before the first frame update
    void Start()
    {
        narrateMessage("Hey, look at this amazing rig!");
        narrateMessage("Wave 1");
    }

    // Update is called once per frame
    void Update()
    {
        if (messages.Count > 0 && !narrating)
        {
            narrating = true;
            StartCoroutine(narrateMsg(messages.Dequeue()));
        }

    }

    public void narrateMessage(string msg)
    {
        messages.Enqueue(msg);
    }

    private IEnumerator narrateMsg(string msg)
    {
        foreach (char c in msg)
        {
            narrator.text += c;
            narrator2.text += c;
            yield return new WaitForSeconds(10f / narrationSpeed);
        }

        yield return new WaitForSeconds(pauseBtwnMessages);
        narrator.text = "";
        narrator2.text = "";
        yield return new WaitForSeconds(pauseAfterClearingMsg);
        narrating = false;
    }
}

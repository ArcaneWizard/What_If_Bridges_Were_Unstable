using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Narration : MonoBehaviour
{
    public Text narrator;
    public Text narrator2;
    private PirateNavigation pirateNavigation;
    public float narrationSpeed = 45f;
    public float pauseBtwnMessages = 2.5f;
    public float pauseAfterClearingMsg = 0.4f;

    private bool narrating = false;
    private Queue<string> messages = new Queue<string>();

    private bool firstShip;
    private bool firstWave;

    void Awake()
    {
        pirateNavigation = transform.GetComponent<PirateNavigation>();
    }

    // Start is called before the first frame update
    void Start()
    {
        narrateMessage("Hey, look at this amazing rig!");
        narrateMessage("Wait you have a bow? Hold your mouse down for power and then release to shoot.");
        narrateMessage("Now, it sure would be a bummer if pirates attacked ur rig.");

        narrateMessage("Wait, is that a pirate ship in the horizon?");
        narrateMessage("Get ready for an ONSLAUGHT!");
        narrateMessage("Get ready for an ONSLAUGHT!");

        narrateMessage("Wave 1");
    }

    // Update is called once per frame
    void Update()
    {
        if (messages.Count > 0 && !narrating)
        {
            if (messages.Count == 4 && !firstShip)
            {
                firstShip = true;
                pirateNavigation.spawnPirateShip();

            }
            else if (messages.Count == 1 && !firstWave)
            {
                firstWave = true;
                StartCoroutine(pirateNavigation.spawnPirateShips());
            }

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

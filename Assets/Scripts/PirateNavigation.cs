using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateNavigation : MonoBehaviour
{
    public Vector2 spawnDistance = new Vector2(320f, 520f);
    private Narration narration;

    public Transform[] pirateShips;

    private int wave = 1;
    private int spawnCount = 0;
    private int totalCount = 5;

    private bool waiting = false;
    private float waitingTimer = 0f;

    void Awake()
    {
        narration = transform.GetComponent<Narration>();
    }

    public IEnumerator spawnPirateShips()
    {
        spawnPirateShip();
        spawnCount++;
        yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));

        if (spawnCount < totalCount)
            StartCoroutine(spawnPirateShips());
        else if (spawnCount == totalCount)
        {
            waiting = true;
            waitingTimer = 7f * totalCount;
        }
    }

    void Update()
    {
        if (waiting)
        {
            if (transform.childCount == 3 || transform.childCount == 4)
            {
                nextWave();
                waiting = false;
                waitingTimer = -1f;
            }
        }

        if (waitingTimer > 0 && waiting)
        {
            waitingTimer -= Time.deltaTime;
        }
        else if (waitingTimer <= 0 && waiting)
        {
            nextWave();
            waiting = false;
        }
    }

    private void nextWave()
    {
        totalCount++;
        spawnCount = 0;
        narration.narrateMessage("Wave " + (totalCount - 4));
        StartCoroutine(spawnPirateShips());
    }

    public void spawnPirateShip()
    {
        //select random pirateship
        int shipType = selectRandomPirateShip();


        //spawn it on the outskirts of the ocean and activate it
        Transform pirateShip = Instantiate(pirateShips[shipType], new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
        Vector2 offset = Random.insideUnitCircle.normalized * Random.Range(spawnDistance.x, spawnDistance.y);

        //find that ship type's specific elevation
        float elevation = 0;
        if (shipType == 0)
            elevation = -99f;
        else if (shipType == 1)
            elevation = -95.4f;
        else if (shipType == 2)
            elevation = -100f;

        pirateShip.parent = transform;
        pirateShip.position = new Vector3(transform.position.x + offset.x, elevation, transform.position.z + offset.y);
        pirateShip.gameObject.SetActive(true);
    }

    private int selectRandomPirateShip()
    {
        int r = Random.Range(0, 11);
        if (r >= 1)
            return 0;
        else if (r <= 3 && r >= 1)
            return 1;
        else
            return 2;
    }
}

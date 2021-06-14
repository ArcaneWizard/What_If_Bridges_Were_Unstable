using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class playerHP : MonoBehaviour
{
    public static int hp = 100;
    public Image health;
    public static int maxHP = 100;

    void Awake()
    {
        hp = maxHP;
    }

    void Update()
    {
        health.fillAmount = (float)hp / (float)maxHP;
    }
}

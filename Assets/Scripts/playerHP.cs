using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class playerHP : MonoBehaviour
{
    public static int hp = 100;
    public static int maxHP = 100;

    void Awake()
    {
        hp = maxHP;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timeManager : MonoBehaviour
{
    public static bool timerAttack;

    void Start()
    {
        
    }

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}

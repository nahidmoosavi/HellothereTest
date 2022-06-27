using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public event Action OnProcessPointsEnd;


    /// <summary>
    /// Singletone
    /// </summary>
    private static EventManager instance;
    public static EventManager EventManagerInstance { get { return instance; } }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }



    public void ProcessingPointsEnd()
    {
        if (OnProcessPointsEnd != null) OnProcessPointsEnd();
    }



}

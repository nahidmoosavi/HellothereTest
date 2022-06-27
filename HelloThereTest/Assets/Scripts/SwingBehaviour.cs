using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SwingBehaviour : MonoBehaviour
{
    [SerializeField] float MaxAngleDeflection = 30.0f;
    [SerializeField] float SpeedOfPendulum = 1.0f;


    private Transform initialTransform;
    private ObjectAssigner objectAssigner;
    float angle = 0;

    private void Start()
    {
        objectAssigner = this.GetComponentInChildren<ObjectAssigner>();
        initialTransform = objectAssigner.transform;

        EventManager.EventManagerInstance.OnProcessPointsEnd += OnResetPositionAndRotation;
    }


    private void FixedUpdate()
    {
        if (objectAssigner.isHitted == true)
        {
            angle = 0;
            return;
        }

        else
        {
            Swing();
        }
    }

    private void Swing()
    {
        angle = MaxAngleDeflection * Mathf.Sin(Time.time * SpeedOfPendulum);
        this.transform.localRotation = Quaternion.Euler(0, 0, angle);
    }



    private void OnResetPositionAndRotation()
    {
        this.transform.localRotation = Quaternion.Euler(0, 0, -angle/10);
    }

    private void OnDestroy()
    {
        EventManager.EventManagerInstance.OnProcessPointsEnd -= OnResetPositionAndRotation;

    }
}

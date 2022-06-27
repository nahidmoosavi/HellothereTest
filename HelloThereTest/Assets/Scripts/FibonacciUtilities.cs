using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FibonacciUtilities
{
    private const float PI = (float)Math.PI; //Const float for PI value ≃ 3.141592...
    private float goldenRatio = (float)(1 + Math.Sqrt(5)) * 0.5f; //float value calculates  Golden ratio ≃ 1.618033...

    //Todo Check performance if generating a new fibunachi is more performant or sorting neghbours

    /// <summary>
    /// Calculate Fibonacci Latice in 3D for a sphere to generates points evenly distubuted on a sphere 
    /// </summary>
    /// <param name="objectsNumber"></param>
    /// <param name="objectsPositions"></param>
    public void CalculateFibunacciLattice(int objectsNumber, Vector3[] objectsPositions)
    {
        ///(theta, phi)->(x,y,z): (cos(theta)sin(phi), sin(theta)sin(phi), cos(phi))
        ///However because of the lefthanded coordination system of unity, values change to adapt to it
        ///For unity coordination system (theta, phi)->(x,y,z): (sin(theta)sin(phi), cos(phi), cos(theta)sin(phi))
        for (int i = 0; i < objectsNumber; i++)
        {
            float theta = (2 * PI * i) / goldenRatio;
            float value = i + 0.5f;
            float phi = (float)Math.Acos(1 - 2 * value / objectsNumber);

            float x = (float)(Math.Sin(theta) * Math.Sin(phi));
            float y = (float)Math.Cos(phi);
            float z = -(float)(Math.Cos(theta) * Math.Sin(phi));

            objectsPositions[i] = new Vector3(x, y, z);
        }

    }

    /// <summary>
    /// Shows the objects one by one according to their positions in vectors coresponding slot
    /// </summary>
    /// <param name="vectors"></param>
    /// <param name="assignedObjects"></param>
    /// <param name="scaleValue"></param>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    public IEnumerator AssignObjects(Vector3[] vectors, Vector3 parentPosition, List<GameObject> assignedObjects, float scaleValue, float waitTime)
    {
        for (int i = 0; i < vectors.Length; i++)
        {
            assignedObjects[i].transform.position = Vector3.zero;
            assignedObjects[i].transform.position = vectors[i] * scaleValue;
            assignedObjects[i].transform.position += parentPosition;
            yield return new WaitForSeconds(waitTime);
            assignedObjects[i].SetActive(true);
        }

    }

    /// <summary>
    /// Ascending sorts the FibonacciLattice's points based on the distance to a point on the sphere (nearest neighbours)
    /// </summary>
    /// <param name="point"></param> 
    /// <param name="objectsPositions"></param>
    /// <param name="objectsNumber"></param>
    /// <returns></returns>
    public Vector3[] SortFibunacciSeries(Vector3 point, Vector3[] objectsPositions, int objectsNumber)
    {
        float distance = 0;
        SortedList<float, Vector3> pointsList = new SortedList<float, Vector3>();

        foreach (Vector3 item in objectsPositions)
        {
            distance = Vector3.Distance(point, item);

            try
            {
                //o(nlogn)
                pointsList.Add(distance, item);
            }
            catch (ArgumentException)
            {
                Debug.Log("An element with Key = \"txt\" already exists.");
            }

        }

        Vector3[] vectors = new Vector3[objectsNumber];
        //O(n)
        pointsList.Values.CopyTo(vectors, 0);
        return vectors;
    }

    /// <summary>
    /// Hides current objects in the assignedObjects list
    /// </summary>
    /// <param name="assignedObjects"></param>
    public void ClearObjects(List<GameObject> assignedObjects)
    {
        if (assignedObjects.Count <= 0) return;

        foreach (GameObject item in assignedObjects)
        {
            item.SetActive(false);
        }

    }


    //private void CalculateFibunacciOffsetLattice(Vector3 hitPoint)
    //{
    //    float epsilon = 0;

    //    if (objectsNumber >= 24 && objectsNumber < 177)
    //        epsilon = 1.24f;
    //    else if (objectsNumber >= 177 && objectsNumber < 890)
    //        epsilon = 3.33f;
    //    else
    //        epsilon = 10;


    //    for (int i = 0; i < objectsNumber; i++)
    //    {
    //        float theta = (2 * PI * i) / goldenRatio;
    //        float phi = (float)Math.Acos(1 - 2 * (i + epsilon) / (objectsNumber - 1 + 2 * epsilon));
    //        float z = -(float)(Math.Cos(theta) * Math.Sin(phi));
    //        float x = (float)(Math.Sin(theta) * Math.Sin(phi));
    //        float y = (float)Math.Cos(phi);

    //        objectsPositions[i] = new Vector3(x, y, z);
    //    }

    //}



    //private void CalculateCDFMethod(Vector3 hitPoint)
    //{
    //    System.Random rand = new System.Random();

    //    //objectsPositions[0] = new Vector3(hitPoint.x, hitPoint.y, hitPoint.z);
    //    for (int i = 0; i < objectsNumber; i++)
    //    {
    //        float next1 = (float)rand.NextDouble();
    //        float next2 = (float)rand.NextDouble();
    //        float theta = 2 * PI * next1;
    //        float phi = (float)Math.Acos(1 - 2 * next2);
    //        float z = -(float)(Math.Cos(theta) * Math.Sin(phi));
    //        float x = (float)(Math.Sin(theta) * Math.Sin(phi));
    //        float y = (float)Math.Cos(phi);

    //        objectsPositions[i] = new Vector3(x, y, z);
    //    }
    //}
}

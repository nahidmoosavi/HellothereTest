using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    [SerializeField] int pointCount = 100;
    [SerializeField] float radius = 14;
    [SerializeField] Color startColor, endColor;
    [SerializeField] float pointSpeed = 1f;
    [SerializeField] float pointSize = 0.15f;
    [SerializeField] GameObject ObjectPrefab;

    private List<AnimatedVector> animatedVectors = new List<AnimatedVector>();
    private const float PI = (float)Math.PI; //Const float for PI value ≃ 3.141592...
    private float goldenRatio = (float)(1 + Math.Sqrt(5)) * 0.5f; //float value calculates  Golden ratio ≃ 1.618033...
    private FibonacciUtilities fibonacci = new FibonacciUtilities();

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 origin = Origin();
            if (animatedVectors.Count > pointCount)
            {
                int dif = animatedVectors.Count - pointCount;
                animatedVectors.RemoveRange(pointCount, dif);
            }

            for (int i = 0; i < pointCount; i++)
            {
                Vector3 targetPoint = FibonacciLattice(i);
                if (i >= animatedVectors.Count)
                {
                    animatedVectors.Add(new AnimatedVector(origin, targetPoint));
                }
                else
                {
                    animatedVectors[i].target = targetPoint;
                }
                DrawPoints();
                animatedVectors[i].Animate(pointSpeed);
            }
        }
        
    }

        Vector3 Origin()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.transform.position.z;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 50f))
        {
                return hitInfo.collider.tag == "Player" ? hitInfo.point : Vector3.zero;
        }
        else return Vector3.zero;
    }

    void DrawPoints()
    {
        for (int i = 0; i < animatedVectors.Count; i++)
        {
            float t = Mathf.InverseLerp(0, animatedVectors.Count, i);
            //ObjectPrefab.GetComponent<MeshRenderer>().material.color = Color.Lerp(startColor, endColor, t);
            Vector3 targetPos = transform.TransformPoint(animatedVectors[i].point) * radius;
            Instantiate(ObjectPrefab,targetPos, Quaternion.identity);
        }
    }

    Vector3 FibonacciLattice(int index)
    {
        float theta = (2 * PI * index) / goldenRatio;
        float value = index + 0.5f;
        float phi = (float)Math.Acos(1 - 2 * value / pointCount);

        float x = (float)(Math.Sin(theta) * Math.Sin(phi));
        float y = (float)Math.Cos(phi);
        float z = -(float)(Math.Cos(theta) * Math.Sin(phi));

        return new Vector3(x, y, z);

    }

}

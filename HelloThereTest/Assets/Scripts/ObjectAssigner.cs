using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

public class ObjectAssigner : MonoBehaviour
{
    [SerializeField] int objectsNumber = 100;
    [SerializeField] Vector3[] objectsPositions;
    [SerializeField] GameObject ObjectPrefab;
    [SerializeField] float waitTime = 0.1f;

    List<GameObject> assignedObjects = new List<GameObject>();
    
    private float scaleValue = 0;
    private FibonacciUtilities fibonacci = new FibonacciUtilities();
    private float timer = 0;
    public bool isHitted { get; private set; }

    private void Start()
    {

        objectsPositions = new Vector3[objectsNumber];
        fibonacci.CalculateFibunacciLattice(objectsNumber, objectsPositions);
        InstansiatObjects();

        scaleValue = this.transform.lossyScale.x / 2;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= ((objectsNumber + objectsNumber / 2) * waitTime))
        {
            if (Input.GetButtonDown("Fire1"))
            {
                timer = 0;
                ShowObjects();
            }
        }
    }



    /// <summary>
    /// Instantiates Objects in the start according to their corresponding position in objectsPositions array
    /// </summary>
    private void InstansiatObjects()
    {
        for (int i = 0; i < objectsNumber; i++)
        {
            GameObject temp = Instantiate(ObjectPrefab);
            assignedObjects.Add(temp);
            assignedObjects[i].transform.parent = this.transform;
            assignedObjects[i].transform.position = objectsPositions[i] * scaleValue;
            assignedObjects[i].SetActive(false);
        }
    }

    /// <summary>
    /// Starts the courutine of Mouse detection
    /// </summary>
    private void ShowObjects()
    {
        StartCoroutine(DetectMouseClickPosition());
    }

    /// <summary>
    /// Detects the mouse Click on Sphere surface and shows object destributaion form the cliked points
    /// </summary>
    private IEnumerator DetectMouseClickPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.transform.position.z;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        //Ray ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
        //Debug.DrawRay(ray.origin, ray.direction, Color.green, 5f);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 50f))
        {
            if (hitInfo.collider.tag == "Player")
            {
                isHitted = true; //If the sphere is hitted, freezes the pendulum's position until the end of AssignObjects work to showing all the objects
                fibonacci.ClearObjects(assignedObjects);
                Vector3[] vectors = fibonacci.SortFibunacciSeries(hitInfo.point, objectsPositions, objectsNumber);
                StartCoroutine(fibonacci.AssignObjects(vectors, this.transform.position, assignedObjects, scaleValue, waitTime));
            }
            else
            {
                fibonacci.ClearObjects(assignedObjects);
            }
            yield return new WaitForSeconds((objectsNumber * 2) * waitTime);

            if(SceneManager.GetActiveScene().name == "Fibunacci Scene Moving Object")
            {
                EventManager.EventManagerInstance.ProcessingPointsEnd();
            }

            isHitted = false;
        }

    }

}

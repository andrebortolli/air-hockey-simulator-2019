using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoModeCamera : MonoBehaviour
{
    public GameObject gameObjectToLookAt;
	// Update is called once per frame
	void Update ()
    {
        //if (gameObjectToLookAt)
        //{
        //    transform.LookAt(gameObjectToLookAt.transform);
        //}
        transform.Rotate(Vector3.down * Time.deltaTime * 10);
        GetComponentInChildren<Camera>().fieldOfView = 100;
    }
}

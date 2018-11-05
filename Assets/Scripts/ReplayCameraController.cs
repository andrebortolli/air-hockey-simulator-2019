using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayCameraController : MonoBehaviour {

    public GameObject[] replayCameras;

    public GameObject GetRandomCamera()
    {
        if (replayCameras != null)
        {
            for (int i = 0; i< replayCameras.Length; i++)
            {
                if (replayCameras[i] == null)
                {
                    return null;
                }
            }
            Debug.Log(replayCameras.Length);
            return replayCameras[Random.Range(0, replayCameras.Length)];
        }
        else
        {
            return null;
        }
    }

    public GameObject GetCamera(int cameraNumber)
    {
        if (replayCameras[cameraNumber] != null)
        {
            return replayCameras[cameraNumber];
        }
        else
        {
            return null;
        }
    }
}

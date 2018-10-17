using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraWithPlayer : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;

    private void Start()
    {
        offset = transform.position - player.transform.position;
    }
    void LateUpdate ()
    {
		transform.position = Vector3.Lerp(transform.position, player.transform.position + offset, Time.deltaTime * 5.0f);
	}
}

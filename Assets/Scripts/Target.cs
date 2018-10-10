using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private GameController gameController;
    public GameObject[] goal = new GameObject[2];
    
    public int goalToFollow = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	void FixedUpdate ()
    {
        transform.LookAt(goal[goalToFollow].transform, Vector3.up);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Area Player 1")
        {
            goalToFollow = 1;
            //GameObject.FindWithTag("Player 1").GetComponent<PlayerController>().ReturnToGoal = true;
            //GameObject.FindWithTag("Player 2").GetComponent<PlayerController>().ReturnToGoal = false;
            //Debug.Log("1");
        }
        else if (other.gameObject.tag == "Area Player 2")
        {
            goalToFollow = 0;
            //GameObject.FindWithTag("Player 1").GetComponent<PlayerController>().ReturnToGoal = false;
            //GameObject.FindWithTag("Player 2").GetComponent<PlayerController>().ReturnToGoal = true;
           // Debug.Log("0");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Area Player 1")
        {
            goalToFollow = 0;
            GameObject.FindWithTag("Player 1").GetComponent<PlayerController>().ReturnToStartingPosition = false;
            GameObject.FindWithTag("Player 2").GetComponent<PlayerController>().ReturnToStartingPosition = true;
            //Debug.Log("1");
        }
        else if (other.gameObject.tag == "Area Player 2")
        {
            goalToFollow = 1;
            GameObject.FindWithTag("Player 1").GetComponent<PlayerController>().ReturnToStartingPosition = true;
            GameObject.FindWithTag("Player 2").GetComponent<PlayerController>().ReturnToStartingPosition = false;
            //Debug.Log("0");
        }
    }
}

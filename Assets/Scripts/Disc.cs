using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disc : MonoBehaviour
{
    private Rigidbody rb;
    public float throwSpeed;
    private Vector3 startingPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        startingPosition = transform.position;
    }

    void Throw(float speed)
    {
        Vector3 randomEulerAngle = new Vector3(Random.Range(60.0f, 120.0f), 0.0f, Random.Range(60.0f, 120.0f));
        //If value is 0, the disc will head towards player 1 (negative heading); Else the disc will remain with positive heading, going towards the player 2.
        if (Random.Range(0,1) == 0)
        {
            randomEulerAngle = randomEulerAngle * -1;
        }
        rb.AddForce(randomEulerAngle * speed, ForceMode.Acceleration);
    }

    // Use this for initialization
    void Start()
    {
        if (rb != null)
        {
            Throw(throwSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Back Goal")
        {
            Debug.Log("Player 2 scored a goal!");
            transform.position = startingPosition;
            Start();
        }
        if (other.gameObject.tag == "Front Goal")
        {
            Debug.Log("Player 1 scored a goal!");
            transform.position = startingPosition;
            Start();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disc : MonoBehaviour
{
    private Rigidbody rb;
    public float throwSpeed;
    private Vector3 startingPosition;
    private AudioSource audioSource;
    public AudioClip[] sfx;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
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

    private void PlaySFX(AudioClip sfx)
    {
        if (sfx)
        {
            audioSource.clip = sfx;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("No SFX. Cannot Play.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Back Goal")
        {
            Debug.Log("Player 2 scored a goal!");
            transform.position = startingPosition;
            if (sfx[3])
            {
                PlaySFX(sfx[3]);
            }
            Start();
        }
        if (other.gameObject.tag == "Front Goal")
        {
            Debug.Log("Player 1 scored a goal!");
            transform.position = startingPosition;
            if (sfx[3])
            {
                PlaySFX(sfx[3]);
            }
            Start();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player 1" || collision.gameObject.tag == "Player 2")
        {
            if (sfx[0])
            {
                PlaySFX(sfx[0]);
            }
        }
        if (collision.gameObject.tag == "Corner")
        {
            if (sfx[1])
            {
                PlaySFX(sfx[1]);
            }
        }
        if (collision.gameObject.tag == "Wall")
        {
            if (sfx[2])
            {
                PlaySFX(sfx[2]);
            }
        }
    }
}

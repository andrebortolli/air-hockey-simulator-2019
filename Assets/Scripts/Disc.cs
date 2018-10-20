using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disc : MonoBehaviour
{
    private GameController gameController;
    private Rigidbody rb;
    public float throwSpeed;
    private Vector3 startingPosition;
    private AudioSource audioSource;
    public AudioClip[] sfx;

    private void Awake()
    {
        Random.InitState((int)System.DateTime.UtcNow.Second);
        gameController = FindObjectOfType<GameController>();
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        startingPosition = transform.position;
    }

    void Throw(float speed)
    {
        Vector3 randomEulerAngle = new Vector3(Random.Range(60.0f, 120.0f), 0.0f, Random.Range(60.0f, 120.0f));
        //If value is 0, the disc will head towards player 1 (negative heading); Else the disc will remain with positive heading, going towards the player 2.
        if (Random.Range(0,2) == 0)
        {
            randomEulerAngle = randomEulerAngle * -1;
        }
        rb.velocity = Vector3.zero;
        rb.AddForce(randomEulerAngle * speed * 0.5f, ForceMode.Acceleration);
    }

    // Use this for initialization
    IEnumerator Start()
    {
        rb.isKinematic = true;
        yield return new WaitForSeconds(1.0f);
        rb.isKinematic = false;
        if (rb != null)
        {
            Throw(throwSpeed);
        }
        yield return null;
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
            //Debug.Log("Player 2 scored a goal!");
            gameController.players[1].AddPlayerScore(1);
            transform.position = startingPosition;
            if (sfx[3])
            {
                PlaySFX(sfx[3]);
            }
            StartCoroutine(Start());
        }
        if (other.gameObject.tag == "Front Goal")
        {
            //Debug.Log("Player 1 scored a goal!");
            gameController.players[0].AddPlayerScore(1);
            transform.position = startingPosition;
            if (sfx[3])
            {
                PlaySFX(sfx[3]);
            }
            StartCoroutine(Start());
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

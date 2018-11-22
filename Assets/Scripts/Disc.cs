using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disc : MonoBehaviour
{
    private GameController gameController;
    private ReplayController replayController;
    private Rigidbody rb;
    public float throwSpeed;
    private Vector3 startingPosition;
    private AudioSource audioSource;
    public AudioClip[] sfx;
    private bool isGoal = false;

    private void Awake()
    {
        Random.InitState((int)System.DateTime.UtcNow.Second);
        gameController = FindObjectOfType<GameController>();
        replayController = FindObjectOfType<ReplayController>();
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        startingPosition = transform.position;
    }

    void Throw(float speed)
    {
        Vector3 randomEulerAngle = new Vector3(Random.Range(60.0f, 120.0f), 0.0f, Random.Range(60.0f, 120.0f));
        //If value is 0, the disc will head towards player 1 (negative heading); Else the disc will remain with positive heading, going towards the player 2.
        if (Random.Range(0, 2) == 0)
        {
            randomEulerAngle = randomEulerAngle * -1;
        }
        rb.velocity = Vector3.zero;
        rb.AddForce(randomEulerAngle * speed * 0.5f, ForceMode.Acceleration);
    }

    IEnumerator StartDisc(bool record = false)
    {
        gameController.disablePausing = true;
        while (replayController.IsReplaying() == true)
        {
            yield return new WaitForSeconds(.5f);
            //Debug.Log("Waiting for IsReplaying to become false.");
        }
        while (gameController.IsPaused == true)
        {
            yield return new WaitForSeconds(.5f);
            //Debug.Log("Waiting for IsPaused to become false.");
        }
        isGoal = false;
        rb.isKinematic = true;
        transform.position = startingPosition;
        yield return new WaitForSeconds(1.0f);
        rb.isKinematic = false;
        if (rb != null)
        {
            rb.detectCollisions = true;
            replayController.SetRecordState(record);
            Throw(throwSpeed);
        }
        gameController.disablePausing = false;
        yield return null;
    }

    public void ResetDisc(bool record)
    {
        StopCoroutine(StartDisc());
        StartCoroutine(StartDisc(record));
    }

    // Use this for initialization
    void Start()
    {
        StartCoroutine(StartDisc());
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

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Apply Force Area Player 1")
        {
            gameController.players[0].aICanAddForce = true;
            Debug.Log("player1-true");
        }
        else
        {
            if (other.gameObject.tag != "Apply Force Area Player 2" && other.gameObject.tag != "Front Goal" && other.gameObject.tag != "Back Goal")
            {
                gameController.players[0].aICanAddForce = false;
                Debug.Log("player1-false");
            }
        }
        if (other.gameObject.tag == "Apply Force Area Player 2")
        {
            gameController.players[1].aICanAddForce = true;
            Debug.Log("player2-true");
        }
        else
        {
            if (other.gameObject.tag != "Apply Force Area Player 1" && other.gameObject.tag != "Front Goal" && other.gameObject.tag != "Back Goal")
            {
                gameController.players[1].aICanAddForce = false;
                Debug.Log("player2-false");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isGoal && replayController.IsReplaying() == false)
        {
            if (other.gameObject.tag == "Back Goal")
            {
                //Debug.Log(other.gameObject.GetInstanceID());
                isGoal = true;
                //Debug.Log("Player 2 scored a goal!");
                gameController.players[1].AddPlayerScore(1);
                rb.detectCollisions = false;
                //transform.position = startingPosition;
                if (sfx[3])
                {
                    PlaySFX(sfx[3]);
                }
                replayController.PlayReplay(240);
                StopCoroutine(StartDisc());
                if (gameController.GameMode != "demo")
                {
                    StartCoroutine(StartDisc(true));
                }
                else
                {
                    StartCoroutine(StartDisc());
                }
            }
            if (other.gameObject.tag == "Front Goal")
            {
                //Debug.Log(other.gameObject.GetInstanceID());
                isGoal = true;
                //Debug.Log("Player 1 scored a goal!");
                gameController.players[0].AddPlayerScore(1);
                rb.detectCollisions = false;
                //transform.position = startingPosition;
                if (sfx[3])
                {
                    PlaySFX(sfx[3]);
                }
                replayController.PlayReplay(240);
                StopCoroutine(StartDisc());
                if (gameController.GameMode != "demo")
                {
                    StartCoroutine(StartDisc(true));
                }
                else
                {
                    StartCoroutine(StartDisc());
                }
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player 1" || collision.gameObject.tag == "Player 2")
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

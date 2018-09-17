using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Player;

public class PlayerController : MonoBehaviour
{
    private Player player;
    public bool aI;
    private bool aILast;
    [Range(0.0f, 1.0f)]
    public float aiResponse;
    public Transform target;
    private Rigidbody rb;
    public bool inverseControls;
    public string[] movementAxes = new string[2];
    public string triggerAxis;
    public Vector2 movementAxesValues;
    public float triggerAxisMultiplier;
    public float speed;
    public float playerSpeedMultiplier = 1000.0f;

    string UpdatePlayerType()
    {
        string type;
        if (aI == true)
        {
            player = new Player(speed);
            type = "AI";
        }
        else
        {
            player = new Player(speed, false, movementAxes[0], movementAxes[1]);
            type = "Player";
        }
        aILast = aI;
        return string.Format("Player Type changed from to {0}, with the following axes: {1}", type, movementAxes[0] + " " + movementAxes[1]);
    }

    private void Awake()
    {
        UpdatePlayerType();
        rb = GetComponent<Rigidbody>();
    }
    // Use this for initialization
    void Start ()
    {
		
	}
	void FixedUpdate ()
    {
        if (aI != aILast)
        {
            Debug.Log(UpdatePlayerType());
        }

        if (aI)
        {
            Vector3 desiredPosition = new Vector3(target.position.x, rb.position.y, target.position.z);
            Vector3 newPosition = Vector3.Lerp(transform.position, desiredPosition, aiResponse);
            rb.velocity = (newPosition - transform.position) * speed;
        }
        else
        {
            triggerAxisMultiplier = Mathf.Clamp(Input.GetAxis(triggerAxis), 0.2f, 1.0f);
            if (inverseControls)
            {
                movementAxesValues = new Vector2(Input.GetAxis(player.MovementAxisNameX), Input.GetAxis(player.MovementAxisNameY));
                rb.velocity = new Vector3(movementAxesValues.x * -speed * playerSpeedMultiplier * triggerAxisMultiplier, 0.0f, movementAxesValues.y * -speed * playerSpeedMultiplier * triggerAxisMultiplier) * Time.fixedDeltaTime;
            }
            else
            {
                movementAxesValues = new Vector2(Input.GetAxis(player.MovementAxisNameX), Input.GetAxis(player.MovementAxisNameY));
                rb.velocity = new Vector3(movementAxesValues.x * speed * playerSpeedMultiplier * triggerAxisMultiplier, 0.0f, movementAxesValues.y * speed * playerSpeedMultiplier * triggerAxisMultiplier) * Time.fixedDeltaTime;
            }
        }
	}

    private void Update()
    {
       
    }
}

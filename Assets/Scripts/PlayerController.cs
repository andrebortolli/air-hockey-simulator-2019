using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Player; //Imports player class.

public class PlayerController : MonoBehaviour
{
    private Player player; //Define the class variable.
    public bool aI; //Toggle switch between AI and Player.
    private bool aILast; //Toggle switch helper.
    [Range(0.0f, 1.0f)] //AI response range.
    public float aiResponse; //AI response slider.
    public Transform target; //AI target to follow.
    private Rigidbody rb; //Player Rigidbody component.
    public bool inverseControls; //Toggle player inverse movement.
    public string[] movementAxes = new string[2]; //Player movement axes names.
    public string triggerAxis; //Player trigger axis name.
    public Vector2 movementAxesValues; //Player movement axes values.
    public float triggerAxisMultiplier; //Player trigger axis multiplier.
    public float speed; //Player speed.
    public float playerSpeedMultiplier = 1000.0f; //Player speed multiplier. NOTE: Use this value to balance between AI and Player movement speeds, since the AI is always faster by unit.

    //Instantiates players.
    string UpdatePlayerType()
    {
        string type; //Player type. AI or Player.
        if (aI == true) //If player type is AI, create a new AI player.
        {
            player = new Player(speed); //Define player variable.
            type = "AI"; //Set type to AI. (Report use only).
        }
        else //If player is not AI, create a new player, using the current script attributes.
        {
            player = new Player(speed, false, movementAxes[0], movementAxes[1]); //Define player variable.
            type = "Player"; //Set type to Player. (Report use only).
        }
        aILast = aI; //Sets the toggle helper
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

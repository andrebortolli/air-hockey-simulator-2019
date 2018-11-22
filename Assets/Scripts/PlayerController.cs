using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Player; //Imports player class.

public class PlayerController : MonoBehaviour
{
    private Player player; //Define the class variable.
    public GameObject goal; //Own goal.
    public GameObject opponentGoal; //Opponent Player's goal.
    private Vector3 startPosition; //Player start position. Used in the return to function in AI movement code.
    public bool aI; //Toggle switch between AI and Player.
    private bool aILast; //Toggle switch helper.
    [Range(0.0f, 1.0f)] //AI response range.
    public float aiResponse; //AI response slider.
    public Transform target; //AI target to follow.
    private Rigidbody rb; //Player Rigidbody component.
    public bool inverseControls; //Toggle player inverse movement.
    public string[] movementAxes = new string[2]; //Player movement axes names.
    public string triggerAxis; //Player trigger axis name.
    private Vector2 movementAxesValues; //Player movement axes values.
    public float triggerAxisMultiplier; //Player trigger axis multiplier.
    public float speed; //Player speed.
    private bool returnToStartingPosition;
    public bool ReturnToStartingPosition
    {
        get
        {
            return returnToStartingPosition;
        }
        set
        {
            returnToStartingPosition = value;
        }
    }

    public void ResetPlayerScore()
    {
        player.Score = 0;
    }

    public int GetPlayerScore()
    {
        return player.Score;
    }

    public void AddPlayerScore(int scoreToAdd)
    {
        player.Score += scoreToAdd;
        //Debug.Log(player.Score);
    }

    public void ChangePlayerMovementAxes(string x, string y, string trigger, bool invert = false)
    {
        if (invert)
        {
            inverseControls = true;
        }
        else
        {
            inverseControls = false;
        }
        player.MovementAxisNameX = movementAxes[0] = x;
        player.MovementAxisNameY = movementAxes[1] = y;
        triggerAxis = trigger;
    }

    //Instantiates players.
    public string UpdatePlayerType()
    {
        string type; //Player type. AI or Player.
        //If player type is AI, create a new AI player.
        if (aI == true) 
        {
            player = new Player(speed); //Define player variable.
            transform.position = startPosition;
            type = "AI"; //Set type to AI. (Report use only).
        }
        else //If player is not AI, create a new player, using the current script attributes.
        {
            player = new Player(speed, false, movementAxes[0], movementAxes[1]); //Define player variable.
            transform.position = startPosition;
            type = "Player"; //Set type to Player. (Report use only).
        }
        aILast = aI; //Sets the toggle helper
        //print(string.Format("Player Type changed from to {0}, with the following axes: {1}", type, movementAxes[0] + " " + movementAxes[1]));
        return string.Format("Player Type changed from to {0}, with the following axes: {1}", type, movementAxes[0] + " " + movementAxes[1]); //Report to console.
    }

    private void Awake()
    {    
        rb = GetComponent<Rigidbody>(); //Get and set the GameObject's Rigidbody on Awake.
        startPosition = transform.position;
        //UpdatePlayerType(); //Set player type on Awake.
    }

	void FixedUpdate ()
    {
        //Detect change in Player type. Needs to be before everything, otherwise it may desync, giving console errors.
        if (aI != aILast)
        {
            UpdatePlayerType();
            //Debug.Log(UpdatePlayerType()); //Update player type and print the method's return on the console.
        }
        //If the player type is AI, use AI movement code.
        if (aI)
        {
            triggerAxisMultiplier = Mathf.Clamp(Mathf.Abs(Input.GetAxis(triggerAxis)), 0.2f, 1.0f); //Clamps the trigger speed. Lower value cannot be 0.0, otherwise the player wont move. Max should be 1.0, since it would be a neutral variable in the following code.
            if (returnToStartingPosition || Vector3.Distance(transform.position, target.transform.position) >= 7.5) 
            {
                Vector3 lerpToStartPosition = Vector3.Lerp(transform.position, startPosition, aiResponse * Random.Range(0.25f, 1.0f));
                rb.velocity = (lerpToStartPosition - transform.position) * speed * triggerAxisMultiplier * Time.fixedDeltaTime;
            }
            else
            {
                Vector3 desiredPosition = new Vector3(target.position.x, rb.position.y, target.position.z); //OLD
                Vector3 newPosition = Vector3.Lerp(transform.position, desiredPosition, aiResponse); //OLD
                rb.velocity = (newPosition - transform.position) * speed * triggerAxisMultiplier /*placeholder*/ * Time.fixedDeltaTime;
            }
        }
        else //If the player type is not AI, use player movement code.
        {
            triggerAxisMultiplier = Mathf.Clamp(Mathf.Abs(Input.GetAxis(triggerAxis)), 0.2f, 1.0f); //Clamps the trigger speed. Lower value cannot be 0.0, otherwise the player wont move. Max should be 1.0, since it would be a neutral variable in the following code.
            //Check if inverseControls are true, if yes use inverted controls.
            if (inverseControls)
            {
                movementAxesValues = new Vector2(Input.GetAxis(player.MovementAxisNameX), Input.GetAxis(player.MovementAxisNameY));
                rb.velocity = new Vector3(movementAxesValues.x * -speed * triggerAxisMultiplier, 0.0f, movementAxesValues.y * -speed * triggerAxisMultiplier) * Time.fixedDeltaTime;
            }
            else
            {
                movementAxesValues = new Vector2(Input.GetAxis(player.MovementAxisNameX), Input.GetAxis(player.MovementAxisNameY));
                rb.velocity = new Vector3(movementAxesValues.x * speed * triggerAxisMultiplier, 0.0f, movementAxesValues.y * speed * triggerAxisMultiplier) * Time.fixedDeltaTime;
            }
        }
	}
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Disc" && aI)
        {
            triggerAxisMultiplier = 1.0f;
            target.parent.parent.GetComponent<Rigidbody>().AddForce((opponentGoal.transform.position - target.transform.position) * Random.Range(25.0f, 75.0f) * aiResponse, ForceMode.Acceleration); //Shoot to goal.
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Disc" && aI)
        {
            triggerAxisMultiplier = 0.2f;
        }
    }
}
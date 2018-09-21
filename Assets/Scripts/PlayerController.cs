using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Player; //Imports player class.

public class PlayerController : MonoBehaviour
{
    private Player player; //Define the class variable.
    public bool aI; //Toggle switch between AI and Player.
    private bool aILast; //Toggle switch helper.
    [Range(0.0f, 0.15f)] //AI response range.
    public float aiResponse; //AI response slider.
    public Transform target; //AI target to follow.
    private Rigidbody rb; //Player Rigidbody component.
    public bool inverseControls; //Toggle player inverse movement.
    public string[] movementAxes = new string[2]; //Player movement axes names.
    public string triggerAxis; //Player trigger axis name.
    public Vector2 movementAxesValues; //Player movement axes values.
    public float triggerAxisMultiplier; //Player trigger axis multiplier.
    public float speed; //Player speed.
    public float playerSpeedMultiplier = 1000.0f; //Player speed multiplier. Used to make the speed variable lower.

    Vector3 predictGameObjectPosition(GameObject gameObject) //WIP
    {
        Vector3 predictedPosition;
        Vector3 targetPosition = gameObject.transform.position;
        Vector3 targetVelocity = gameObject.GetComponent<Rigidbody>().velocity;
        //Vector3 heading = gameObject.transform.position - this.transform.position;
        //float distance = heading.magnitude;
        predictedPosition = targetPosition + targetVelocity;
        Debug.DrawLine(gameObject.transform.position, predictedPosition, Color.red);
        Debug.Log("Predicted Position: " + predictedPosition.ToString() + "Current Position: " + targetPosition.ToString());
        return predictedPosition;
    }
        
    //Instantiates players.
    string UpdatePlayerType()
    {
        string type; //Player type. AI or Player.
        //If player type is AI, create a new AI player.
        if (aI == true) 
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
        return string.Format("Player Type changed from to {0}, with the following axes: {1}", type, movementAxes[0] + " " + movementAxes[1]); //Report to console.
    }

    private void Awake()
    {
        UpdatePlayerType(); //Set player type on Awake.
        rb = GetComponent<Rigidbody>(); //Get and set the GameObject's Rigidbody on Awake.
    }

	void FixedUpdate ()
    {
        //Detect change in Player type. Needs to be before everything, otherwise it may desync, giving console errors.
        if (aI != aILast)
        {
            Debug.Log(UpdatePlayerType()); //Update player type and print the method's return on the console.
        }
        //If the player type is AI, use AI movement code.
        if (aI)
        {
            Vector3 predictedPosition = predictGameObjectPosition(target.gameObject);
            Vector3 newPosition = Vector3.Lerp(transform.position, predictedPosition, aiResponse);
            //Vector3 desiredPosition = new Vector3(target.position.x, rb.position.y, target.position.z); //OLD
            //Vector3 newPosition = Vector3.Lerp(transform.position, desiredPosition, aiResponse); //OLD
            rb.velocity = (newPosition - transform.position) * speed * playerSpeedMultiplier * triggerAxisMultiplier /*placeholder*/ * Time.fixedDeltaTime;
        }
        else //If the player type is not AI, use player movement code.
        {
            triggerAxisMultiplier = Mathf.Clamp(Input.GetAxis(triggerAxis), 0.2f, 1.0f); //Clamps the trigger speed. Lower value cannot be 0.0, otherwise the player wont move. Max should be 1.0, since it would be a neutral variable in the following code.
            //Check if inverseControls are true, if yes use inverted controls.
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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Disc" && aI)
        {
            triggerAxisMultiplier =Random.Range(0.5f,1.0f);
            rb.AddRelativeForce(0.0f, 0.0f, -50.0f);
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

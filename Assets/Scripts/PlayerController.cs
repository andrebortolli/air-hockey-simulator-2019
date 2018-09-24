using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Player; //Imports player class.

public class PlayerController : MonoBehaviour
{
    private GameController gameController;
    private Player player; //Define the class variable.
    public GameObject goal;
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
    //public float playerSpeedMultiplier = 1000.0f; //Player speed multiplier. Used to make the speed variable lower.
    private bool returnToGoal;
    public bool ReturnToGoal
    {
        get
        {
            return returnToGoal;
        }
        set
        {
            returnToGoal = value;
        }
    }

    void RaycastTo(GameObject origin, GameObject direction)
    {
        RaycastHit hit;
        if (Physics.Raycast(origin.transform.position, direction.transform.position, out hit, 30f, 13))
        {
           //Debug.Log(string.Format("[{4}] The Raycast hit the object at the following position: {0}. The distance between the origin {1} and the direction {2} is: {3} units.", hit.point, origin.gameObject.name, direction.gameObject.name, hit.distance, this.gameObject.tag));
            Debug.DrawRay(origin.transform.position, direction.transform.position, Color.green);
        }
    }

    Vector3 PredictGameObjectPosition(GameObject gameObject) //WIP
    {
        Vector3 predictedPosition;
        Vector3 targetPosition = gameObject.transform.position;
        Vector3 targetVelocity = gameObject.GetComponent<Rigidbody>().velocity.normalized;
        //Vector3 heading = gameObject.transform.position - this.transform.position;
        //float distance = heading.magnitude;
        predictedPosition = targetPosition + targetVelocity;
        Debug.DrawLine(gameObject.transform.position, predictedPosition, Color.red);
        //Debug.Log("Predicted Position: " + predictedPosition.ToString() + " Current Position: " + targetPosition.ToString());
        return predictedPosition;
    }

    Vector3 GetPositionByRayCast(GameObject origin, GameObject direction)
    {
        RaycastHit hit;
        if (Physics.Raycast(origin.transform.position, direction.transform.position, out hit))
        {
            //Debug.Log(string.Format("[{4}] The Raycast hit the object at the following position: {0}. The distance between the origin {1} and the direction {2} is: {3} units.", hit.point, origin.gameObject.name, direction.gameObject.name, hit.distance, this.gameObject.tag));
            Debug.DrawRay(origin.transform.position, direction.transform.position, Color.red);
            return hit.point;
        }
        else
        {
            return Vector3.zero;
        }
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
        //gameController.FindPlayers();
        return string.Format("Player Type changed from to {0}, with the following axes: {1}", type, movementAxes[0] + " " + movementAxes[1]); //Report to console.
    }

    private void Awake()
    {
        UpdatePlayerType(); //Set player type on Awake.
        rb = GetComponent<Rigidbody>(); //Get and set the GameObject's Rigidbody on Awake.
        gameController = FindObjectOfType<GameController>();
    }

	void FixedUpdate ()
    {
        //RaycastTo(target.gameObject, goal.gameObject);
        //Detect change in Player type. Needs to be before everything, otherwise it may desync, giving console errors.
        if (aI != aILast)
        {
            Debug.Log(UpdatePlayerType()); //Update player type and print the method's return on the console.
        }
        //If the player type is AI, use AI movement code.
        if (aI)
        {
            triggerAxisMultiplier = Mathf.Clamp(Input.GetAxis(triggerAxis), 0.2f, 1.0f); //Clamps the trigger speed. Lower value cannot be 0.0, otherwise the player wont move. Max should be 1.0, since it would be a neutral variable in the following code.
            //Vector3 predictedPosition = GetPositionByRayCast(this.gameObject, target.gameObject);
            //Vector3 predictedPosition = PredictGameObjectPosition(target.gameObject);
            //Vector3 newPosition = Vector3.Lerp(transform.position, predictedPosition, aiResponse);
            if (returnToGoal)
            {
                Vector3 goalPosition = new Vector3(goal.transform.position.x, rb.position.y, goal.transform.position.z); //OLD
                Vector3 lerpToGoalPosition = Vector3.Lerp(transform.position, goalPosition, aiResponse * 0.25f); //OLD
                rb.velocity = (lerpToGoalPosition - transform.position) * speed * triggerAxisMultiplier * Time.fixedDeltaTime;
                //Debug.Log("I'm trying to go to: " + goal.name);
            }
            else
            {
                Vector3 desiredPosition = new Vector3(target.position.x, rb.position.y, target.position.z); //OLD
                Vector3 newPosition = Vector3.Lerp(transform.position, desiredPosition, aiResponse); //OLD
                rb.velocity = (newPosition - transform.position) * speed * triggerAxisMultiplier /*placeholder*/ * Time.fixedDeltaTime;
            }
            //rb.velocity = Vector3.Lerp(transform.position, new Vector3(target.transform.position.x, transform.position.y, transform.position.z), aiResponse);
        }
        else //If the player type is not AI, use player movement code.
        {
            triggerAxisMultiplier = Mathf.Clamp(Input.GetAxis(triggerAxis), 0.2f, 1.0f); //Clamps the trigger speed. Lower value cannot be 0.0, otherwise the player wont move. Max should be 1.0, since it would be a neutral variable in the following code.
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
            target.parent.parent.GetComponent<Rigidbody>().AddForce(Vector3.forward * -target.parent.transform.eulerAngles.z * speed * Random.Range(420f,42000f), ForceMode.Acceleration);
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

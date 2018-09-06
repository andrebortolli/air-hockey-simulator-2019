using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Player;

public class PlayerController : MonoBehaviour
{
    private Player player;
    public bool aI;
    [Range(0.0f, 1.0f)]
    public float aiResponse;
    public Transform target;
    private Rigidbody rb;
    public string[] movementAxes = new string[2];
    public Vector2 movementAxesValues;
    public float speed;
    private void Awake()
    {
        if (aI)
        {
            player = new Player(speed);
        }
        else
        {
            player = new Player(speed, false, movementAxes[0], movementAxes[1]);
        }
        rb = GetComponent<Rigidbody>();
    }
    // Use this for initialization
    void Start ()
    {
		
	}
	void FixedUpdate ()
    {
		if (aI)
        {
            Vector3 desiredPosition = new Vector3(target.position.x, rb.position.y, target.position.z);
            Vector3 newPosition = Vector3.Lerp(transform.position, desiredPosition, aiResponse);
            rb.velocity = (newPosition - transform.position) * speed;
        }
        else
        {
            movementAxesValues = new Vector2(Input.GetAxis(player.MovementAxisNameX), Input.GetAxis(player.MovementAxisNameY));
            rb.velocity = new Vector3(movementAxesValues.x * speed, 0.0f, movementAxesValues.y * speed) * Time.fixedDeltaTime;
        }
	}
}

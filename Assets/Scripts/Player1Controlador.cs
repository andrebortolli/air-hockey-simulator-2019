using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Controlador : MonoBehaviour
{
    private Rigidbody rb;
    public float multiplicadorDeVelocidade = 35f;
    public Vector2 controleXY;
	// Use this for initialization
	void Awake ()
    {
        rb = GetComponent<Rigidbody>();	
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        controleXY = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
        rb.AddForce(-controleXY.x * multiplicadorDeVelocidade, 0.0f, controleXY.y * multiplicadorDeVelocidade);
        //transform.Translate(-controleXY.x * Time.deltaTime * 2.5f, 0.0f, controleXY.y * Time.deltaTime * 2.5f);
	}
}

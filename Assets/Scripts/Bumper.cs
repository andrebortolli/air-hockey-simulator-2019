using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Disc")
        {
            collision.gameObject.GetComponent<Rigidbody>().AddForce(collision.gameObject.GetComponent < Rigidbody>().velocity * Random.Range(25f, 50f), ForceMode.Acceleration);
        }
    }
}

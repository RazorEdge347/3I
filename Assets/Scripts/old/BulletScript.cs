using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

	private Transform Planet;
	private Rigidbody rb;
	private float lifetime = 1.5f;
	// Use this for initialization
	void Start () {
		Planet = GameObject.FindGameObjectWithTag ("Planet").transform;
		rb = GetComponent<Rigidbody> ();
		rb.AddForce (transform.forward*1000);
	}

	void Update(){
		lifetime -= 1 * Time.deltaTime;

		if(lifetime <= 0){
			Destroy(gameObject);
		}
	}

	// Update is called once per frame
	void FixedUpdate () {

		Vector3 GravityUp = (Planet.position - transform.position).normalized;

	}
}

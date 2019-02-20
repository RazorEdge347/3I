using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class feetScript : MonoBehaviour {

	public GameObject SoundBubble;
	public GameObject PL;

	private bool fallHit = true;
	private PLController PLc;
	private float spawnspeedRate = 1;
	float dropVelocity;
	//private int btype = 0;
	// Use this for initialization
	void Start () {
		PLc = PL.GetComponent<PLController> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (PLc.isGrounded && !PLc.isGrabbingLedge) {
			if ((Input.GetAxis ("Vertical") != 0 || Input.GetAxis ("Horizontal") != 0) && !PLc.isCrouching) {
				if (!PLc.isWalking) {
					spawnspeedRate -= 1 * Time.deltaTime;
					//btype = 0;
				} else {
					spawnspeedRate -= 1 * Time.deltaTime;
					//btype = 1;
				}

				if (PLc.isSprinting) {
					spawnspeedRate -= 1 * Time.deltaTime;
					//btype = 2;
				} 

				if (spawnspeedRate <= 0) {
					spawnspeedRate = 1;
					Instantiate (SoundBubble, transform.position, transform.rotation).GetComponent<soundbubbleScript> ().BBType (PLc.movementSpeed);
				}
			}

			if (fallHit && !Input.GetKey (KeyCode.LeftControl)) {
				Instantiate (SoundBubble, transform.position, transform.rotation).GetComponent<soundbubbleScript> ().BBType (dropVelocity);
				//btype = 4;
			}

			fallHit = false;

		} else {
			dropVelocity = PLc.GetComponent<Rigidbody> ().velocity.magnitude;
			fallHit = true;
		} 
		//Debug.Log ("fallHit: " + fallHit);
	}
}

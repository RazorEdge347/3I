using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grappleGun : MonoBehaviour {

	//public GameObject Ammo;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.rotation = Camera.main.gameObject.transform.rotation;


		if(Input.GetKeyDown(KeyCode.C)){
			transform.localPosition = new Vector3 (transform.localPosition.x * -1f, transform.localPosition.y, transform.localPosition.z); //(Vector3.Lerp(transform.localPosition, ,Time);
		}

		/*if(Input.GetMouseButton(1) && Input.GetMouseButtonDown(0))
			Instantiate (Ammo, transform.position + transform.forward, transform.rotation).GetComponent<Rigidbody> ().AddForce (transform.forward * 200);*/
	}
}

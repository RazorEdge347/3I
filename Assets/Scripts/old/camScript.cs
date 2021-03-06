using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camScript : MonoBehaviour {

	public GameObject PLHead;
	public GameObject Planet;
	[Range(0.0f, 10.0f)] public float CamSpeed;
	[Range(-15.0f, 15.0f)] public float distBACK;
	[Range(-15.0f, 15.0f)] public float distLR;
	[Range(0.0f, 15.0f)] public float distUP;
	public float PlanetarymodeHight = 100.0f;

	public float speed = 5f;
	public float rotSpeed = 0.5f;

	private Vector3 Reloffset;
	private bool Start = false;
	Vector3 gravityUp;
	Vector3 PlanetaryMode;

	void setCursorState(){
		Cursor.lockState = Input.GetKey (KeyCode.LeftAlt) ? CursorLockMode.None : CursorLockMode.Locked;
	}

	// Use this for initialization
	void Update () {
		setCursorState ();
	}



	// Update is called once per frame
	void FixedUpdate () {
		Vector3 TargetMove = PLHead.transform.position + PLHead.transform.rotation * (new Vector3 (distLR, distUP, -distBACK));

		if (!Input.GetKey (KeyCode.Q)) {
			transform.LookAt (PLHead.transform);
			if (!Start) {
				transform.position = TargetMove; //Vector3.Lerp(transform.position, TargetMove, Time.deltaTime*CamSpeed);
				transform.rotation = new Quaternion (-PLHead.transform.rotation.x, -PLHead.transform.rotation.y, -PLHead.transform.rotation.z, -PLHead.transform.rotation.w);
			} else {
				/*transform.position = Vector3.Lerp (transform.position, TargetMove, Time.fixedDeltaTime * CamSpeed / 3.0f);
				transform.rotation = Quaternion.Lerp (transform.rotation, PLHead.transform.rotation, Time.fixedDeltaTime);*/
				transform.position = Vector3.Lerp (transform.position, TargetMove, Time.fixedDeltaTime * CamSpeed / 3.0f);
				//Debug.Log ((PLHead.transform.position - transform.position).magnitude);
				if ((PLHead.transform.position - transform.position).magnitude <= 5) {
					Start = false;
				}
			}
		}
		/*
		Vector3 TargetMove = PLHead.transform.position + PLHead.transform.rotation * (new Vector3 (distLR, distUP, -distBACK));
		if (!Input.GetKey (KeyCode.Q)) { // NOT PLANETARY MODE ?
			if ((PLHead.transform.position - transform.position).magnitude < 10) {
				transform.position = TargetMove; //Vector3.Lerp(transform.position, TargetMove, Time.deltaTime*CamSpeed);
				transform.LookAt (PLHead.transform.position);
				transform.rotation = new Quaternion (-PLHead.transform.rotation.x, -PLHead.transform.rotation.y, -PLHead.transform.rotation.z, -PLHead.transform.rotation.w);
			} else {
				transform.position = Vector3.Lerp(transform.position, TargetMove, Time.deltaTime*CamSpeed/2.0f);
				transform.LookAt (Planet.transform.position);
				}
		} else { // PLANETARY MODE 
			Vector3 gravityUp = -(Planet.transform.position - transform.position).normalized;
			//Vector3 UPvect = -(Planet.transform.position - PLHead.transform.position).normalized;
			Vector3 PlanetaryMode = new Vector3 (gravityUp.x, gravityUp.y, gravityUp.z)*PlanetarymodeHight;		
			if (Mathf.Abs(transform.position.y) < Mathf.Abs(PlanetaryMode.y - 1.0f)) {
				transform.position = Vector3.Lerp (transform.position, PlanetaryMode, Time.deltaTime * CamSpeed / 2.0f);
			}
			transform.LookAt (Planet.transform.position);


		}
	}*/
	}
}

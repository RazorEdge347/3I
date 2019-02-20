using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySightScript : MonoBehaviour {

	private Vector3 LastPlayerSighting;
	public bool PlayerInSight;
	public float FOV = 25;
	public LayerMask IgnoreXlayer;
	public GameObject PL;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 SightDir = PL.transform.position - transform.position;
		float ViewAngle = Vector3.Angle (SightDir, transform.forward);
		//Debug.DrawRay (transform.position, SightDir, Color.green,15);
		if (ViewAngle <= FOV) {
			RaycastHit Hit;
			if (Physics.Raycast (transform.position, SightDir.normalized, out Hit,25.0f,~IgnoreXlayer)) {
				if (Hit.collider.gameObject == PL) {
					Debug.DrawRay (transform.position, SightDir, Color.red,25);
					PlayerInSight = true;
					LastPlayerSighting = PL.transform.position;
				} else {
					PlayerInSight = false;
				}
			}
		}

	}
}

using UnityEngine;
using System.Collections;

public class followerScript : MonoBehaviour {
	public Transform Target;
	public float distance = 3f;
	public float speed = 3f;

	private Vector3 Targpos;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.position = Vector3.Slerp (transform.position, Target.position, (Target.position - transform.position ).magnitude/1 * Time.fixedDeltaTime);
	}
}

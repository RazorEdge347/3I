using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseMenu : MonoBehaviour {

	private bool ispaused = false;
	float normalTime;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			ispaused = !ispaused;
			GetComponent<Canvas> ().enabled = !GetComponent<Canvas> ().enabled;
		}

		if (ispaused) {
			Time.timeScale = 0;
		} else {
			Time.timeScale = 1;
		}
	}
}

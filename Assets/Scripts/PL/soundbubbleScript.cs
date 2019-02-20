using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundbubbleScript : MonoBehaviour {

	private float inflateRate = 2f;
	private Vector3 maxSize;


	// Use this for initialization
	void Start () {
		//maxSize = Vector3.one;
		GetComponent<MeshRenderer> ().enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		transform.localScale = Vector3.Lerp (transform.localScale, maxSize, Time.deltaTime * inflateRate);

		if (transform.localScale.x > maxSize.x -.5f) {
			Destroy (gameObject);
		}
		//GetComponent<Material> ().SetColor(
	}

	public void BBType(float speed){
		maxSize = Vector3.one * speed;
	}

	/*public void BBType(int t){
		switch (t) {
		case 0:
			inflateRate = 2f;
			maxSize = new Vector3(12,12,12);
			break;
		case 1:
			inflateRate = 2f;
			maxSize = new Vector3(7,7,7);
			break;
		case 2:
			inflateRate = 2f;
			maxSize = new Vector3(15,15,15);
			break;
		case 3:
			inflateRate = 2f;
			maxSize = new Vector3(4,4,4);
			break;
		case 4:
			inflateRate = 2f;
			maxSize = new Vector3(15,15,15);
			break;
		}
	}*/
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundDetectionEnemy : MonoBehaviour {

	public GameObject InvestigatePoint;
	public List<GameObject> SoundMemory = new List<GameObject>(3);
	public GameObject InvP;
	// Use this for initialization
	void Start () {
		GetComponent<MeshRenderer> ().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Sound") {

			InvestigatePoint = Instantiate (InvP, other.transform.position, transform.rotation);
			SoundMemory.Insert(0,InvestigatePoint);

			//Instantiate (InvP,InvestigatePoint, transform.rotation).transform.position
			//SoundMemory.Insert(0,other.transform.position);
		}
	}

	public void clearAudioMemoryPoint(){
		InvestigatePoint = null;
		for (int i = SoundMemory.Count-1; i >= 0; i--) {
			Destroy(SoundMemory [i].gameObject);
			SoundMemory.RemoveAt (i);
		}

	}
}

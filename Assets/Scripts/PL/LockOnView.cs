using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnView : MonoBehaviour {

	private GameObject EnemyTarget;

	private GameObject[] Enemies;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public GameObject getEnemyTarget (){
		return EnemyTarget;
	}

	public void clearEnemyTarget (){
		EnemyTarget = null;
	}

	void OnTriggerStay(Collider other){
		/*int IndexEnemy = 0;
		foreach (GameObject enemy in this.GetComponent<Collider>()) {
			Enemies[IndexEnemy] = enemies.gameObject;
			IndexEnemy++;
		}*/
		if (other.gameObject.tag == "Enemy") {
			//Debug.Log("ENEMIEEE!");
			EnemyTarget = other.gameObject;
		}else{
		}
	}
}

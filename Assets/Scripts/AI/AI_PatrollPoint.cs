using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_PatrollPoint : MonoBehaviour {

	public int Index = 0;
	static int maxindex = 0;
	private float fn;
	private float gn;
	private float hn;
	public GameObject previousPoint;

	public List<GameObject> connections;
	// Use this for initialization
	void Start() {
		fn = Mathf.Infinity;
		gn = Mathf.Infinity;
		//Debug.Log ("Max Index: " + maxindex);
		maxindex++;
		GetComponent<MeshRenderer> ().enabled = true;
	}

	public GameObject[] getConnections(){

		RaycastHit connRay;
		GameObject[] connectionsAux1 = GameObject.FindGameObjectsWithTag("PatrollPoint");
		connections = new List<GameObject>(connectionsAux1.Length);
		for (int connindex = 0; connindex < connectionsAux1.Length; connindex++) {
			if(Physics.Raycast(transform.position,(connectionsAux1[connindex].transform.position - transform.position).normalized,out connRay) 
				&& connRay.collider.gameObject == connectionsAux1[connindex]){
				connections.Add(connectionsAux1 [connindex]);

			}
		}

		connections.Sort (sortByIndex);

		//Debug.Log ("SIZE OF DIZ: " + connections.Count);

		return connections.ToArray();
	}

	int sortByIndex(GameObject a, GameObject b){
		return a.GetComponent<AI_PatrollPoint> ().getIndex().CompareTo(b.GetComponent<AI_PatrollPoint> ().getIndex());
	}

	public float getGn(){
		return gn;
	}

	public void setGn(float val){
		gn = val;
	}
		
	public float calculate_fn(GameObject Goal){
		hn = (Goal.transform.position - transform.position).magnitude;
		fn =  gn + hn;
		return fn;
	}

	public void setFn(float val){
		fn = val;
	}

	public int getIndex(){
		return Index;
	}

	public void setIndex(int i){
		Index = i;
	}

	public void setPrevious(GameObject previous){
		previousPoint = previous;
	}

	public GameObject getPrevious(){
		return previousPoint;
	}

	// Update is called once per frame
	void Update () {
		//Update Dynamic Connections



		/*GameObject[] connectionsAux1 = GameObject.FindGameObjectsWithTag("PatrollPoint");
		GameObject[] connectionsAux2;
		connectionsAux2 = new GameObject[connectionsAux1.Length];
		for (int connindex = 0; connindex < connectionsAux1.Length; connindex++) {
			if(Physics.Raycast(transform.position,(connectionsAux1[connindex].transform.position - transform.position).normalized,out connRay) 
				&& connRay.collider.gameObject == connectionsAux1[connindex]){
				connectionsAux2 [connindex] = connectionsAux1 [connindex];
				//Debug.DrawRay (transform.position, connectionsAux1 [connindex].transform.position - transform.position, Color.yellow);
			}
			//Debug.DrawRay (transform.position, connectionsAux [connindex].transform.position - transform.position, Color.green);
		}
		int Connsize = 0;
		for (int i = 0; i < connectionsAux2.Length; i++) {
			
			if (connectionsAux2 [i] != null) {
				Connsize++;
			}
		}
		connections = new GameObject[Connsize];
		Connsize = 0;
		for (int i = 0; i < connectionsAux2.Length; i++) {

			if (connectionsAux2 [i] != null) {
				connections [Connsize] = connectionsAux2 [i];
				Connsize++;
			}
		}*/
	}

	//
}

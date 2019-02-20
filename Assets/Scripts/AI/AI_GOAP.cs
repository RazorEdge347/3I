using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_GOAP : MonoBehaviour {

	public List<GameObject> PatrollList;
	public GameObject Connector;
	public GameObject PatrollAux;
	public int [] PatrollOrder; //2-3-4

	private GameObject CameFrom;
	private Animator FSM;
	private int PatrollNum;
	private Vector3 NExtPatrollPath;
	private Vector3 NExtInvestigatePath;

	// Use this for initialization

	void Start () {
		FSM = GetComponent<Animator> ();
		_SightScript = GetComponent<EnemySightScript>();


		GameObject[] aux = GameObject.FindGameObjectsWithTag ("PatrollPoint");

		for (int i = 0; i < aux.Length; i ++){
			PatrollList.Add(aux[i]);
		}
		/*PatrollList.Reverse ();

		PatrollList.Reverse ();*/

	}

	int sortByIndex(GameObject a, GameObject b){
		return a.GetComponent<AI_PatrollPoint> ().getIndex().CompareTo(b.GetComponent<AI_PatrollPoint> ().getIndex());
	}

	/*int isObjInArray(GameObject[] list, GameObject Obj){
		for (int i = 0; i < list.Length; i ++){
			if (Obj == list [i]) {
				//Debug.Log ("SEARCH: Found! " + i);
				return i;
			}
		}
		//Debug.Log ("SEARCH: Not Found -1");
		return -1;
	}

	int Arraypush(GameObject[] list){
		for (int i = list.Length-1; i >= 0 ; i --){
			if (list [i] != null) {
				Debug.Log("push To: " + (i+1));
				return i+1;

			}
		}
		return 0;
	}*/

	void printArray(GameObject[] list){
		for (int i = 0; i < list.Length; i ++){
			Debug.Log ("LIST: " + i + " " + list [i]);
		}
	}

	/*int NullCount(GameObject[] list){
		int nullCount = 0;
		for (int i = 0; i < list.Length; i ++){
			if (list[i] == null) {
				nullCount++;
			}
		}
		return nullCount;
	}*/


	GameObject[] PathFinding(GameObject Start, GameObject Goal){
		List<GameObject> OpenSet = new List<GameObject>(PatrollList.Count);
		List<GameObject> ClosedSet = new List<GameObject>(PatrollList.Count);
		OpenSet.Add(Start);
		Start.GetComponent<AI_PatrollPoint> ().setGn (0);
		Start.GetComponent<AI_PatrollPoint> ().calculate_fn (Goal);

		int CurrentLowerindex = 0;
		//A* algorithm // f(n) = g(n) + h(n)
		while (OpenSet.Count != 0){//NullCount(OpenSet) < OpenSet.Length
			//Debug.Log("IN");
			//printArray(OpenSet);
			for(int i = 0; i < OpenSet.Count-1; i++){
				//if(OpenSet[i] != null)
				if(OpenSet[i].GetComponent<AI_PatrollPoint>().calculate_fn(Goal) < OpenSet[CurrentLowerindex].GetComponent<AI_PatrollPoint>().calculate_fn(Goal)){
					CurrentLowerindex = i;
				}
			}
			GameObject Current = OpenSet [CurrentLowerindex];

			if (Current == Goal){
				return reconstruct_path (Current);
			}

			//removefromArray(openSet, Current);
			OpenSet.Remove(Current);

			//OpenSet = ArrangeArray (OpenSet);
			//ReducingSize--;

			//closedSet.push(Current);
			ClosedSet.Add(Current);

			//ClosedSet = ArrangeArray (ClosedSet);
			/*Debug.Log("AA:");
			Debug.Log(Current.name);
			Debug.Log (Current.GetComponent<AI_PatrollPoint> ().getConnections ().Length);
			//printArray (Current.GetComponent<AI_PatrollPoint> ().getConnections ());*/
			foreach (GameObject connection in Current.GetComponent<AI_PatrollPoint> ().getConnections ()) {
				//Debug.Log ("COUNT !");
				Debug.DrawRay (Current.transform.position, connection.transform.position - Current.transform.position, Color.cyan);
				if (ClosedSet.Contains(connection) == false) {
					float tempG = (Start.transform.position - connection.transform.position).magnitude;
					//float tempG = Current.GetComponent<AI_PatrollPoint> ().getGn () + (Current.transform.position - connection.transform.position).magnitude;
					if (OpenSet.Contains(connection) == false) {
						//Debug.Log ("ADD CONN TO OPEN");
						//openSet.push(connection);
						OpenSet.Add(connection);
					} else if (tempG < connection.GetComponent<AI_PatrollPoint> ().getGn ()) {
						//Debug.Log ("ENTER");
						connection.GetComponent<AI_PatrollPoint> ().setGn (tempG);
						connection.GetComponent<AI_PatrollPoint> ().calculate_fn (Goal);
					}
					connection.GetComponent<AI_PatrollPoint> ().setPrevious (Current);
				}
			}
			/*Debug.Log ("O: ");
			printArray(OpenSet.ToArray());
			Debug.Log ("C: ");
			printArray(ClosedSet.ToArray());
			Debug.Log ("Foreach DONE");*/
		}
		Debug.Log("FAILED");
		//Instantiate(PatrollAux,new Vector3(transform.position.x, transform.position.y + 2 , transform.position.z),transform.rotation);
		return null;
	}

	GameObject[] reconstruct_path(GameObject Current){
		//FindPath by Backtracking
		List<GameObject> total_path = new List<GameObject>(PatrollList.Count);
		//fpath.push(temp);
		total_path.Add(Current);
		//Debug.DrawRay (Current.transform.position, Vector3.up, Color.yellow, 10);

		//Current = CameFrom;
		while(Current.GetComponent<AI_PatrollPoint>().getPrevious()){
			//Debug.Log ("FINAL");
			Debug.DrawRay (Current.transform.position, Current.GetComponent<AI_PatrollPoint>().getPrevious().transform.position - Current.transform.position, Color.yellow);
			//fpath.push(temp.getComponent<AI_PatrollPoint().getPrevious());
			//fpath = ArrangeArray(fpath);
			Current = Current.GetComponent<AI_PatrollPoint>().getPrevious();
			total_path.Add(Current);
		}
		total_path.Reverse ();
		//printArray (total_path.ToArray());
		//resetPatrollNodes ();
		//Debug.Log("FINISHED ^-^!");
		return total_path.ToArray();
	}

	void resetPatrollNodes(){
		//cleanNodes
		for (int i = 0; i < PatrollList.Count; i ++){
			PatrollList[i].GetComponent<AI_PatrollPoint> ().setGn (Mathf.Infinity);
			PatrollList[i].GetComponent<AI_PatrollPoint> ().setFn (Mathf.Infinity);
		}
	}

	GameObject getPatrollObjbyIndex(int ind){
		for (int i = 0; i < PatrollList.Count; i++) {
			if (PatrollList [i].GetComponent<AI_PatrollPoint> ().getIndex () == ind) {
				return PatrollList [i];
			}
		}
		Debug.Log ("FailedSearchForIndex");
		return null;
	}

	public static bool FastApproximately(float a, float b, float threshold)
	{
		return ((a - b) < 0 ? ((a - b) * -1) : (a - b)) <= threshold;
	}

	void InvestigateSound (){
		//SAVE START POS
		if (StartInvestigateSound) {
			soundsearchindex = 0;
			//StartInvestigateSoundPoint = transform.position;
			PathToSound = PathFinding (Connector, _SoundDetect.SoundMemory[0]);
			soundsearchindex++;
			StartInvestigateSound = false;
			//Debug.Break ();
		}
		Debug.Log ("SI: " + soundsearchindex);
		InvestigateSoundPoint = PathToSound[soundsearchindex].transform.position;
		//LOOK TO SOUND DIR
		RaycastHit soundhit;

		if (Physics.Raycast (transform.position, _SoundDetect.InvestigatePoint.transform.position - transform.position,out soundhit) && soundhit.collider.tag == "PatrollPoint" && soundhit.collider.gameObject.layer == 10) {
			transform.forward = Vector3.Lerp(transform.forward, _SoundDetect.InvestigatePoint.transform.position - transform.position, Time.deltaTime);
			InvestigateWaitTime -= Time.deltaTime;
			if (InvestigateWaitTime <= 0) {
				//GOTO SOUND POS
				transform.position = /*transform.forward.normalized * Time.deltaTime*2 ;//*/Vector3.MoveTowards (transform.position, _SoundDetect.InvestigatePoint.transform.position, Time.deltaTime* 3f);
				//IF there
			}
			if (FastApproximately (transform.position.x, _SoundDetect.InvestigatePoint.transform.position.x, .5f) && FastApproximately (transform.position.z,  _SoundDetect.InvestigatePoint.transform.position.z, .5f)) {
				_SoundDetect.clearAudioMemoryPoint ();
				soundsearchindex = 0;
				StartInvestigateSound = false;
				InvestigateWaitTime = 2;
			}
		} else {
			transform.forward = Vector3.Lerp(transform.forward,  InvestigateSoundPoint - transform.position, Time.deltaTime);
			//Wait a little
			InvestigateWaitTime -= Time.deltaTime;
			if (InvestigateWaitTime <= 0) {
				//GOTO SOUND POS
				transform.position = /*transform.forward.normalized * Time.deltaTime*2 ;//*/Vector3.MoveTowards (transform.position, InvestigateSoundPoint, Time.deltaTime* 3f);
				//IF there
				if (transform.position == InvestigateSoundPoint/*FastApproximately (transform.position.x, InvestigateSoundPoint.x, .5f) && FastApproximately (transform.position.z, InvestigateSoundPoint.z, .5f)*/) {
					//_SoundDetect.SoundMemory.RemoveAt (0);
					soundsearchindex++;
					StartInvestigateSound = true;
					newStep = true;

					//Start Patrolling
					if (soundsearchindex == PathToSound.Length) {
						_SoundDetect.clearAudioMemoryPoint ();
						soundsearchindex = 0;
						InvestigateWaitTime = 2;
						StartInvestigateSound = false;
					}
				} 
				/*if (GetBackToPatroll) {
				transform.forward = Vector3.Lerp (transform.forward, StartInvestigateSoundPoint - transform.position, Time.deltaTime * .5f);
			}*/
			}
		}
	}

	void Patrolling(){
		if (newStep) {
			
			PatrollPath = PathFinding (Connector, getPatrollObjbyIndex(PatrollOrder[PatrollPoint]));
			if (PatrollWalk >= PatrollPath.Length) {
				PatrollWalk = PatrollPath.Length-1;
			}
			NExtPatrollPath = PatrollPath [PatrollWalk].transform.position;
			newStep = !newStep;
			Debug.Log ("PW: " + PatrollWalk);
			Debug.Log("PP: " + PatrollPoint);
			//Debug.Break ();
		}


		if (/*transform.position == NExtPatrollPath*/FastApproximately(transform.position.x, NExtPatrollPath.x, .5f) && FastApproximately(transform.position.z, NExtPatrollPath.z, .5f)) {
			PatrollWalk++;
			if (PatrollWalk == PatrollPath.Length) {
				if (PatrollPoint == PatrollOrder.Length-1 || PatrollPoint == 0) {
					patrollDir *= -1;
				}
				PatrollWalk = 1;
				PatrollPoint += patrollDir;
				newStep = true;
			}
			//PatrollPath = PathFinding (Connector, getPatrollObjbyIndex(PatrollOrder[PatrollPoint]));
			if (PatrollWalk >= PatrollPath.Length) {
				PatrollWalk = PatrollPath.Length-1;
			}
			//newStep = true;
			NExtPatrollPath = PatrollPath [PatrollWalk].transform.position;
			//Debug.Log ("PW: " + PatrollWalk);
			//Debug.Log("PP: " + PatrollPoint);
		}

		//change Walk Direction
		transform.forward = Vector3.Lerp(transform.forward, NExtPatrollPath - transform.position, Time.deltaTime*.3f);

		//WALK to direction
		NExtPatrollPath.y = transform.position.y;
		transform.position = /*transform.forward.normalized * Time.deltaTime*2 ;//*/Vector3.MoveTowards (transform.position, NExtPatrollPath, Time.deltaTime* 3f);

		}

	void ChasePlayer(){
		transform.forward = Vector3.Lerp(transform.forward, GameObject.FindGameObjectWithTag ("Player").transform.position - transform.position, Time.deltaTime);
		transform.position = /*transform.forward.normalized * Time.deltaTime*2 ;//*/Vector3.MoveTowards (transform.position,  GameObject.FindGameObjectWithTag ("Player").transform.position, Time.deltaTime* 3f);
		StartInvestigateSound = false;
		//_SoundDetect.InvestigatePoint = null;
	}

	// Update is called once per frame
	/*float updateFreq = .1f; bool start = true;*/

	GameObject[] PatrollPath;
	int PatrollPoint = 0;
	int PatrollWalk = 1; 
	int patrollDir = -1;
	bool newStep = true;

	bool StartInvestigateSound = true;
	float InvestigateWaitTime = 2;
	Vector3 StartInvestigateSoundPoint;
	Vector3 InvestigateSoundPoint;
	GameObject[] PathToSound;
	int soundsearchindex = 0;

	public soundDetectionEnemy _SoundDetect;
	private EnemySightScript _SightScript;

	void Update () {
		if (_SightScript.PlayerInSight) {
			InvestigateWaitTime = 2;
			soundsearchindex = 0;
			ChasePlayer ();
			_SoundDetect.clearAudioMemoryPoint ();
		}else if (_SoundDetect.InvestigatePoint) {
			InvestigateSound();
		} else {
			InvestigateWaitTime -= Time.deltaTime;
			if (InvestigateWaitTime <= 0)
			Patrolling ();
		}
	}
}


//THE NET 34:51 https://www.youtube.com/watch?v=-qJwU8H8UqQ&list=UUyhqBfYUdAlECQtfmNIlbuw&index=2

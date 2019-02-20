using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thirdPersonCam : MonoBehaviour {

	public GameObject Target;
	public GameObject FOVobj;
	public LayerMask layer_mask;
	//public LockOnView _LockOnView;

	public bool CamInvetY = true;
	public bool SwitchCamInv = true;
	public bool InvetAimX  = false;
	public bool isLockedOn = false;
	public bool attach_wall = false;
	public float LockedOnRange = 15;
	public float CamSpeed;
	public bool aimmode;

	public float SlashOffsetZ = 0;
	//public float SlashoffsetY = 0;
	//public float SlashoffsetX = 0;

	public float AimOffsetZ = 0;
	public float AimOffsetX = 0;
	public float AimOffsetY = 0;
	private PLController PLstate;

	private GameObject[] Enemies;
	private GameObject LockedOnEnemy;
	// Use this for initialization
	void Start () {
		PLstate = Target.GetComponent<PLController> ();
		LockedOnEnemy = null;
	}

	void setCursorState(){
		Cursor.lockState = Input.GetKey (KeyCode.Tab) ? CursorLockMode.None : CursorLockMode.Locked;
	}
	float ENdist;
	void Update(){
		
		//LOCK ON SYSTEM
		//LockedOnEnemy = _LockOnView.getEnemyTarget ();
		Vector3 camford = this.transform.forward.normalized * LockedOnRange;

		if (Input.GetKeyDown(KeyCode.F)){
			Enemies = GameObject.FindGameObjectsWithTag ("Enemy");
			foreach(GameObject En in Enemies){
				Vector3 EnVec = En.transform.position - Target.transform.position;
				if (EnVec.magnitude > ENdist)
					ENdist = EnVec.magnitude;
				RaycastHit hitEn;
				if (Physics.Raycast (Target.transform.position, EnVec, out hitEn) && hitEn.distance < LockedOnRange && hitEn.collider.tag == "Enemy") {
					Debug.DrawRay (Target.transform.position, camford, Color.yellow);
					if (Vector3.Angle(camford,EnVec) < 25) {
						LockedOnEnemy = hitEn.collider.gameObject;
						isLockedOn = !isLockedOn;
					}
				}
			}
		}

		if (PLstate.isattachedwall == true)
		{
			attach_wall = true;
		}
		else
		{
			attach_wall = false;
		}

		if (getclosestEnemydist() > LockedOnRange) {
			isLockedOn = false;
		}

		/*LockedOnEnemy = Physics.Raycast(Target.transform.position, camfordXZ,
		if (LockedOnEnemy != null && (LockedOnEnemy.transform.position - Target.transform.position).magnitude < LockedOnRange) {
			if (Input.GetMouseButtonDown(2)) {
				/*if (_LockOnView.getEnemyTarget () != null) {
					Debug.DrawRay (Target.transform.position, LockedOnEnemy.transform.position - transform.position);
					isLockedOn = !isLockedOn;
				}


				if()
			}
		} else {
			isLockedOn = false;
		}*/

		//Dynamic Cam Wall detection
		RaycastHit camhit;
		float camRayDistance;

			if (Physics.Raycast (Target.transform.position, transform.position - Target.transform.position, out camhit, layer_mask)) {
			//if(camhit.collider.gameObject == )
			camRayDistance = camhit.distance;
			Debug.DrawRay (Target.transform.position, transform.position - Target.transform.position, Color.cyan);
			if (!Input.GetMouseButton (1) || isLockedOn) {
				//Debug.Log ("1/3: " + camhit.distance);
				//Limmit Distance (removes infinity)
				if (camRayDistance > 5) {
					camRayDistance = 5;
				}

				SlashOffsetZ = camRayDistance;
			} else if (!isLockedOn) {
				
				if (camRayDistance > 1) {
					camRayDistance = 1;
				}

				//Debug.Log ("2: " + camhit.distance);
				AimOffsetZ = camRayDistance;
			}
		} else {
			SlashOffsetZ = 5;
			AimOffsetZ = 1;
		}
	}

	public float getclosestEnemydist(){
		Enemies = GameObject.FindGameObjectsWithTag("Enemy");
		float smalldist = Mathf.Infinity;
		float EdistFromPlayer;
		foreach (GameObject enemy in Enemies) {
			EdistFromPlayer = (enemy.transform.position - Target.transform.position).magnitude;
			//Debug.Log ("DIST FROM PL : " + distFromPlayer);
			if (EdistFromPlayer < smalldist) {
				smalldist = EdistFromPlayer;
			}
		}
		return smalldist;
	}

	float AbsAngleBtw2Vect3(Vector3 a, Vector3 b){
		Vector3 diff = a - b;
		return Mathf.Abs (Vector3.Angle (Vector3.right, diff));
	}

	void FixedUpdate () {
		setCursorState();
		
		float XmousePos = Input.GetAxis ("Mouse X");
		float YmousePos;	

		if (CamInvetY) {
			YmousePos = Input.GetAxis ("Mouse Y");
		} else {
			YmousePos = -Input.GetAxis ("Mouse Y");
		}

		Vector3 MouseVec = new Vector3 (XmousePos, 0, YmousePos);
		Vector3 CamFordXZ = transform.forward;
		CamFordXZ.y = 0;
		Quaternion referentialRot = Quaternion.FromToRotation (Target.transform.forward, CamFordXZ);

		//Aim
		if(Input.GetMouseButtonDown(2)){
			if(SwitchCamInv)
				CamInvetY = !CamInvetY;
			CamSpeed *= 2; 
		}

		//SwitchSide
		if(Input.GetKeyDown(KeyCode.C)){
			InvetAimX = !InvetAimX;
			AimOffsetX *= -1;
		}

		//CamMovement
		Vector3 TargetMove = Target.transform.position + transform.rotation * (Quaternion.AngleAxis (XmousePos , new Vector3(0,1,0)) * Quaternion.AngleAxis (YmousePos , new Vector3(1,0,0))) * (new Vector3 (0, 0, -SlashOffsetZ));
		//Vector3 TargetAim = Target.transform.position + transform.rotation * new Vector3 (AimOffsetX, AimOffsetY, -AimOffsetZ);	
		Vector3 TargetLock = Target.transform.position + transform.rotation * new Vector3 (0, 1, -SlashOffsetZ);
	
		if (Input.GetMouseButton (2) && !isLockedOn) {
			//AIM MODE 
			aimmode = true;
			transform.RotateAround (Target.transform.position, Target.transform.up, XmousePos);
			transform.RotateAround (Target.transform.position, Target.transform.right, YmousePos);
				if (PLstate.isCrouching) {
				transform.eulerAngles = new Vector3 (transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
				transform.position = Vector3.Lerp (transform.position, Target.transform.position + transform.rotation * new Vector3 (0, 0, -AimOffsetZ), Time.fixedDeltaTime * CamSpeed);
			} else {
				transform.eulerAngles = new Vector3 (transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
				transform.position = Vector3.Lerp (transform.position, Target.transform.position + transform.rotation * new Vector3 (AimOffsetX, AimOffsetY, -AimOffsetZ), Time.fixedDeltaTime * CamSpeed);
			}

			//transform.position = TargetAim;
		} else if (!isLockedOn && !attach_wall)
		{
			aimmode = false;
			transform.position = TargetMove;
			//transform.position = Vector3.Slerp (transform.position, TargetMove, Time.deltaTime * CamSpeed);
			transform.LookAt(Target.transform.position);
		}
		else if (isLockedOn && !attach_wall)
		{
			//LOCKED ON
			Vector3 LockCamPos = LockedOnEnemy.transform.position - Target.transform.position;
			transform.position = Vector3.Lerp(transform.position, TargetLock, Time.fixedDeltaTime * 5);
			transform.LookAt(LockedOnEnemy.transform.position + LockCamPos.normalized * LockCamPos.magnitude);
			//Debug.DrawRay (ClosestEnemy.transform.position + LockCamPos.normalized * LockCamPos.magnitude, Vector3.up, Color.yellow);
		}
		else if (!isLockedOn && attach_wall)
		{
			print(PLstate.d);
			Vector3 Normal = Vector3.ProjectOnPlane(PLstate.d , PLstate.p);


			if (Input.GetAxis("Horizontal") > 0)
			{

				transform.position = Vector3.Lerp(transform.position, new Vector3(Normal.x - 1, Normal.y, Normal.z + 2), Time.fixedDeltaTime * 5);
			}
			if (Input.GetAxis("Horizontal") < 0)
				transform.position = Vector3.Lerp(transform.position, new Vector3(Normal.x - 1, Normal.y, Normal.z - 2), Time.fixedDeltaTime * 5);


		}
	}
}


/*public GameObject getclosestEnemy(){
		Enemies = GameObject.FindGameObjectsWithTag("Enemy");
		float smalldist = Mathf.Infinity;
		float EdistFromPlayer;
		foreach (GameObject enemy in Enemies) {
			EdistFromPlayer = (enemy.transform.position - Target.transform.position).magnitude;
			//Debug.Log ("DIST FROM PL : " + distFromPlayer);
			if (EdistFromPlayer < smalldist) {
				smalldist = EdistFromPlayer;
			}
		}
		return smalldist;
	}*/

/*GameObject getLookingAtEnemy(){
Enemies = GameObject.FindGameObjectsWithTag("Enemy");
float EnemyAngle;
float lowestAngle = Mathf.Infinity;
foreach (GameObject enemy in Enemies) {
	EdistFromPlayer = (enemy.transform.position - Target.transform.position).magnitude;
	//Debug.Log ("DIST FROM PL : " + distFromPlayer);
	EnemyAngle = AbsAngleBtw2Vect3(transform.forward,enemy.transform.position - transform.position);
	Debug.Log ("EnAngle = " + lowestAngle);
	if (EnemyAngle < lowestAngle) {
		lowestAngle = EnemyAngle;
		LockedOnEnemy = enemy;
	}
	//Debug.DrawRay(enemy.transform.position, enemy.transform.forward, Color.blue);
	Debug.DrawRay(enemy.transform.position, transform.position - enemy.transform.position, Color.blue);
}
Debug.Log ("LowAngle = " + lowestAngle);
return LockedOnEnemy;
}*/

/*void Update(){
//LOCK ON SYSTEM
//getLookingAtEnemy ();
Debug.DrawRay(transform.position, transform.forward, Color.yellow);
//float EnemyDist = (getLookingAtEnemy ().transform.position - Target.transform.position).magnitude;
if (Input.GetKeyDown (KeyCode.C)) {
	if (EdistFromPlayer <= LockedOnRange) {
		getLookingAtEnemy ();
		isLockedOn = !isLockedOn;
	}
} else if (EdistFromPlayer > LockedOnRange) {
	isLockedOn = false;
}
}*/
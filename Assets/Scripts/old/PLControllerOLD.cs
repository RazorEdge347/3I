using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLControllerOLD : MonoBehaviour {
	public Transform Hand;
	public Camera MainCamera;

	public bool Grappleactive;
    public int GrappleMode = 0;
    public GameObject Hook;

	public float PLSpeed = 5;
	private float JumpSpeed = 700f;
	private float GravityAcelaration = 25f;
	public bool isGrounded;
	public bool isGrabbingLedge = false;
	public bool isCrouching = false;
	public bool isWalking = false;
	public bool isSprinting = false;
	private RaycastHit Handhit;	
	private Vector3 camFordXZ;
	// Use this for initialization
	void Start () {
		 Hook = GameObject.Find("Hook");
	}
	
void OnCollisionEnter(Collision col)
    {
        // if player reaches grappel , it attaches to the wall 
        if (GrappleMode == 1)
        {
            if (col.gameObject.name == "Hook"){
				GrappleMode = 2;
            }

			if (col.gameObject.name == "Hook")
			{
				GrappleMode = 2;
			}
				
        }

    }

	// Update is called once per frame
	void Update () {

		 //Shoot Grappel
        if (Input.GetKeyDown(KeyCode.Mouse0))
            Hook.GetComponent<GrappleHook>().GrapplingShot();

        // Grapple confirmation
        if (Grappleactive == true)
        {
            Vector3 dist = Hook.transform.position - this.transform.position;

            if(dist.magnitude > 15)
            {
                Hook.GetComponent<GrappleHook>().Grapplingreturns();
            }
            //Player inputs button to Motion (Player to Grappel)
			if (Input.GetKeyDown(KeyCode.V))
            {
                GrappleMode = 1;
            }
            // Player to Grappel - motion
            if(GrappleMode == 1)
            {
                
                    transform.position = Vector3.Lerp(transform.position, Hook.transform.position, Time.fixedDeltaTime * 7f);

               
            }
            //Player is attached to a wall
            if (GrappleMode == 2) {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                // Player drops to the ground with LCTRL 
                if (Input.GetKeyDown(KeyCode.LeftControl))
                {
                    Grappleactive = false;
                    GrappleMode = 0;
                    
                }

            }


        }

		//Grab Ledge
		Debug.DrawRay (Hand.position,  Quaternion.Euler(0,-45,0) * camFordXZ.normalized * 1.5f,Color.green);
		Debug.DrawRay (Hand.position, Quaternion.Euler(0,45,0) * camFordXZ.normalized * 1.5f,Color.red);
		/*if (MainCamera.GetComponent<thirdPersonCam> ().isLockedOn == false) {
		}*/
		if (Physics.Raycast (Hand.position, Quaternion.Euler(0,-45,0) * camFordXZ , out Handhit, 1.5f) || Physics.Raycast (Hand.position,Quaternion.Euler(0,45,0) * camFordXZ, out Handhit, 1.5f) || 
			Physics.Raycast (Hand.position, Quaternion.Euler(0,-45,0) * transform.forward , out Handhit, 1.5f) || Physics.Raycast (Hand.position,Quaternion.Euler(0,45,0) * transform.forward, out Handhit, 1.5f)  
			&& !isGrounded && !isCrouching && !isSprinting && !isWalking && !isCrouching) {

			if (Handhit.collider.tag == "Ledge" && GetComponent<Rigidbody>().velocity.y <= 1) {
				//Debug.Log ("LEDGE!");
				isGrabbingLedge = true;
				isGrounded = true;
				isCrouching = false;
				isSprinting = false;
				//GetComponent<Rigidbody> ().velocity = Vector3.zero;
			}else {
				//isGrabbingLedge = false;
				//isGrounded = false;
			}
		}
		//PL RUN
		if (Input.GetKey (KeyCode.LeftShift) && !isGrabbingLedge && isGrounded && !isCrouching) {
			isSprinting = true;
			isWalking = false;
			isCrouching = false;
			PLSpeed = 7.0f * 1.5f;
		} else if(!isGrabbingLedge && isGrounded && !isCrouching && !isWalking ){
			isSprinting = false;
			PLSpeed = 7.0f;
		}
			
		//PL Slow Walk
		if (Input.GetKeyDown (KeyCode.LeftAlt) && !isGrabbingLedge && isGrounded && !isSprinting && !isCrouching) {
			isWalking = !isWalking;
			PLSpeed = 7.0f / 2.0f;
		} else if(!isGrabbingLedge && isGrounded && !isCrouching && !isSprinting && !isWalking && !isCrouching){
			PLSpeed = 7.0f;
		}

		//PLCrouch
		if (Input.GetKeyDown (KeyCode.LeftControl) && !isGrabbingLedge && isGrounded && !isSprinting) {
			isCrouching = !isCrouching;
			isSprinting = false;
			PLSpeed = 7.0f / 4.0f;
			if (isCrouching) {
				transform.localScale = new Vector3(1f,0.5f,1f);
			} else{
				transform.localScale = new Vector3(1f,1f,1f);
				if (isWalking) {
					PLSpeed = 7.0f / 2.0f;
				}
			}
		} else if(!Physics.Raycast (Hand.transform.position, Hand.transform.up,1.5f,9) && isGrabbingLedge && isGrounded && !isCrouching && !isSprinting && !isWalking && !isCrouching){
			PLSpeed = 7.0f;
			transform.localScale = new Vector3(1f,1f,1f);
		}
			
		//dropOutOfLedge
		if(Input.GetKey(KeyCode.LeftControl) && isGrabbingLedge){
			isGrabbingLedge = false;
			isGrounded = false;
			isCrouching = false;
			//GetComponent<Rigidbody> ().AddForce (transform.rotation * new Vector3 (0f, -JumpSpeed * 0.2f, -10f));
		}

		/*if (!isGrounded) {
			PLSpeed -= 0.2f;
			PLSpeed = Mathf.Clamp(PLSpeed,0,100.0f);
		}*/
	}

	void FixedUpdate(){

		Transform camTrf = MainCamera.transform;
		camFordXZ = Vector3.Normalize(new Vector3 (camTrf.forward.x, 0, camTrf.forward.z));

		//PL DIRECTION
		if (!Input.GetMouseButton (1) && !isGrabbingLedge) {
			if (Input.GetAxis ("Vertical") > 0) {
				transform.forward = Vector3.Lerp (transform.forward, camFordXZ, Time.fixedDeltaTime * PLSpeed);
			}

			if (Input.GetAxis ("Vertical") < 0) {
				transform.forward = Vector3.Lerp (transform.forward, -camFordXZ, Time.fixedDeltaTime * PLSpeed);
			}

			if (Input.GetAxis ("Horizontal") > 0) {
				transform.forward = Vector3.Lerp (transform.forward, Quaternion.Euler (0, 90, 0) * camFordXZ, Time.fixedDeltaTime * PLSpeed);
			}

			if (Input.GetAxis ("Horizontal") < 0) {
				transform.forward = Vector3.Lerp (transform.forward, Quaternion.Euler (0, 90, 0) * -camFordXZ, Time.fixedDeltaTime * PLSpeed);
			}
		} else if (isGrounded){
			transform.forward = Vector3.Lerp (transform.forward, camFordXZ,  Time.fixedDeltaTime * PLSpeed);
		}

		//MovementTransform
		if (!isGrabbingLedge) {
			transform.position += camTrf.right * Input.GetAxis("Horizontal") * PLSpeed * Time.fixedDeltaTime;
			transform.position += camFordXZ * Input.GetAxis("Vertical") * PLSpeed * Time.fixedDeltaTime;
			//ON LEDGE
		}else if (!Input.GetKeyDown(KeyCode.LeftControl) && !Input.GetKeyDown(KeyCode.Space) && isGrounded && isGrabbingLedge){
			
			transform.position += -Handhit.normal * (PLSpeed/2) * Time.fixedDeltaTime;
			transform.position += Quaternion.Euler(0,90,0) * -Handhit.normal * Input.GetAxis ("Horizontal")* (PLSpeed/2) * Time.fixedDeltaTime;
		}

		//isGrounded?
		if (Physics.Raycast (transform.position, new Vector3 (0, -1, 0), 1.1f)) {
			//Debug.Log ("isGrounded");
			isGrounded = true;
		} else {
			//Debug.Log ("Airborn");
			isGrounded = false;
		}

		//Jump
		if (Input.GetKeyDown (KeyCode.Space) && isGrounded && !isCrouching ) {
			isGrounded = false;
			GetComponent<Rigidbody> ().AddForce (transform.rotation * new Vector3 (0f, JumpSpeed, 0f)); 
		} else if (Input.GetKeyDown (KeyCode.Space) && isGrabbingLedge) {
			if (Input.GetKey (KeyCode.W)) {
				GetComponent<Rigidbody> ().AddForce (new Vector3 (camFordXZ.x * JumpSpeed* 0.2f, JumpSpeed * 1.3f,camFordXZ.z * JumpSpeed* 0.2f));
			} else {
				GetComponent<Rigidbody> ().AddForce (new Vector3 (camFordXZ.x * JumpSpeed* 0.2f, JumpSpeed ,camFordXZ.z * JumpSpeed* 0.2f));
			}
			isGrabbingLedge = false;
			isGrounded = false;

		}
			
		//Gravity
		if (!isGrounded && !isGrabbingLedge && GrappleMode==0) {
			/*if (Mathf.Abs(GetComponent<Rigidbody> ().velocity.y) < 15f) {
				GetComponent<Rigidbody> ().AddForce (new Vector3 (0f, -GravityAcelaration, 0f));
			} else {
				if (GetComponent<Rigidbody> ().velocity.y > 0) {
					GetComponent<Rigidbody> ().velocity = new Vector3(GetComponent<Rigidbody> ().velocity.x, 15f, GetComponent<Rigidbody> ().velocity.z);
				} else {
					GetComponent<Rigidbody> ().velocity = new Vector3(GetComponent<Rigidbody> ().velocity.x, -15f, GetComponent<Rigidbody> ().velocity.z);
				}
			}*/
			GetComponent<Rigidbody> ().AddForce (new Vector3 (0f, -GravityAcelaration, 0f));
		}else{
			GetComponent<Rigidbody> ().velocity = Vector3.zero;
		}

	}
}

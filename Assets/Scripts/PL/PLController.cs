using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLController : MonoBehaviour {
	public Transform Hand;
	public Camera MainCamera;

	public bool Grappleactive;
    public int GrappleMode = 0;
    public GameObject Hook;
    private RaycastHit hit;
	public Animator PLA;
	private float iddleState = 0;
	private float MovementState = 0;
    //private bool attachedwall = false;
    

    private GameObject hitbox;
	public Vector3 d;
	public Vector3 p;


    public float PLSpeed = 5;
	private float JumpSpeed = 700f;
	private float GravityAcelaration = 25f;
	public float movementSpeed;
	public float groundedRange = 1.1f;
	public bool isGrounded;
	public bool isattachedwall = false;
	public bool isGrabbingLedge = false;
	public bool isCrouching = false;
	public bool isWalking = false;
	public bool isSprinting = false;
    private int ani_framecounta = 0;
    private int roll_frame = 0;
    //Hack and Slash
    
    private bool roll_enabeld;
    private bool ani_attack_enabeld;
    private bool ani_Hattack_enabeld;
    private bool frame_start;

	private RaycastHit feethit;
	private RaycastHit Handhit;	
	private Vector3 camFordXZ;
	// Use this for initialization
	void Start () {
		Hook = GameObject.Find("Hook");
        hitbox = GameObject.Find("Hitbox");

	}

    /*public bool get_attack_ani()
    {
        return ani_attack_enabeld;
    }

    public bool get_Hattack_ani()
    {
        return ani_Hattack_enabeld;
    }

    void Attack_motion(int frame)
    {
        if (frame > 3 && frame < 7)
        {
            hitbox.GetComponent<MeshRenderer>().enabled = true;
            //print("GOTCHA BITCH");
            hitbox.GetComponent<BoxCollider>().enabled = true;
        }
        else
        {
            if (frame == 50)
            {
                ani_attack_enabeld = false;
                ani_framecounta = 0;
                frame_start = false;
            }

            //print("Wait for it");
            hitbox.GetComponent<MeshRenderer>().enabled = false;
            hitbox.GetComponent<BoxCollider>().enabled = false;
        }
    }

    void Heavy_Attack_motion(int frame)
    {
        if (frame > 5 && frame < 15)
        {
            hitbox.GetComponent<MeshRenderer>().enabled = true;
            //print("GOTCHA BITCH");
            hitbox.GetComponent<BoxCollider>().enabled = true;
        }
        else
        {
            if (frame == 55)
            {
                ani_Hattack_enabeld = false;
                ani_framecounta = 0;
                frame_start = false;
            }

            //print("Wait for it");
            hitbox.GetComponent<MeshRenderer>().enabled = false;
            hitbox.GetComponent<BoxCollider>().enabled = false;
        }
    }

    void Roll_motion(int frame)
    {
        //print(frame);
        if(frame < 5)
        {
            gameObject.tag = "Imortal";
            
        }
        else
        {
            gameObject.tag = "Player";
           
        }

        if (frame == 10)
        {
            
            roll_enabeld = false;
            frame_start = false;
        }

        transform.position += camFordXZ * PLSpeed * 3f * Time.fixedDeltaTime;
    }

    void Defend_motion()
    {

    }*/
	
void OnCollisionEnter(Collision col)
    {
        // if player reaches grappel , it attaches to the wall 
        if (GrappleMode == 1)
        {
            if (col.gameObject.name != "Hook")
            {

            }

            else
            {
                GrappleMode = 2;
            }
        }

		if(isattachedwall == true)
		{

			d = col.contacts[0].point;
			p = transform.position;

		}

    }

	// Update is called once per frame
	void Update () {
		Vector3 feet = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        //Shoot Grappel
        if (isGrounded == true)
        {
            if (Input.GetMouseButton(2) && Input.GetMouseButton(0))
                Hook.GetComponent<GrappleHook>().GrapplingShot();
        }
        // Grapple confirmation
        if (Grappleactive == true)
        {
            if (GrappleMode == 0)
            {
               
                
                Vector3 dist = Hook.transform.position - this.transform.position;

                if (dist.magnitude > 15)
                {
                    Hook.GetComponent<GrappleHook>().Grapplingreturns();
                }
                //Player inputs button to Motion (Player to Grappel)
                if (Hook.GetComponent<GrappleHook>().returngrapple != true)
                {
                    
                    LayerMask layer_mask = 1 << 8;
                    layer_mask = ~layer_mask;
                    Debug.DrawRay(Hook.transform.position, transform.position - Hook.transform.position);
                    if (Physics.Raycast(Hook.transform.position, transform.position - Hook.transform.position, out hit, Mathf.Infinity, layer_mask))
                    {
                        //print("GrappleMode :" + GrappleMode + " Entrou no 0");

                        if (hit.collider.gameObject.tag == "Player")
                        {
                            
                            if (Hook.GetComponent<GrappleHook>().inAir == false)
                            {
                                GrappleMode = 1;
                            }
                        }
						else if(hit.collider.gameObject.tag != "MainCamera" || (hit.collider.gameObject.tag != "PatrollPoints" && hit.collider.gameObject.transform.parent == null) || hit.collider.gameObject.tag == "Enemy" ){

                            Hook.GetComponent<GrappleHook>().Grapplingreturns();
                        }
                    }
                }
            }
           
            // Player to Grappel - motion
            if(GrappleMode == 1)
            {
                //print("GrappleMode :" + GrappleMode+" Entrou no 1");
                transform.position = Vector3.Lerp(transform.position, Hook.transform.position, Time.fixedDeltaTime * 7f);

               
            }
            //Player is attached to a wall
            if (GrappleMode == 2) {
                //print("GrappleMode :" + GrappleMode + " Entrou no 2");
                
                
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                // Player drops to the ground with LCTRL 
                if (Input.GetKeyDown(KeyCode.LeftControl))
                {
                    GrappleMode = 0;
                    Grappleactive = false;
                }
                // Player jumps (attached to a wall)
                if(Input.GetKeyDown(KeyCode.Space))
                {
                    isGrounded = true;
                                     
                }

            }


        }

		//Grab Ledge
		Debug.DrawRay (Hand.position,  Quaternion.Euler(0,-45,0) * camFordXZ.normalized * 1.5f,Color.green);
		Debug.DrawRay (Hand.position, Quaternion.Euler(0,45,0) * camFordXZ.normalized * 1.5f,Color.red);
		/*if (MainCamera.GetComponent<thirdPersonCam> ().isLockedOn == false) {
		}*/

		//FEET
		Debug.DrawRay(feet, Quaternion.Euler(0, 45, 0) * transform.forward * 1.5f, Color.blue);
		Debug.DrawRay(feet, Quaternion.Euler(0, -45, 0) * transform.forward * 1.5f, Color.magenta);

		//Cover System
		if (Physics.Raycast(feet, Quaternion.Euler(0, -45, 0) * transform.forward, out feethit, 1.5f) || Physics.Raycast(feet, Quaternion.Euler(0, 45, 0) * transform.forward, out feethit, 1.5f) || Physics.Raycast(feet, Quaternion.Euler(0, -45, 0) * transform.forward, out feethit, 1.5f)
			&& isGrounded && !isCrouching && !isSprinting && !isWalking && !isCrouching)
		{
			print("true");
			if (feethit.collider.gameObject.isStatic == true && Input.GetKeyDown(KeyCode.T) && isGrounded)
			{
				Vector3 pos = transform.forward;
				isattachedwall = !isattachedwall;
				isGrounded = true;
				isSprinting = false;
				isCrouching = false;
			}

		}

		else if (isattachedwall)
		{
			isattachedwall = false;

		}

		//Grabbing Ledge
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
			MovementState = 3;
		} else if(!isGrabbingLedge && isGrounded && !isCrouching && !isWalking ){
			isSprinting = false;
			PLSpeed = 7.0f;
			MovementState = 1;
		}
			
		//PL Slow Walk
		if (Input.GetKeyDown (KeyCode.LeftAlt) && !isGrabbingLedge && isGrounded && !isSprinting && !isCrouching) {
			isWalking = !isWalking;
			PLSpeed = 7.0f / 2.0f;
			MovementState = 2;

		} else if(!isGrabbingLedge && isGrounded && !isCrouching && !isSprinting && !isWalking && !isCrouching){
			PLSpeed = 7.0f;
			MovementState = 1;
		}

		//PLCrouch
		if (!isGrabbingLedge && isGrounded && !isSprinting) {
			if (Input.GetKeyDown (KeyCode.LeftControl)) {
				isCrouching = !isCrouching;
				isSprinting = false;
				PLSpeed = 7.0f / 4.0f;
				if (isCrouching) {
					//	transform.localScale = new Vector3(1f,0.5f,1f);
					MovementState = 0;
				} else{
					//transform.localScale = new Vector3(1f,1f,1f);

					if (isWalking) {
						PLSpeed = 7.0f / 2.0f;
						MovementState = 1;
					}
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



		//PLA.SetFloat ("iddleState", iddleState);


		//BELENDING ANIMATION

		if (movementSpeed != 0) {
			if (iddleState < 2) {
				iddleState += Time.deltaTime * 2;
			}

		} else {
			
			if (MovementState == 1) {
				//Debug.Log (PLA.GetFloat ("iddleState"));
				if (PLA.GetFloat("iddleState") > 1) {
					iddleState -= Time.deltaTime * 2;
				}else if(PLA.GetFloat("iddleState") < 1){
					iddleState += Time.deltaTime * 2;
				}
			}

			if (MovementState == 0) {
				if (PLA.GetFloat("iddleState") > 0) {
					iddleState -= Time.deltaTime * 2;
				}
			}

		}
		PLA.SetFloat ("MoveState", MovementState);
		PLA.SetFloat ("iddleState", iddleState);

		/*} else {
			
		}

		if (!isCrouching && isGrounded && iddleState > 1) {
			iddleState -= Time.deltaTime*2;	
			//MovementState -= Time.deltaTime*2;

		} else if (!isCrouching && isGrounded && iddleState < 1){
			iddleState += Time.deltaTime*2;	
			//MovementState += Time.deltaTime*2;
		}

		if (isCrouching && iddleState > 0 ) {
			iddleState -= Time.deltaTime*2;
			//MovementState -= Time.deltaTime*2;
		}*/


	}

	void FixedUpdate(){

        // Combat Skills
        /*bool aim = MainCamera.GetComponent<thirdPersonCam>().aimmode;

        if (isGrounded == true  && aim == false )
        {
            if (ani_Hattack_enabeld == false)
            {
                if (Input.GetMouseButton(0))
                {
                    frame_start = true;
                    ani_attack_enabeld = true;
                }
                if (ani_attack_enabeld == true)
                {
                    Attack_motion(ani_framecounta);
                }
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                frame_start = true;
                roll_frame = 0;
                roll_enabeld = true;
            }
            if (roll_enabeld == true)
                Roll_motion(roll_frame);

            if (ani_attack_enabeld == false)
            {
                if (Input.GetMouseButton(1))
                {
                    frame_start = true;
                    ani_Hattack_enabeld = true;
                }
                if (ani_Hattack_enabeld == true)
                    Heavy_Attack_motion(ani_framecounta);
            }


        }*/


		Transform camTrf = MainCamera.transform;
		camFordXZ = Vector3.Normalize(new Vector3 (camTrf.forward.x, 0, camTrf.forward.z));

		//PL DIRECTION
		if (!Input.GetMouseButton (2) && !isGrabbingLedge) {
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
		if (!isGrabbingLedge && GrappleMode !=2 && !isattachedwall ) {
			transform.position += camTrf.right * Input.GetAxis("Horizontal") * PLSpeed * Time.fixedDeltaTime;
			transform.position += camFordXZ * Input.GetAxis("Vertical") * PLSpeed * Time.fixedDeltaTime;
			movementSpeed = (Mathf.Abs(Input.GetAxis ("Horizontal") * PLSpeed * Time.fixedDeltaTime) + Mathf.Abs((Input.GetAxis ("Vertical") * PLSpeed * Time.fixedDeltaTime)))*35;
			//Debug.Log (movementSpeed);
		}else if (!Input.GetKeyDown(KeyCode.LeftControl) && !Input.GetKeyDown(KeyCode.Space) && isGrounded && isGrabbingLedge){
			movementSpeed = 0;
			transform.position += -Handhit.normal * (PLSpeed/2) * Time.fixedDeltaTime;
			transform.position += Quaternion.Euler(0,90,0) * -Handhit.normal * Input.GetAxis ("Horizontal")* (PLSpeed/2) * Time.fixedDeltaTime;
		}
		if ((!Input.GetKeyDown(KeyCode.LeftControl) && !Input.GetKeyDown(KeyCode.Space) && isGrounded && isattachedwall))
		{
			print("WTF??");
			transform.position += -feethit.normal * (PLSpeed / 2) * Time.fixedDeltaTime;
			transform.position += Quaternion.Euler(0, 90, 0) * -feethit.normal * Input.GetAxis("Horizontal") * (PLSpeed / 2) * Time.fixedDeltaTime;
		}

		//isGrounded?
		RaycastHit hitground;
		if (Physics.Raycast (transform.position, new Vector3 (0, -1, 0), out hitground ,groundedRange)) {
			//Debug.Log ("isGrounded");
			if(hitground.collider.tag != "PatrollPoint")
			isGrounded = true;
		} else {
			//Debug.Log ("Airborn");
			isGrounded = false;
		}

		//Jump
		if (Input.GetKeyDown (KeyCode.Space) && isGrounded && !isCrouching ) {
			isGrounded = false;
			GetComponent<Rigidbody> ().AddForce (transform.rotation * new Vector3 (0f, JumpSpeed, 0f));
            
		} else if (Input.GetKeyDown (KeyCode.Space) && (isGrabbingLedge || GrappleMode == 2)) {
			if (Input.GetKey (KeyCode.W)) {
				GetComponent<Rigidbody> ().AddForce (new Vector3 (camFordXZ.x * JumpSpeed* 0.2f, JumpSpeed * 1.3f,camFordXZ.z * JumpSpeed* 0.2f));
			} else {
				GetComponent<Rigidbody> ().AddForce (new Vector3 (camFordXZ.x * JumpSpeed* 0.2f, JumpSpeed ,camFordXZ.z * JumpSpeed* 0.2f));
			}
			isGrabbingLedge = false;
			isGrounded = false;
            if(Grappleactive == true)
            {
                
                GrappleMode = 0;
                Grappleactive = false;
            }
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
        if (frame_start == true)
        {
            ani_framecounta++;
            roll_frame++;
        }
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHook : MonoBehaviour {
    public PLController player;
	public bool inAir = false;
 	private GameObject pl;
    private float dist_pred;
 	public float speed = 70f;
    private bool shoot=false;
    private RaycastHit grappleHit;
   
    public bool returngrapple;
    Rigidbody rb;


 //To shoot your hook, call this method:
	void Start () {
        pl = GameObject.Find("PL");
        
	GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

            
	}	

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name == "grappler")
		{

			if (returngrapple == true)
			{
				GetComponent<Rigidbody>().velocity = Vector3.zero;
				transform.parent = pl.transform.GetChild(1).transform.GetChild(0).transform;
				transform.position = transform.parent.transform.position + transform.parent.transform.forward;
				transform.rotation = Camera.main.gameObject.transform.rotation;
				transform.localScale = new Vector3(1, 1, 1);
				GetComponent<SphereCollider>().enabled = false;
				player.Grappleactive = false;
				returngrapple = false;
			}

		}
	}

	void OnCollisionEnter(Collision col)
	{

		// Grapple Comes back to player

		if (col.gameObject.name == "PL")
		{
			print("Should be touching me without the 'returning grapple'");
			transform.parent = pl.transform.GetChild(1).transform.GetChild(0).transform;
			transform.position = transform.parent.transform.position + transform.parent.transform.forward;
			transform.rotation = Camera.main.gameObject.transform.rotation;
			transform.localScale = new Vector3(1, 1, 1);
			GetComponent<SphereCollider>().enabled = false;
			GetComponent<Rigidbody>().velocity = Vector3.zero;

		}
		//Grapple touches a wall
		else if (col.gameObject.tag != "Enemy")
		{
			print("Touches the wall");
			GetComponent<Rigidbody>().velocity = Vector3.zero;
			transform.parent = null;
			transform.localScale = new Vector3(.5f, .5f, .5f);
			player.Grappleactive = true;
			shoot = false;
		}
		else if (col.gameObject.tag == "Enemy")
		{

			Grapplingreturns();
			GetComponent<Rigidbody>().velocity = Vector3.zero;
			shoot = false;
		}


		inAir = false;
	}


//Grapple will be shot
public void GrapplingShot() {

         if (Physics.Raycast(transform.position, transform.parent.transform.parent.transform.forward, out grappleHit))
            dist_pred = grappleHit.distance;

        else
            dist_pred = 15f;

        GetComponent<Rigidbody>().isKinematic=false;
        GetComponent<SphereCollider>().enabled= true;
        shoot = true;
        transform.parent = null;      
        inAir = true;
    }

// Grapple returns
public void Grapplingreturns() {
        
        GetComponent<Rigidbody>().isKinematic = false;
        returngrapple = true;
        inAir = true;
    }
 
	// Use this for initialization
	
	
	// Update is called once per frame
	void Update () {
        
        if (shoot == true)
        {
            //If raycast doesn't hit anything , grapple has a predefined hit point 
            if (dist_pred == 15f)
            {

                Vector3 hitpoint = pl.transform.GetChild(1).transform.position + pl.transform.GetChild(1).transform.rotation * new Vector3(0, 0, dist_pred);
                transform.position = Vector3.Lerp(transform.position, hitpoint, Time.fixedDeltaTime * 9f);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, grappleHit.point , Time.fixedDeltaTime * 9f);
            }
               
            if (inAir == true )
            {
                Vector3 Dist = pl.transform.position  - this.transform.position;
                
                if (Dist.magnitude >= 15f)
                {
                    returngrapple = true;
                    shoot = false;
                }
            }


        }
        
        
        if (returngrapple == true)
        {
            
            transform.position = Vector3.Lerp(transform.position, pl.transform.GetChild(1).transform.position, Time.fixedDeltaTime * 12f);
        }

        

    }
}

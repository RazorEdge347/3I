using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitbox : MonoBehaviour {
    private GameObject pl;
    private Renderer re;
    private bool color_a;
    private bool color_h;
    public Material[] material;
	// Use this for initialization
	void Start () {
        pl = GameObject.Find("PL");
        re = GetComponent<Renderer>();
        re.sharedMaterial = material[0];
	}
	
	// Update is called once per frame
	void Update () {
        /*color_a = pl.GetComponent<PLController>().get_attack_ani();
        color_h = pl.GetComponent<PLController>().get_Hattack_ani();*/

        
       

        if(color_a == true)
        {
            re.sharedMaterial = material[1];
        }
        
        
        if(color_h == true)
        {
            re.sharedMaterial = material[2];
        }
        

        //        transform.position = pl.transform.forward;

    }
}

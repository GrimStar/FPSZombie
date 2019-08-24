using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour {
    public Animator anim;
    private float speed = 0f;
    public SpeedControl speedScript;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(speedScript == null || anim == null)
        {
            return;
        }
        if (speedScript.crouching)
        {
            anim.SetBool("crouch", true);
        }
        if (!speedScript.crouching)
        {
            anim.SetBool("crouch", false);
        }
        
        speed = speedScript.speedMultiplier * Input.GetAxis("Vertical");
        anim.SetFloat("velocityZ", speed);
		
	}
}

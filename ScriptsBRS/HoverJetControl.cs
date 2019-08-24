using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverJetControl : MonoBehaviour {
    Rigidbody rb;
    [SerializeField]
    float acceleration = 20f;
    [SerializeField]
    float verticalAcceleration = 30f;
    [SerializeField]
    float maxForce = 100f;
    [SerializeField]
    float turnSpeed = 10f;
    [SerializeField]
    float antiGravityForce = 10f;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
       
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Motor();
	}
    void Motor()
    {
        rb.AddForce(transform.up * antiGravityForce);
        float zAxis = Input.GetAxis("Vertical");
        float xAxis = Input.GetAxis("Horizontal");
        rb.AddForce(transform.forward * zAxis * acceleration);
        rb.AddForce(transform.right * acceleration * xAxis);
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(transform.up * verticalAcceleration);
        }
        float yAxis = Input.GetAxis("Mouse X");
        Quaternion yRot = Quaternion.Euler(0, yAxis * turnSpeed, 0);
        Quaternion rot = Quaternion.Euler(rb.transform.rotation.eulerAngles.x, rb.transform.rotation.eulerAngles.y + yRot.y, rb.transform.rotation.eulerAngles.z);
        rb.MoveRotation(rot);
    }
}

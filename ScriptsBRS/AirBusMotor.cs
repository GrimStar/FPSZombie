using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBusMotor : MonoBehaviour {
    public Rigidbody rb;
    [SerializeField]
    public float currentThrust = 50f;
    public float takeOffThrust = 40f;
    public float horizontalThrust = 20f;
    public float takeOffAcceleration = 10f;
    public float deAcceleration = 5f;
    public float upThrust = 12f;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	
    public void AddForceDirection(Vector3 _direction)
    {
        rb.AddForce(_direction * currentThrust);
    }
    public void AddUpwardsThrust()
    {
        rb.AddForce(transform.up * upThrust);
    }
}

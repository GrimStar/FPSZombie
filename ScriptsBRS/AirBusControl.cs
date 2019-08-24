using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBusControl : MonoBehaviour {

    bool forward = true;
    [SerializeField]
    Transform[] waypoints;
    [SerializeField]
    Transform[] backwardsWaypoints;
    [SerializeField]
    Transform[] landingPads;
    Transform closestWaypoint;
    [SerializeField]
    int currentDestination = 0;
    AirBusMotor motor;
    bool takeOff = false;
    [SerializeField]
    float cycleDistance = 10f;
    public bool landing = false;
    public bool landed = false;
    bool horizontalFlight = false;
    [SerializeField]
    float postTakeOffDelay = 2f;
    [SerializeField]
    float preLandingDelay = 4f;
    [SerializeField]
    Transform groundCheckObject;
    [SerializeField]
    float landTime = 10f;
    bool landingMode = false;
	// Use this for initialization
	void Start () {
        motor = GetComponent<AirBusMotor>();
        FindClosestWaypoint();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!landed)
        {
            if (!landingMode)
            {
                CheckDistance();
            }
            CheckDestinationDirection();
            
        }
        
        ControlThrust();
	}
    void ControlThrust()
    {
        if (landed)
        {
            motor.currentThrust = 0f;
        }
        if (takeOff)
        {
            if(motor.currentThrust < motor.takeOffThrust)
            {
                motor.currentThrust += Time.time * motor.takeOffAcceleration;
            }
            else
            {
                motor.currentThrust = motor.takeOffThrust;
            }
        }
        if (landing)
        {
            if(motor.currentThrust > 2f)
            {
                motor.currentThrust -= Time.time * motor.deAcceleration;
            }
            else
            {
                motor.currentThrust = 2f;
            }
            if (!landed)
            {
                CheckDistanceFromGround();
            }
        }
        if (horizontalFlight)
        {
            motor.currentThrust = motor.horizontalThrust;
            CheckAltitude();
        }
    }
    void CheckDestinationDirection()
    {
        Vector3 dir = Vector3.zero;
        if (forward)
        {
            dir = waypoints[currentDestination].position - transform.position;


        }
        else
        {
            dir = backwardsWaypoints[currentDestination].position - transform.position;
        }
        motor.AddForceDirection(dir);


    }
    void CheckDistanceFromGround()
    {
        RaycastHit hit;
        if(Physics.Raycast(groundCheckObject.position, -transform.up, out hit, 0.25f))
        {
            landing = false;
            landed = true;
            StartCoroutine(LandTime());
        }
    }
    void FindClosestWaypoint()
    {
        float shortestDist = 9999f;
        int i = 0;
        Transform[] currentPath = waypoints;
        if (forward)
        {
            currentPath = waypoints;
        }
        else
        {
            currentPath = backwardsWaypoints;
        }
        foreach(Transform _waypoint in currentPath)
        {
            float dist = Vector3.Distance(transform.position, _waypoint.position);
            if(dist < shortestDist)
            {
                shortestDist = dist;
                closestWaypoint = _waypoint;
                currentDestination = i;

            }
            i++;

        }
        TakeOff();
    }
    void CheckDistance()
    {
        Transform[] currentPath = waypoints;
        if (forward)
        {
            currentPath = waypoints;
        }
        else
        {
            currentPath = backwardsWaypoints;
        }
        float dist = Vector3.Distance(transform.position, currentPath[currentDestination].position);
     
        if(dist < cycleDistance)
        {
            CycleDestination();
        }
    }
    void CycleDestination()
    {
        Transform[] currentPath = waypoints;
        if (forward)
        {
            currentPath = waypoints;
        }
        else
        {
            currentPath = backwardsWaypoints;
        }
        
        if (currentDestination < currentPath.Length - 1)
        {
            if (currentDestination == 0)
            {
                StartCoroutine(PostTakeOffDelay());
            }
            currentDestination++;
            
        }
        else
        {
            if (currentDestination == currentPath.Length - 1)
            {
                landingMode = true;
                StartCoroutine(PreLandingDelay());
            }
        }
        
    }
    void CheckAltitude()
    {
        Debug.Log("Checkal");
        Transform[] currentPath = waypoints;
        if (forward)
        {
            currentPath = waypoints;
        }
        else
        {
            currentPath = backwardsWaypoints;
        }
        if (transform.position.y < currentPath[currentDestination].position.y)
        { 
            Debug.Log("Up");
            motor.AddUpwardsThrust();
        }
    }
    void TakeOff()
    {
        takeOff = true;
    }
    IEnumerator PostTakeOffDelay()
    {
        yield return new WaitForSeconds(postTakeOffDelay);
        takeOff = false;
        horizontalFlight = true;
    }
    IEnumerator PreLandingDelay()
    {
        yield return new WaitForSeconds(preLandingDelay);
        horizontalFlight = false;
        landing = true;
    }
    IEnumerator LandTime()
    {
        yield return new WaitForSeconds(landTime);
        landed = false;
        landingMode = false;
        if (forward)
        {
            forward = false;
        }
        else
        {
            forward = true;
        }
        FindClosestWaypoint();
    }
}

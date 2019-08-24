using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NPCBasic1 : MonoBehaviour {

    NavMeshAgent agent;
    [SerializeField]
    float speed = 2f;
    [SerializeField]
    float speedMulti = 1f;
    [SerializeField]
    Transform player;
    NPCHealthMC _health;
    Rigidbody rb;
    [SerializeField]
    float torque = 5f;
    [SerializeField]
    float counterFallForce = 9f;
    [SerializeField]
    float counterForceDropAmount = 2f;
    bool dormant = false;
    [SerializeField]
    AudioClip[] clips;
    [SerializeField]
    AudioSource _source;
    [SerializeField]
    AudioSource _mainSource;
	// Use this for initialization
	void Start () {
       
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        _health = GetComponent<NPCHealthMC>();
        agent.SetDestination(player.position);
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (dormant)
        {
            Destroy(this);
        }
        if (_health.dead) {
            speedMulti = 0f;
            agent.enabled = false;
            if(rb.useGravity == false)
            {
                _mainSource.Stop();
                _source.clip = clips[0];
                _source.Play();
                rb.AddTorque(transform.forward * torque, ForceMode.Impulse);
                rb.AddForce(-transform.forward * 3, ForceMode.Impulse);
            }
            rb.useGravity = true;
            if (counterFallForce > 0)
            {
                rb.AddForce(transform.up * counterFallForce);
                counterFallForce -= Time.deltaTime * counterForceDropAmount;
            }
            else
            {
                dormant = true;
            }
            
        }
        agent.speed = speed * speedMulti;
        
	}
}

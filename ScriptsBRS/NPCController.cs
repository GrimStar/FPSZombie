using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NPCController : MonoBehaviour {
    [SerializeField]
    private float speed = 1;
    private float actualSpeed = 0;

    public Animator anim;
    public NavMeshAgent agent;
    public Transform[] destinations;
    int destinationCount;
    public bool run = false;
    public bool idle = true;
    private Rigidbody rb;
    public bool defaultState = false;
    public bool alertState = false;
    private bool dead = false;
    public AudioSource scream;
    public GameObject player;

    public float speedMulti;

    public Kalagaan.BlendShapesPresetTool.BlendShapesPresetControllerBase m_bspm;
    bool screaming = false;
    // Use this for initialization
    void Start () {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        ChangeDestination();
	}
	
	// Update is called once per frame
	void Update () {
        AnimatorClipInfo[] clip = anim.GetCurrentAnimatorClipInfo(0);
        Debug.Log(clip[0].clip.name.ToString());
        if(clip[0].clip.name.ToString() == "Zombie Scream")
        {
            m_bspm.SetWeight(0, 1);
            if (!screaming)
            {
                screaming = true;
                scream.Play();
            }
        }
        else
        {
            screaming = false;
            m_bspm.SetWeight(0, 0);
        }
        //Debug.Log(rb.angularVelocity.sqrMagnitude.ToString());
        agent.speed = actualSpeed;
        if (run)
        {
            actualSpeed = speed * speedMulti;
            anim.SetFloat("speed", 1);
        }
        else if (idle)
        {
            actualSpeed = 0;
            anim.SetFloat("speed", 0);
        }
        else
        {
            actualSpeed = speed / 2;
            anim.SetFloat("speed", 0.5f);
        }
        float distance = Vector3.Distance(transform.position, agent.destination);
        
        if(distance < 1 && defaultState)
        {
            ChangeDestination();
        }
        CheckPlayerDistance();

        if (dead)
        {
            ShrinkCollider();
        }
	}
    void ShrinkCollider()
    {
        if (agent.height > 0.25f)
        {
            agent.height -= 0.3f * Time.deltaTime;
            
        }
        if(agent.baseOffset > -0.8)
        {
            agent.baseOffset -= 0.3f * Time.deltaTime;
        }
    }
    void CheckPlayerDistance()
    {
        float playerDistance = Vector3.Distance(transform.position, player.transform.position);
        if (playerDistance < 30)
        {
            agent.SetDestination(player.transform.position);
            if (!alertState)
            {
                Alert();
            }
        }
        else
        {
            if (!defaultState)
            {
                alertState = false;
                defaultState = true;
            }
        }
    }
    public void ChangeDestination()
    {
        if (destinationCount >= destinations.Length)
        {
            destinationCount = 0;
            
        }
        agent.SetDestination(destinations[destinationCount].position);
        destinationCount++;
    }
    public void Die()
    {
        dead = true;
        GetComponent<CapsuleCollider>().enabled = false;
        speed = 0;
        anim.SetBool("Dead", true);
    }
    public void Alert()
    {
        alertState = true;
        anim.SetBool("Scream", true);
        
        idle = true;
        defaultState = false;
        
        
        
        StartCoroutine(ScreamTimer());
        
    }
    IEnumerator ScreamTimer()
    {
        yield return new WaitForSeconds(2);
        anim.SetBool("Scream", false);
        idle = false;
        run = true;
    }
}

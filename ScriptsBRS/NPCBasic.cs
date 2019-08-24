using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NPCBasic : NPCMasterClass {
    [SerializeField]
    Transform player;
    
    [SerializeField]
    LayerMask mask;
    [SerializeField]
    float reactionTime = 1f;
    

    bool checking = false;
    bool inDanger = false;
    bool takingCover = false;
    bool returningToCover = false;
    bool reachedCover = false;
    Vector3 coverPosition = Vector3.zero;
	// Use this for initialization
	void Start () {
        mask = ~mask;
        agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!inDanger && !takingCover)
        {
            agent.SetDestination(player.position);
        }
        if (agent.remainingDistance > 0.2f && !takingCover)
        {
            agent.speed = 2f;
        }
        if(agent.remainingDistance < 0.2f)
        {
            agent.speed = 0f;
        }
        if(CanSee(player.gameObject, transform.position) && !checking && !takingCover)
        {
            
            checking = true;
            StartCoroutine(checkcooldown());
            StartCoroutine(FindCoverReaction());
            


        }
        if (takingCover)
        {
            if(agent.remainingDistance < 0.2f)
            {
                //check if player is aiming at them
                reachedCover = true;
                returningToCover = false;
                agent.speed = 0f;
            }
            float playerLOS = Vector3.Angle(player.transform.forward, transform.position - player.position);
            
            if (playerLOS > 20f && !CanSee(player.gameObject, transform.position))
            {
                inDanger = false;
                agent.speed = 1;
                agent.SetDestination(player.position);
            }
            if (playerLOS > 20f && CanSee(player.gameObject, transform.position) && reachedCover)
            {
                inDanger = false;
                agent.speed = 0;
                agent.SetDestination(player.position);
            }
            if (playerLOS > 20f && CanSee(player.gameObject, transform.position) && !reachedCover)
            {
                inDanger = false;
                agent.speed = 1f;
                agent.SetDestination(coverPosition);
            }
            if (playerLOS < 20f && !CanSee(player.gameObject, transform.position))
            {
                inDanger = true;
                agent.speed = 0;
                agent.SetDestination(coverPosition);
            }
            if (playerLOS < 20f && CanSee(player.gameObject, transform.position) && !returningToCover)
            {
                returningToCover = true;
                StartCoroutine(ReturnToCoverReaction());
            }
            



        }
        if(!CanSee(player.gameObject, transform.position))
        {
            
        }
	}
    void ReturnToCover()
    {

    }
    void CheckForCover()
    {
        if (CanSee(player.gameObject, transform.position))
        {
            inDanger = true;
            reachedCover = false;
            NPCAreaDetection detection = GetComponent<NPCAreaDetection>();
            List<Vector3> possibleCoverPositions = detection.CheckForCover(player.gameObject, mask);
            float dist = 999;
            Vector3 position = Vector3.zero;
            if (possibleCoverPositions.Count == 0f)
            {
                //attack
                agent.SetDestination(transform.position);
            }
            else
            {
                foreach (Vector3 spot in possibleCoverPositions)
                {
                    float _dist = Vector3.Distance(spot, transform.position);
                    if (_dist < dist)
                    {
                        dist = _dist;
                        position = spot;

                    }
                }
                agent.SetDestination(position);
                coverPosition = position;
                takingCover = true;
            }



        }
    }
    IEnumerator checkcooldown()
    {
        yield return new WaitForSeconds(2);
        checking = false;
    }
    IEnumerator FindCoverReaction()
    {
        yield return new WaitForSeconds(reactionTime);
        CheckForCover();

    }
    IEnumerator ReturnToCoverReaction()
    {
        yield return new WaitForSeconds(reactionTime);
        inDanger = true;
        agent.speed = 1;
        agent.SetDestination(coverPosition);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NPCZombieHerd : MonoBehaviour {
    List<GameObject> Herd = new List<GameObject>();
    NPCZombie zombie;
    bool aggroHerd = false;
	// Use this for initialization
	void Start () {
        zombie = GetComponent<NPCZombie>();
	}
	
	// Update is called once per frame
	void Update () {
        if (zombie.aggro)
        {
            if (!aggroHerd)
            {
                AggroHerd();
            }

        }
        else
        {
            aggroHerd = false;
        }
	}
    void AggroHerd()
    {
        foreach(GameObject _zombie in Herd)
        {
            _zombie.GetComponent<NPCZombie>().aggro = true;
            _zombie.GetComponent<NavMeshAgent>().SetDestination(_zombie.GetComponent<NPCZombie>().player.transform.position);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if(other.transform.GetComponent<NPCZombie>() != null)
        {
            Herd.Add(other.transform.gameObject);
            foreach (GameObject zombie in Herd)
            {
                Debug.Log(zombie.name.ToString());
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.GetComponent<NPCZombie>() != null)
        {
            Herd.Remove(other.transform.gameObject);
            foreach (GameObject zombie in Herd)
            {
                Debug.Log(zombie.name.ToString());
            }
        }
    }
}

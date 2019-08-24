using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSWATControl : MonoBehaviour {

    public Transform leftFront;
    public Transform leftMiddle;
    public Transform leftBack;
    public Transform rightFront;
    public Transform rightMiddle;
    public Transform rightBack;

    public List<Transform> enemiesLeftFront = new List<Transform>();
    public List<Transform> enemiesLeftMiddle = new List<Transform>();
    public List<Transform> enemiesLeftBack = new List<Transform>();
    public List<Transform> enemiesRightFront = new List<Transform>();
    public List<Transform> enemiesRightMiddle = new List<Transform>();
    public List<Transform> enemiesRightBack = new List<Transform>();
    private List<Transform> deadEnemies = new List<Transform>();
    public List<List<Transform>> EnemyLists = new List<List<Transform>>();
    public List<Transform> enemies = new List<Transform>();
    public Transform Player;
    public Transform closestEnemy;
    public Transform testEnemy;
    public static NPCSWATControl instance;
	// Use this for initialization
	void Start () {
        EnemyLists.Add(enemiesLeftFront);
        EnemyLists.Add(enemiesLeftBack);
        EnemyLists.Add(enemiesLeftMiddle);
        EnemyLists.Add(enemiesRightFront);
        EnemyLists.Add(enemiesRightMiddle);
        EnemyLists.Add(enemiesRightBack);
        
        if(instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
		
	}
	
	// Update is called once per frame
	void Update () {
        
        transform.position = Player.position;

        EvaluateEnemies();
        
	}
    void EvaluateEnemies()
    {
        float shortDist = 999f;
        foreach(Transform _enemy in enemies)
        {
            if (!_enemy.root.transform.GetComponent<NPCHealthMC>().dead)
            {
                float dist = Vector3.Distance(_enemy.position, transform.position);
                if (dist < shortDist)
                {
                    closestEnemy = _enemy;
                    shortDist = dist;
                }
            }
            else
            {
                deadEnemies.Add(_enemy);
            }
            
        }
        foreach(Transform _dead in deadEnemies)
        {
            enemies.Remove(_dead);
        }
        deadEnemies.Clear();
        GameData.instance.closestEnemy = closestEnemy;
        
    }
    public List<Transform> TryGetEnemy()
    {
        bool foundList = false;
        List<Transform> temp = null;
        foreach(List<Transform> _list in EnemyLists)
        {
            if(_list.Count > 0)
            {
                foundList = true;
                temp = _list;
            }
            else
            {
                continue;
            }
        }
        if (!foundList)
        {
            return null;
        }
        else
        {
            return temp;
        }
        
    }
    void AddToList(Transform _object)
    {
        if (transform.position.z < _object.position.z)
        {
            
            if (transform.position.x < _object.position.x)
            {
                if ((transform.position.z - _object.position.z) > -2 && (transform.position.z - _object.position.z) < 2)
                {
                    
                    enemiesRightMiddle.Add(_object);
                    NPCSwatTeam NPCScript = rightMiddle.GetComponent<NPCSwatTeam>();
                    NPCScript.enemies = enemiesRightMiddle;
                    NPCScript.CheckClosestEnemy();
                }
                else
                {
                    
                    enemiesRightFront.Add(_object);
                    NPCSwatTeam NPCScript = rightFront.GetComponent<NPCSwatTeam>();
                    NPCScript.enemies = enemiesRightFront;
                    NPCScript.CheckClosestEnemy();
                }

            }
            else
            {
                if ((transform.position.z - _object.position.z) > -2 && (transform.position.z - _object.position.z) < 2)
                {
                    
                    enemiesLeftMiddle.Add(_object);
                    NPCSwatTeam NPCScript = leftMiddle.GetComponent<NPCSwatTeam>();
                    NPCScript.enemies = enemiesLeftMiddle;
                    NPCScript.CheckClosestEnemy();
                }
                else
                {
                    
                    enemiesLeftFront.Add(_object);
                    NPCSwatTeam NPCScript = leftFront.GetComponent<NPCSwatTeam>();
                    NPCScript.enemies = enemiesLeftFront;
                    NPCScript.CheckClosestEnemy();
                }
            }
        }
        else
        {
            Debug.Log("Back");
            if (transform.position.x < _object.position.x)
            {
                if ((transform.position.z - _object.position.z) > -2 && (transform.position.z - _object.position.z) < 2)
                {
                    
                    enemiesRightMiddle.Add(_object);
                    NPCSwatTeam NPCScript = rightMiddle.GetComponent<NPCSwatTeam>();
                    NPCScript.enemies = enemiesRightMiddle;
                    NPCScript.CheckClosestEnemy();
                }
                else
                {
                    
                    enemiesRightBack.Add(_object);
                    NPCSwatTeam NPCScript = rightBack.GetComponent<NPCSwatTeam>();
                    NPCScript.enemies = enemiesRightBack;
                    NPCScript.CheckClosestEnemy();
                }
            }
            else
            {
                if ((transform.position.z - _object.position.z) > -2 && (transform.position.z - _object.position.z) < 2)
                {
                   
                    enemiesLeftMiddle.Add(_object);
                    NPCSwatTeam NPCScript = leftMiddle.GetComponent<NPCSwatTeam>();
                    NPCScript.enemies = enemiesLeftMiddle;
                    NPCScript.CheckClosestEnemy();
                }
                else
                {
                    
                    enemiesLeftBack.Add(_object);
                    NPCSwatTeam NPCScript = leftBack.GetComponent<NPCSwatTeam>();
                    NPCScript.enemies = enemiesLeftBack;
                    NPCScript.CheckClosestEnemy();
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "enemy")
        {
            Debug.Log("addtolist");
            if (other.transform.parent == null)
            {
                enemies.Add(other.transform);
                //AddToList(other.transform);
            }  
        }
    }
    private void OnTriggerExit(Collider other)
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoldierSquadControl : MonoBehaviour {

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

    public List<List<Transform>> EnemyLists = new List<List<Transform>>();

    public Transform Player;

    public Transform testEnemy;
    public static EnemySoldierSquadControl instance;
    // Use this for initialization
    void Start()
    {
        EnemyLists.Add(enemiesLeftFront);
        EnemyLists.Add(enemiesLeftBack);
        EnemyLists.Add(enemiesLeftMiddle);
        EnemyLists.Add(enemiesRightFront);
        EnemyLists.Add(enemiesRightMiddle);
        EnemyLists.Add(enemiesRightBack);

        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

    }

    // Update is called once per frame
    void Update()
    {

        transform.position = Player.position;



    }
    void EvaluateEnemies()
    {


    }
    public List<Transform> TryGetEnemy()
    {
        bool foundList = false;
        List<Transform> temp = null;
        foreach (List<Transform> _list in EnemyLists)
        {
            if (_list.Count > 0)
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
                    NPCEnemySoldier NPCScript = rightMiddle.GetComponent<NPCEnemySoldier>();
                    NPCScript.enemies = enemiesRightMiddle;
                    NPCScript.CheckClosestEnemy();
                }
                else
                {

                    enemiesRightFront.Add(_object);
                    NPCEnemySoldier NPCScript = rightFront.GetComponent<NPCEnemySoldier>();
                    NPCScript.enemies = enemiesRightFront;
                    NPCScript.CheckClosestEnemy();
                }

            }
            else
            {
                if ((transform.position.z - _object.position.z) > -2 && (transform.position.z - _object.position.z) < 2)
                {

                    enemiesLeftMiddle.Add(_object);
                    NPCEnemySoldier NPCScript = leftMiddle.GetComponent<NPCEnemySoldier>();
                    NPCScript.enemies = enemiesLeftMiddle;
                    NPCScript.CheckClosestEnemy();
                }
                else
                {

                    enemiesLeftFront.Add(_object);
                    NPCEnemySoldier NPCScript = leftFront.GetComponent<NPCEnemySoldier>();
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
                    NPCEnemySoldier NPCScript = rightMiddle.GetComponent<NPCEnemySoldier>();
                    NPCScript.enemies = enemiesRightMiddle;
                    NPCScript.CheckClosestEnemy();
                }
                else
                {

                    enemiesRightBack.Add(_object);
                    NPCEnemySoldier NPCScript = rightBack.GetComponent<NPCEnemySoldier>();
                    NPCScript.enemies = enemiesRightBack;
                    NPCScript.CheckClosestEnemy();
                }
            }
            else
            {
                if ((transform.position.z - _object.position.z) > -2 && (transform.position.z - _object.position.z) < 2)
                {

                    enemiesLeftMiddle.Add(_object);
                    NPCEnemySoldier NPCScript = leftMiddle.GetComponent<NPCEnemySoldier>();
                    NPCScript.enemies = enemiesLeftMiddle;
                    NPCScript.CheckClosestEnemy();
                }
                else
                {

                    enemiesLeftBack.Add(_object);
                    NPCEnemySoldier NPCScript = leftBack.GetComponent<NPCEnemySoldier>();
                    NPCScript.enemies = enemiesLeftBack;
                    NPCScript.CheckClosestEnemy();
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            
            if (other.transform.parent == null)
            {
                AddToList(other.transform);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {

    }
}

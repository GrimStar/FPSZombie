using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NPCSWAT : MonoBehaviour {
    [SerializeField]
    float speed = 2f;
    [SerializeField]
    float shotInterval = 0.2f;
    public Transform targetPosition;
    public Transform enemyTarget;
    public NavMeshAgent agent;
    public List<Transform> enemies;
    public List<Transform> deadEnemies = new List<Transform>();
    public Animator anim;
    bool attacking = false;
    [SerializeField]
    Transform rayCaster;
    Quaternion origRotation;
    [SerializeField]
    float idleRange = 0.025f;
    [SerializeField]
    AudioSource shot;
    [SerializeField]
    GameObject Bullet;
    [SerializeField]
    GameObject bulletSpawn;
    [SerializeField]
    float catchUpRange = 0.1f;
    [SerializeField]
    float catchUpMultiplier = 1.25f;
    [SerializeField]
    ParticleSystem flash;
	// Use this for initialization
	void Start () {
        shot = GetComponent<AudioSource>();
        origRotation = transform.rotation;
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 0f;
        agent.SetDestination(targetPosition.position);
	}
	
	// Update is called once per frame
	void Update () {
        agent.updateRotation = false;
        NavMeshHit hit;
        if(!agent.SamplePathPosition(0, 1f, out hit))
        {
            //goidle
            //cycle Position
            
            agent.speed = 0f;
        }
        else
        {
            //Keep going to current Target
            
            agent.SetDestination(targetPosition.position);
            float axis = Input.GetAxis("Vertical");
            if (agent.remainingDistance < idleRange)
            {
                agent.speed = speed * axis;
                
                anim.SetFloat("Speed", 1 * axis);
            }
            else
            {
                agent.speed = speed * catchUpMultiplier;
                anim.SetFloat("Speed", 1);
            }
           
        }
        if(enemyTarget != null)
        {

            RotateTowardsTarget();
            if (enemyTarget.GetComponent<NPCHealthMC>().dead)
            {
                anim.SetBool("Shooting", false);
                enemyTarget = null;
            }
        }
        if(enemyTarget == null)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, origRotation, 0.5f);
        }
        CheckDestinationAngle();
        if(enemies.Count > 0)
        {
            CheckClosestEnemy();
        }
        if(enemies.Count <= 0)
        {
            if(enemyTarget != null)
            {
                enemyTarget = null;
            }
            List<Transform> _list = NPCSWATControl.instance.TryGetEnemy();
            if(_list != null)
            {
                enemies = _list;
            }
        }
        agent.SetDestination(targetPosition.position);
	}
    void RotateTowardsTarget()
    {
        
        Vector3 _dir = enemyTarget.position - transform.position;
        RaycastHit hit;
        if (Physics.Raycast(rayCaster.position, _dir, out hit, 100f))
        {
            if (hit.transform.tag == "enemy")
            {
                Vector3 dir = enemyTarget.position - transform.position;
                Vector3 bulletSpawnDir = Vector3.RotateTowards(transform.forward, dir, 5, 5);
                dir.y = 0;
                Vector3 bodyDir = Vector3.RotateTowards(transform.forward, dir, 5, 5);                
                transform.rotation = Quaternion.LookRotation(bodyDir);
                if (Vector3.Angle(transform.forward, _dir) < 25f)
                {
                    anim.SetBool("Shooting", true);
                    bulletSpawn.transform.rotation = Quaternion.LookRotation(bulletSpawnDir);
                    if (!attacking)
                    {
                        StartCoroutine(AttackCooldown());
                        attacking = true;
                    }
                }
                else
                {
                    anim.SetBool("Shooting", false);
                }
            }
        }
    }
    IEnumerator AttackCooldown()
    {
        Shoot();
        yield return new WaitForSeconds(shotInterval);
        attacking = false;
    }
    public void Shoot()
    {
        if (enemyTarget != null)
        {
            flash.Play();
            GameObject _bullet = (GameObject)Instantiate(Bullet);
            _bullet.transform.position = bulletSpawn.transform.position;
            _bullet.transform.rotation = bulletSpawn.transform.rotation;
            
        }
    }
    public void CheckClosestEnemy()
    {
        float shortestDist = 9999f;
        Transform closestEnemy = null;
        foreach(Transform _enemy in enemies)
        {
            if (!_enemy.root.transform.GetComponent<NPCHealthMC>().dead)
            {
                float dist = Vector3.Distance(_enemy.position, transform.position);
                if (dist < shortestDist)
                {
                    shortestDist = dist;
                    closestEnemy = _enemy.transform;
                }
            }
            else
            {
                deadEnemies.Add(_enemy);
            }
            
        }
        foreach(Transform _enemy in deadEnemies)
        {
            enemies.Remove(_enemy);
        }
        if(deadEnemies.Count > 0)
        {
            deadEnemies.Clear();
        }
        if(closestEnemy != null)
        {
            enemyTarget = closestEnemy;
        }
        else
        {
            enemyTarget = null;
        }
    }
    void CheckDestinationAngle()
    {
        Vector3 dir = targetPosition.position - transform.position;
        float axisZ = Vector3.Angle(transform.forward, dir);
        float axisX = Vector3.Angle(transform.right, dir);

        if(axisZ < 45)
        {
            //axisZ = 1f;
        }
        else if(axisZ > 135)
        {
            //axisZ = -1;
        }
        else
        {
            //axisZ = 0;
        }
        if(axisX < 45)
        {
            axisX = 1;
        }
        if(axisX > 135)
        {
            axisX = -1;
        }
        if(axisX <= 135 && axisX >= 45)
        {
            axisX = 0;
        }
        axisZ = 0;
        anim.SetFloat("axisZ", axisZ);
        anim.SetFloat("axisX", axisX);
    }
}

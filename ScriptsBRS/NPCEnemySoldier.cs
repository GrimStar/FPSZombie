using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NPCEnemySoldier : NPCMasterClass {

    [SerializeField]
    float shotInterval = 0.2f;
    public Transform targetPosition;
    public Transform enemyTarget;
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
    bool shooting = false;
    bool targetInSight = false;
    [SerializeField]
    ParticleSystem flash;
    [SerializeField]
    float rotationSpeed = 1f;
    bool takingDamage = false;
    bool dead = false;

    private void Start()
    {
        shot = GetComponent<AudioSource>();
        origRotation = transform.rotation;
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 0f;
        agent.SetDestination(targetPosition.position);
    }
    private void Update()
    {
        agent.updateRotation = false;
        NavMeshHit hit;
        if (!agent.SamplePathPosition(0, 1f, out hit))
        {
            //look for another targetposition
            agent.speed = 0f;
        }
        else
        {
            //Keep going to current Target

            agent.SetDestination(targetPosition.position);
            
            if (agent.remainingDistance < idleRange)
            {
                agent.speed = speed;

                anim.SetFloat("Speed", 1);
            }
            else
            {
                agent.speed = speed * catchUpMultiplier;
                anim.SetFloat("Speed", 1);
            }

        }
        if (enemyTarget != null)
        {
            if (enemyTarget.GetComponent<NPCHealthMC>().dead)
            {
                enemyTarget = null;
                return;
            }
            if (CanSeeTarget(enemyTarget.GetComponent<NPCBodyParts>().bodyParts[1].position, rayCaster.position))
            {
                Debug.Log("CanSee");
                targetInSight = true;
                RotateTowardsTarget();
            }
            if (!CanSeeTarget(enemyTarget.GetComponent<NPCBodyParts>().bodyParts[1].position, rayCaster.position))
            {
                Debug.Log("!CanSee");
                shooting = false;
            }
        }
        if (enemyTarget == null)
        {
            shooting = false;
            transform.rotation = Quaternion.Lerp(transform.rotation, origRotation, rotationSpeed * Time.deltaTime);
        }
        if (shooting)
        {
            anim.SetBool("Shooting", true);
            Shoot();
        }
        if (!shooting)
        {
            anim.SetBool("Shooting", false);
        }
        CheckDestinationAngle();
        if (enemies.Count > 0)
        {
            CheckClosestEnemy();
        }
        if (enemies.Count <= 0)
        {
            if (enemyTarget != null)
            {
                enemyTarget = null;
            }
            List<Transform> _list = EnemySoldierSquadControl.instance.TryGetEnemy();
            if (_list != null)
            {
                enemies = _list;
            }
        }
        takingDamage = GetComponent<NPCHealthMC>().takingDamage;
        dead = GetComponent<NPCHealthMC>().dead;
        if (takingDamage)
        {
            TakingDamage();
        }
        if (dead)
        {
            Die();
        }
        agent.SetDestination(targetPosition.position);

    }
    public void Die()
    {
        dead = true;
        run = false;
        walk = false;
        idle = false;
        speedMultiplier = 0;
        velocityZTarget = 0f;
        agent.speed = 0f;
        anim.SetTrigger("Dead");
    }
    void TakingDamage()
    {
        if (run)
        {
            
        }
    }
    protected override void SetDestination()
    {

    }
    private bool CanSeeTarget(Vector3 target, Vector3 _npc)
    {

        RaycastHit hit;
        Vector3 dir = target - _npc;
        if (Physics.Raycast(_npc, dir, out hit, 100f))
        {
            //Debug.DrawLine(_npc, hit.point, Color.green, 5f);
            if (hit.transform.tag == "Player")
            {
                return true;
            }
            else
            {
                
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    void CheckDestinationAngle()
    {
        Vector3 dir = targetPosition.position - transform.position;
        float axisZ = Vector3.Angle(transform.forward, dir);
        float axisX = Vector3.Angle(transform.right, dir);

        if (axisZ < 45)
        {
            //axisZ = 1f;
        }
        else if (axisZ > 135)
        {
            //axisZ = -1;
        }
        else
        {
            //axisZ = 0;
        }
        if (axisX < 45)
        {
            axisX = 1;
        }
        if (axisX > 135)
        {
            axisX = -1;
        }
        if (axisX <= 135 && axisX >= 45)
        {
            axisX = 0;
        }
        axisZ = 0;
        anim.SetFloat("axisZ", axisZ);
        anim.SetFloat("axisX", axisX);
    }
    public void CheckClosestEnemy()
    {
        float shortestDist = 9999f;
        Transform closestEnemy = null;
        foreach (Transform _enemy in enemies)
        {
            if (!_enemy.GetComponent<NPCHealthMC>().dead)
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
        foreach (Transform _enemy in deadEnemies)
        {
            enemies.Remove(_enemy);
        }
        if (deadEnemies.Count > 0)
        {
            deadEnemies.Clear();
        }
        if (closestEnemy != null)
        {
            enemyTarget = closestEnemy;
        }
        else
        {
            enemyTarget = null;
        }
    }

    void RotateTowardsTarget()
    {

        Vector3 _dir = enemyTarget.position - transform.position;
        Vector3 dir = enemyTarget.position - transform.position;
        Vector3 bulletSpawnDir = Vector3.RotateTowards(transform.forward, dir, 5, 5);
        dir.y = 0;
        Vector3 bodyDir = Vector3.RotateTowards(transform.forward, dir, 5, 5);
        transform.rotation = Quaternion.LookRotation(bodyDir * rotationSpeed * Time.deltaTime);
        if (Vector3.Angle(transform.forward, _dir) < 25f)
        {
            shooting = true;
            bulletSpawn.transform.rotation = Quaternion.LookRotation(bulletSpawnDir * rotationSpeed * Time.deltaTime);

        }

    }
    void Shoot()
    {

        if (!attacking)
        {
            StartCoroutine(AttackCooldown());
            attacking = true;
        }
    }
    IEnumerator AttackCooldown()
    {
        if (enemyTarget != null)
        {
            GameObject _bullet = (GameObject)Instantiate(Bullet);
            _bullet.transform.position = bulletSpawn.transform.position;
            _bullet.transform.rotation = bulletSpawn.transform.rotation;
            flash.Play();

        }
        yield return new WaitForSeconds(shotInterval);
        attacking = false;
    }
}

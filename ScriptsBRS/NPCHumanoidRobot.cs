using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCHumanoidRobot : NPCMasterClass
{

    [SerializeField]
    public bool lastSpawn = false;
    [SerializeField]
    Transform[] zombieDestinations;
    [SerializeField]
    public Transform[] destinations;

    [SerializeField]
    public GameObject player;
    Vector3 playerPosition;
    [SerializeField]
    float aggroRange = 20;
    [SerializeField]
    float aggroCooldown = 5;
    float aggroTimer = 0;
    public bool aggro = false;
    bool attacking = false;
    public Animator anim;
    [SerializeField]
    float attackCooldown = 1;
    public LayerMask mask;
    [SerializeField]
    float attackRayLength = 2f;
    [SerializeField]
    Transform attackRayCaster;
    [SerializeField]
    float damageDelay = 2f;
    bool dead = false;
    [SerializeField]
    public Transform[] bodyParts;
    public bool takingDamage = false;
    [SerializeField]
    float dodgeCooldown = 0f;
    bool dodging = false;
    bool canDodge = true;
    [SerializeField]
    bool hasRootFull = true;
    [SerializeField]
    bool hasAttackRoot = false;
    float timeTillEndOfAnim = 0f;
    [SerializeField]
    bool runner = false;
    [SerializeField]
    private int damage = 20;
    [SerializeField]
    float soundFXDelay = 3f;
    bool isPlayingSound = false;
    bool isGrowling = false;
    bool shutdown = false;
    [SerializeField]
    float attackRange = 2f;
    HumanoidRobotSoundEffects _soundEffects;
    // Use this for initialization

    void Start()
    {
        _soundEffects = GetComponent<HumanoidRobotSoundEffects>();
        if (player == null)
        {
            player = GameData.instance.player;
        }
        agent = GetComponent<NavMeshAgent>();
        //anim = GetComponent<Animator>();
        if (hasAttackRoot)
        {
            anim.applyRootMotion = false;
        }
        if (hasRootFull)
        {
            anim.applyRootMotion = true;
        }


        destinationArrayLength = destinations.Length;
        CurrentDestination = 0;
        if (runner)
        {
            GoRun();
        }
        else
        {
            GoWalk();
        }

        SetDestination();

    }

    // Update is called once per frame
    private void Update()
    {
        takingDamage = GetComponent<NPCHealthMC>().takingDamage;
        dead = GetComponent<NPCHealthMC>().dead;
        CheckDestinationAngle();
        if (aggro)
        {
            if (CanSee(player, transform.position))
            {
            
                aggroTimer = aggroCooldown;
                TrackPlayer();
            }
            if (GetPlayerDistance(player.transform.position, transform.position) > agent.stoppingDistance)
            {
                if (!attacking)
                {
                    if (runner)
                    {
                        GoRun();
                    }
                    else
                    {
                        GoWalk();
                    }
                }
            }

            agent.SetDestination(playerPosition);
        }
        if (aggroTimer > 0)
        {
            aggroTimer -= Time.deltaTime;
        }
        if (aggroTimer <= 0)
        {
            aggroTimer = 0;
            if (aggro)
            {
                aggro = false;
                GoWalk();
                SetDestination();
            }
        }
        if (!aggro)
        {
            CheckDestinationDistance(transform.position, destinations[CurrentDestination].position);
        }
        if (attacking)
        {
            Vector3 dir = player.transform.position - transform.position;
            dir.y = 0;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, dir, 5, 5);

            transform.rotation = Quaternion.LookRotation(newDir);
        }
        if (!dead)
        {
            _soundEffects.velZ = velocityZ;
            _soundEffects.speedMulti = speedMultiplier;
            CheckPlayerDistance();
            SetSpeed();
        }
        if (takingDamage)
        {
            TakingDamage();
        }
        if (dead)
        {
            Die();
        }
        velocityZ = Mathf.SmoothDamp(velocityZ, velocityZTarget, ref curVelocityZ, smoothTime);
        anim.SetFloat("speed", velocityZ);
    }
    void TakingDamage()
    {
        if (!isPlayingSound)
        {
            isPlayingSound = true;
            ZombieSoundEffects _soundFX = GetComponent<ZombieSoundEffects>();
            _soundFX.PlaySound(2);
            StartCoroutine(SoundFXCooldown());
        }
        if (run)
        {
            if (canDodge && !dodging)
            {
                dodging = true;
                canDodge = false;
                StartCoroutine(DodgeCooldown());


                if (!anim.hasRootMotion)
                {

                    anim.applyRootMotion = true;
                    anim.SetTrigger("Dodge");
                    StartCoroutine(GetAnimInfo());
                }
                else
                {
                    anim.SetTrigger("Dodge");
                }
            }
        }
    }
    void CheckPlayerDistance()
    {
        float playerDistance = GetPlayerDistance(player.transform.position, transform.position);
        if (playerDistance < aggroRange)
        {
            if (!aggro)
            {
                if (CanSee(player, transform.position))
                {
                    aggroTimer = aggroCooldown;
                    GoAggro();
                    TrackPlayer();
                }
            }
        }
        if (playerDistance <= attackRange)
        {
            if (CanSee(player, transform.position))
            {
                GoIdle();
                if (!attacking)
                {
                    Attack();
                }
            }
        }
        

    }

    void TrackPlayer()
    {
        playerPosition = player.transform.position;
    }
    protected override void SetDestination()
    {
        agent.SetDestination(destinations[CurrentDestination].position);
    }
    void Attack()
    {
        attacking = true;
        agent.velocity = Vector3.zero;
        anim.SetTrigger("Attack");
        if (hasAttackRoot)
        {
            if (!anim.hasRootMotion)
            {
                agent.updateRotation = false;
                anim.applyRootMotion = true;
                StartCoroutine(GetAnimInfo());
            }
        }
        StartCoroutine(AttackDelay());
        //StartCoroutine(AttackCooldown());
        if (!isPlayingSound)
        {
            ZombieSoundEffects _soundFX = GetComponent<ZombieSoundEffects>();
            _soundFX.PlaySound(1);
            StartCoroutine(SoundFXCooldown());
        }

    }
    void GoGrowl()
    {
        if (!isGrowling)
        {
            if (!isPlayingSound)
            {
                isGrowling = true;
                ZombieSoundEffects _soundFX = GetComponent<ZombieSoundEffects>();
                _soundFX.PlaySound(1);
                StartCoroutine(SoundFXCooldown());
            }
        }
    }
    void GoIdle()
    {
        run = false;
        walk = false;
        idle = true;
        
        speedMultiplier = 0;
        velocityZTarget = 0;
    }
    void GoWalk()
    {
        if (!dodging)
        {
            run = false;
            idle = false;
            walk = true;
            speedMultiplier = 1f;
            velocityZTarget = 0.5f;
        }
    }
    void GoRun()
    {
        if (!dodging)
        {
            idle = false;
            walk = false;
            run = true;
            speedMultiplier = 2;
            velocityZTarget = 1;
        }
    }
    void GoAggro()
    {
        aggro = true;
        if (runner)
        {
            GoRun();
        }
        else
        {
            GoWalk();
        }
    }
    public void Die()
    {
        if (!shutdown)
        {
            shutdown = true;
            dead = true;
            run = false;
            walk = false;
            idle = false;
            speedMultiplier = 0;
            velocityZTarget = 0f;
            agent.speed = 0f;
            if (lastSpawn)
            {
                GameObject go = Instantiate(ProgressTracker.instance.keyCards[ProgressTracker.instance.keyLevel - 1]);
                go.transform.position = transform.position + transform.up;
            }

            anim.SetTrigger("Dead");
        }

    }
    [SerializeField]
    float leftAngle = 90f;
    [SerializeField]
    float rightAngle = 90f;
    void CheckDestinationAngle()
    {

        Vector3 dir = agent.nextPosition - transform.position;
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
        if (axisX < leftAngle)
        {
            axisX = 1;
        }
        else if (axisX > rightAngle)
        {
            axisX = -1;
        }
        else
        {
            axisX = 0;
        }
        axisZ = 0;
        //anim.SetFloat("axisZ", axisZ);
        //anim.SetFloat("axisX", axisX);
    }
    void DisableOnDeath()
    {
        Destroy(agent);
        Destroy(this);
    }
    IEnumerator GetAnimInfo()
    {
        yield return new WaitForEndOfFrame();
        timeTillEndOfAnim = anim.GetCurrentAnimatorStateInfo(0).length;
        StartCoroutine(EndOfAnimation());
    }
    IEnumerator EndOfAnimation()
    {
        yield return new WaitForSeconds(timeTillEndOfAnim);
        if (hasAttackRoot)
        {
            anim.applyRootMotion = false;
            dodging = false;
            agent.updateRotation = true;
        }

        if (runner)
        {
            GoRun();
        }
        if (!runner)
        {
            GoWalk();
        }
        attacking = false;
    }
    IEnumerator DodgeCooldown()
    {
        yield return new WaitForSeconds(5);
        canDodge = true;
    }
    IEnumerator AttackDelay()
    {

        yield return new WaitForSeconds(damageDelay);
        RaycastHit hit;
        if (Physics.Raycast(attackRayCaster.position, transform.forward, out hit, attackRayLength, mask))
        {
            Debug.Log(hit.transform.name.ToString());
            if (hit.transform.tag == "Player")
            {
                Debug.Log("HitPlayer");
                hit.transform.GetComponent<PlayerOffline>().TakeDamage(damage);
            }
        }
    }
    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);

        attacking = false;

    }
    IEnumerator SoundFXCooldown()
    {
        yield return new WaitForSeconds(soundFXDelay);
        isPlayingSound = false;
        isGrowling = false;
    }
}


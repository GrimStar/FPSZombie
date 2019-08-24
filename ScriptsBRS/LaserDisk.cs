using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDisk : MonoBehaviour {
    [SerializeField]
    float activatedRotationSpeed = 2f;
    [SerializeField]
    float chargeTime = 4f;
    [SerializeField]
    float laserActivateInterval = 0.25f;
    float chargeSpeed = 0f;
    [SerializeField]
    float chargeAcceleration = 2f;
    [SerializeField]
    GameObject[] lasers;
    int laserCount;
    bool laserInterval = true;
    bool lasersActivated = false;
    bool chargeing = true;
    bool shootingRay = false;
    int rayCount = 0;
    List<GameObject> targetsInRange = new List<GameObject>();
    List<GameObject> targetsToRemove = new List<GameObject>();
    int counter = 0;
    [SerializeField]
    float doDamageInterval = 0.5f;
    bool isDoingDamage = false;
    [SerializeField]
    float lifeTime = 5f;
    bool deathCountdown = false;
    public bool isAirborn = false;
    float yRot = 0f;
    // Use this for initialization
    void Start () {
        StartCoroutine(ChargeTime());
        GetComponent<SphereCollider>().radius = 3;
	}
	
	// Update is called once per frame
	void Update () {
        if (chargeing)
        {
            Charge();
        }
        if (!laserInterval)
        {
            if (!isAirborn)
            {
                laserInterval = true;
                StartCoroutine(LaserInterval());
            }
        }
        if (lasersActivated)
        {
            ActivatedRotate();
            if (!deathCountdown)
            {
                deathCountdown = true;
                StartCoroutine(LifeTimeCountdown());
            }
        }
        if (!chargeing)
        {
            
            if (!isDoingDamage)
            {
                isDoingDamage = true;
                StartCoroutine(DamageDelay());
            }
           
        }
    }
   
    void ActivatedRotate()
    {
        if(yRot > 360f)
        {
            yRot = 0f;
        }
        Quaternion rot = Quaternion.Euler(0, yRot, 0);   
        transform.localRotation = rot;
        yRot += Time.deltaTime * activatedRotationSpeed;
        
    }
    void Charge()
    {
        chargeSpeed += chargeAcceleration * Time.deltaTime;
        if (yRot > 360f)
        {
            yRot = 0f;
        }
        Quaternion rot = Quaternion.Euler(0, yRot, 0);
        transform.localRotation = rot;
        yRot += Time.deltaTime * chargeSpeed;
        
        
    }
    void ActivateLasers()
    {
        lasers[laserCount].SetActive(true);
    }
    IEnumerator LaserInterval()
    {
        ActivateLasers();
        yield return new WaitForSeconds(laserActivateInterval);
        if(laserCount < lasers.Length - 1)
        {
            laserCount += 1;
            laserInterval = false;
        }
        else
        {
            lasersActivated = true;
        }
        
    }
    IEnumerator DamageDelay()
    {
        yield return new WaitForSeconds(doDamageInterval);
        DoDamage();
        isDoingDamage = false;
    }
    void DoDamage()
    {
        Debug.Log("Damage");
        foreach(GameObject _target in targetsInRange)
        {
            IDamagable damagable = _target.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.ITakeDamage(25);
            }
        }
        foreach(GameObject _target in targetsToRemove)
        {
            targetsInRange.Remove(_target);
        }
        targetsToRemove.Clear();
    }
    IEnumerator ChargeTime()
    {
        
        yield return new WaitForSeconds(chargeTime);
        chargeing = false;
        laserInterval = false;
    }
    IEnumerator LifeTimeCountdown()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(transform.root.transform.gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "enemy")
        {
            Debug.Log("Enemyinrange");
            targetsInRange.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "enemy")
        {
            targetsToRemove.Add(other.gameObject);
        }
    }

}

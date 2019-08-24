using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    
    public GameObject bulletHole;
    public GameObject bloodFX;
    public GameObject sparkEffects;
	// Use this for initialization
	

    // Update is called once per frame
    public float raysPS = 700f;
    public float rayTimer = 1f;
    public float bulletDrop = 5f;
    public float rayMultiplier = 100f;
    private float rayDistance = 0f;
    public float currentRaySpawn = 0f;
    private bool casting = false;
    private Vector3 raySpawn = Vector3.zero;
    private float destroyCountdown = 0f;
    private bool hitObject = false;
    private RaycastHit hit;
    [SerializeField]
    public AudioSource bulletSound;
    [SerializeField]
    GameObject[] particalEffects;
    [SerializeField]
    private GameObject bloodSplatterFX;
   

    private void Awake()
    {
        float _pitch = Random.Range(0.9f, 1.1f);
        bulletSound.pitch = _pitch;
        
    }
    void Start()
    {
        bulletSound.Play();
        raySpawn = transform.position;
        rayTimer = (rayTimer / raysPS) * rayMultiplier;
        rayDistance = rayMultiplier;
        bulletDrop =  ((bulletDrop / 1000) * rayMultiplier);
    }
    private void FixedUpdate()
    {
        if (!casting)
        {
            casting = true;
            StartCoroutine(RayTimer());
        }
        destroyCountdown += 1f * Time.deltaTime;
        if(destroyCountdown >= 5f)
        {
            Destroy(this.gameObject);
        }
        
    }
    void ShootRay()
    {
        if (hitObject)
        {
            return;
        }
        
        
        
        if (Physics.Raycast(raySpawn, transform.forward, out hit, rayDistance + 1))
        {
            hitObject = true;
            
            //-----------------------
            SurfaceData surfData = hit.transform.root.GetComponent<SurfaceData>();
            if(surfData != null)
            {
                GameObject impactFX = (GameObject)Instantiate(particalEffects[surfData.surfaceMaterial], hit.point, Quaternion.identity);

                impactFX.transform.up = hit.normal;
                impactFX.transform.SetParent(hit.transform);
            }

            IDamagable damagable = hit.transform.root.GetComponent<IDamagable>();
            if(damagable != null)
            {
                damagable.ITakeDamage(25);
            }
            Debug.Log("Hit: " + hit.transform.name);
            
        }
        currentRaySpawn += rayMultiplier;
        Debug.DrawLine(raySpawn, raySpawn + transform.forward * rayMultiplier, Color.green, 2f * Time.deltaTime);
        raySpawn = raySpawn + (-transform.up * bulletDrop);
        raySpawn = raySpawn + (transform.forward * rayMultiplier); //rayMultiplier was originally currentRaySpawn
        

        
    }
    void BloodSplatterCast(Vector3 _hitPoint)
    {
        RaycastHit _hit;
        if(Physics.Raycast(_hitPoint + transform.forward, transform.forward, out _hit, 1f))
        {
            GameObject bloodSplatter = (GameObject)Instantiate(bloodSplatterFX, hit.point, Quaternion.identity);
            bloodSplatter.transform.forward = -hit.normal;
            bloodSplatter.transform.SetParent(hit.transform);
        }
    }
    IEnumerator RayTimer()
    {
        ShootRay();
        yield return new WaitForSeconds(rayTimer);
        casting = false;
    }
}

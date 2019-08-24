using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour {

    PlayerShootControlV2 shootScript;
    public GameObject muzzleFlash;
    private float destroyDelay = 0;
	// Use this for initialization
	void Start () {
        shootScript = transform.root.transform.GetComponent<PlayerShootControlV2>();
        Debug.Log("Start");
	}
    private void OnEnable()
    {
        Debug.Log("Enabled");
        if(shootScript != null)
        {
            shootScript.muzzleFlash = this;
        }
    }
    private void Awake()
    {
        Debug.Log("Awake");
        shootScript = transform.root.transform.GetComponent<PlayerShootControlV2>();
    }
    // Update is called once per frame
    void Update () {
		
	}
    public void InstantiateEffects()
    {
        GameObject flash = (GameObject)Instantiate(muzzleFlash);
        flash.transform.position = transform.position;
        flash.transform.rotation = transform.rotation;
        destroyDelay = flash.GetComponent<ParticleSystem>().main.duration;
        StartCoroutine(DestroyDelay(flash));
    }
    IEnumerator DestroyDelay(GameObject _flash)
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(_flash);
    }
}

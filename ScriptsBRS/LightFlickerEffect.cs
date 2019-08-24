using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlickerEffect : MonoBehaviour {
    public Light light;
    float intensity = 2;
    float intensityV;
    bool flick = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        light.intensity = intensity;
        if (!flick)
        {
            flick = true;
            StartCoroutine(FlickerTimer());
        }
        

    }
    IEnumerator FlickerTimer()
    {
        float ran = Random.Range(0.8f, 2);
        intensity = 0f;
        yield return new WaitForSeconds(ran);
        intensity = 2f;
        flick = false;
    }
}

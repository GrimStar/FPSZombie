using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepSound : MonoBehaviour {

    AudioSource _audio;
    [SerializeField]
    AudioClip[] clips;
    int prevClip = 0;
	// Use this for initialization
	void Start () {
        _audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Step()
    {
        int randClip = Random.Range(0, clips.Length - 1);
        if(randClip == prevClip)
        {
            Step();
            return;
        }
        _audio.clip = clips[randClip];
        prevClip = randClip;
        _audio.Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidRobotSoundEffects : MonoBehaviour {

    public float timer = 0.0f;
    public float bobbingSpeed = 0.18f;
    private float actualBobbingSpeed = 0f;
    public float bobbingAmount = 0.2f;
    public float midpoint = 2.0f;
    public float speedMulti = 0f;
    bool step = false;
    bool joint = false;
    public float velZ = 0f;
    [SerializeField]
    AudioSource jointAudio;
    [SerializeField]
    AudioSource stepAudio;
    int prevClip = 0;
    [SerializeField]
    AudioClip[] jointClips;
    [SerializeField]
    AudioClip[] footStepClips;

    private void Awake()
    {
        midpoint = transform.localPosition.x;
    }
    private void Start()
    {
        
       
    }
    private void Update()
    {
        actualBobbingSpeed = bobbingSpeed * speedMulti;
        float waveslice = 0.0f;
     
        float vertical = velZ;
        if (Mathf.Abs(vertical) == 0)
        {
            timer = 0.0f;
        }
        else
        {
            waveslice = Mathf.Sin(timer);
            timer = timer + actualBobbingSpeed;
            if (timer > Mathf.PI * 2)
            {
                timer = timer - (Mathf.PI * 2);
            }
        }
        if (waveslice != 0)
        {

            float translateChange = waveslice * bobbingAmount;
            float totalAxes = Mathf.Abs(vertical);
            totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
            translateChange = totalAxes * translateChange;           
            if (translateChange > 0)
            {
                if (!step)
                {
                    step = true;
                    Step();
                }

            }
            else
            {
                if (step)
                {
                    step = false;
                    Step();
                }
            }
            if (translateChange > 0.05f)
            {
                if (!joint)
                {
                    joint = true;
                    Joints();
                }

            }
            if(translateChange < -0.05f)
            {
                if (joint)
                {
                    joint = false;
                    Joints();
                }
            }

        }
       

    }
    void Step()
    {
        int randClip = Random.Range(0, footStepClips.Length - 1);
        if (randClip == prevClip)
        {
            Step();
            return;
        }
        stepAudio.clip = footStepClips[randClip];
        prevClip = randClip;
        stepAudio.Play();
    }
    void Joints()
    {
        int randClip = Random.Range(0, jointClips.Length - 1);
        if (randClip == prevClip)
        {
            Step();
            return;
        }
        jointAudio.clip = jointClips[randClip];
        prevClip = randClip;
        jointAudio.Play();
    }
}

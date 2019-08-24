using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour {

    // Use this for initialization
    public float timer = 0.0f;
    public float bobbingSpeed = 0.18f;
    private float actualBobbingSpeed = 0f;
    public float bobbingAmount = 0.2f;
   
    public float midpoint = 2.0f;
    private SpeedControl speedScript;
    bool step = false;
    [SerializeField]
    FootStepSound _footStep;
    private void Awake()
    {
        midpoint = transform.localPosition.x;
    }
    private void Start()
    {
        speedScript = transform.root.GetComponent<SpeedControl>();
    }
    private void Update()
    {
        actualBobbingSpeed = bobbingSpeed * speedScript.speedMultiplier;
        float waveslice = 0.0f;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
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
           
            float translateChange = waveslice * bobbingAmount * speedScript.speedMultiplier;
            float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
            translateChange = totalAxes * translateChange;
            //transform.localPosition.y = midpoint + translateChange;
            Vector3 newPosition = new Vector3(midpoint + translateChange, transform.localPosition.y, transform.localPosition.z);
            transform.localPosition = newPosition;
            if(translateChange > 0)
            {
                if (_footStep != null)
                {
                    if (!step)
                    {
                        step = true;
                        _footStep.Step();
                    }
                }

            }
            else
            {
                if (_footStep != null)
                {
                    if (step)
                    {
                        step = false;
                        _footStep.Step();
                    }
                }
            }
            
        }
        else
        {
            
            
            Vector3 newPosition = new Vector3(midpoint, transform.localPosition.y, transform.localPosition.z);
            transform.localPosition = newPosition;
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour {
    public float lookSensitivity;
    public float yRotation;
    public float xRotation;
    public float currentYRotation;
    public float currentXRotation;
    public float yRotationV;
    public float xRotationV;
    public float lookSmoothDamp = 0.1f;
    [SerializeField]
    private Camera cam;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        yRotation += Input.GetAxis("Mouse X") * lookSensitivity;
        xRotation -= Input.GetAxis("Mouse Y") * lookSensitivity;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        currentXRotation = Mathf.SmoothDamp(currentXRotation, xRotation, ref xRotationV, lookSmoothDamp);
        currentYRotation = Mathf.SmoothDamp(currentYRotation, yRotation, ref yRotationV, lookSmoothDamp);

        cam.transform.rotation = Quaternion.Euler(currentXRotation, currentYRotation, 0);

    }
}

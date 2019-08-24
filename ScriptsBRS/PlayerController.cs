using UnityEngine;


[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {
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
    public float speed = 5f;
	public float actualSpeed;
	private PlayerMotor motor;
	private float origHeight;
	//public float forward;
	public float crouchMovementSpeed;
    public float crouchSpeed;
	public bool crouch = false;
	//public float xRot = 0f;
	public bool inventoryActive = false;
    private Quaternion targetRotation;
    private Quaternion targetRotationHorizontal;
    private float minRestrict = -90f;
    private float maxRestrict = 90f;
    //[SerializeField]
    //private GameObject recObject;
    [SerializeField]
    private GameObject mainObject;
    public float negRestriction = -90f;
    public float posRestriction = 90f;
    private Vector3 crouchPosition;
    private Vector3 standPosition;
    public bool mountedToChair = false;
    private SpeedControl speedScript;
    public bool DisableUpdate = false;
    // Use this for initialization
    void Start () {
        speedScript = GetComponent<SpeedControl>();
		origHeight = GetComponent<CapsuleCollider>().height;
		motor = GetComponent<PlayerMotor> ();
        crouchPosition = new Vector3(mainObject.transform.localPosition.x, mainObject.transform.localPosition.y - 0.5f, mainObject.transform.localPosition.z);
        
    }
	
    // Update is called once per frame
    void Update()
    {
        if (DisableUpdate)
        {
            return;
        }
        else
        {
            motor.disableUpdate = false;
        }
        
        actualSpeed = speed * speedScript.speedMultiplier;
        
        targetRotation = motor.cam.transform.localRotation;

        float _xMov = Input.GetAxisRaw("Horizontal");
        float _zMov = Input.GetAxisRaw("Vertical");
        if (speedScript.sliding)
        {
            _xMov = 0f;
            _zMov = 0f;
        }
        
        if (speedScript.crouching)
        {
           
            CapsuleCollider _col = GetComponent<CapsuleCollider>();
            if(_col.height > origHeight / 2)
            {
                _col.height -= 0.1f * crouchSpeed * Time.deltaTime;
            }                           
            crouch = true;
        }
        else
        {
            CapsuleCollider _col = GetComponent<CapsuleCollider>();
            if (_col.height < origHeight)
            {
                _col.height += 0.1f * crouchSpeed * Time.deltaTime;
            }
            
            crouch = false;
        }
      
        Vector3 _movHorizontal = transform.right * _xMov;
        Vector3 _movVertical = transform.forward * _zMov;

        Vector3 _velocity = (_movHorizontal + _movVertical).normalized * actualSpeed;
        
        yRotation += Input.GetAxis("Mouse X") * lookSensitivity;
        xRotation -= Input.GetAxis("Mouse Y") * lookSensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        

        currentXRotation = Mathf.SmoothDamp(currentXRotation, xRotation, ref xRotationV, lookSmoothDamp);
        currentYRotation = Mathf.SmoothDamp(currentYRotation, yRotation, ref yRotationV, lookSmoothDamp);
        

        motor.Move(_velocity);
        motor.UpdateRotation(currentXRotation, currentYRotation);
    }
	public void ReConfigRotation(float _yRotation)
    {
        yRotation = _yRotation;
      
        currentYRotation = _yRotation;
        motor.currentYRotation = _yRotation;
        DisableUpdate = false;
      
    }	
    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, negRestriction, posRestriction);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }
}

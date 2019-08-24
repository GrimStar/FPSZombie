using UnityEngine;
using System.Collections;

public class CamRecoil : MonoBehaviour {

    private int shotCount = 1;
    public float recoilUpAmount = 2f;
    public float minKick = 0f;
    public float kickSpeed;
    public float kickAmount;
    private bool shooting = false;
    private bool ready = false;

    [SerializeField]
	private GameObject weapon;
    private PlayerController controllerScript;
    private Vector3 kick = Vector3.zero;
    //CamHolder Variables
    [SerializeField]
	private GameObject cam;
    //[SerializeField]
    //private GameObject camHolder;
    public float camRotationSpeed = 0f;
    public Vector3 camsKickVertical = new Vector3(0f, 0f, 0.02f);
    private Vector3 camKickTarget;
    private Vector3 origCameraPosition;
    private Vector3 camKickedPosition;
    public float camRotationHorizontal = 0.2f;
    public float camRotationVertical = 0f;
    //Weapon variables
    private Quaternion weaponkickedRotation;
    private Vector3 weaponKickTarget;
	private Vector3 origWeaponPos;   
	private Quaternion weaponRotationTarget;
	private Quaternion origWeaponRotation;
    private Vector3 weaponKickedPosition;

    //public GameObject[] weapons;
    //public float kickRot = 0f;
    // public float upAmount;
    //public float recSpeed = 1f;
    //public float _recKick;
    //public float maxRec = 10f;
    //private Quaternion targetAngle;
    //public Quaternion origAngle;
    //public WeaponSpecs scrpt;
    //----------------------

    // Use this for initialization
    void Start () {
        controllerScript = GetComponent<PlayerController>();
        UpdateWeapon(transform.gameObject);
        ready = true;
	}
	
	// Update is called once per frame
	void Update () {
        if(controllerScript != null)
        {
           //controllerScript.UpdateRestrictions(cam.transform.localRotation.x);
        }
        if(weapon == null)
        {
            return;
        }
		if (ready == true) {
            
			Quaternion camRotationalRecoil = Quaternion.Euler (-camRotationVertical, camRotationHorizontal, 0f);
            //recRot = Quaternion.Euler(-kickRot, 0f, 0f);

            //camHolder.transform.localRotation = Quaternion.Lerp (camHolder.transform.localRotation, camRotationalRecoil, Time.time * camRotationSpeed);
           // weapon.transform.localPosition = Vector3.Lerp (weapon.transform.localPosition, weaponKickTarget, Time.time * kickSpeed);
			//weapon.transform.localRotation = Quaternion.Lerp (weapon.transform.localRotation, weaponRotationTarget, Time.time * kickSpeed);
            cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, camKickTarget, Time.time * kickSpeed);
            if(camKickTarget == camKickedPosition)
            {
                if(cam.transform.localPosition.z <= camKickedPosition.z)
                {
                   // weaponKickTarget = origWeaponPos;
                    //weaponRotationTarget = origWeaponRotation;

                    camKickTarget = origCameraPosition;

                }
            }
            if (camKickTarget == origCameraPosition && shooting == true) {
                if(cam.transform.localPosition.z >= origCameraPosition.z)
                {
                    shooting = false;
                }
			}
		}
	}
	public void Recoil () {
        Debug.Log("Recoil");
        minKick = (recoilUpAmount / 100) * 80;
        camRotationVertical += Random.Range(minKick, recoilUpAmount);
		WeaponRecoil ();

	}
	public void UpdateWeapon (GameObject _equipped) {
        
        weapon = _equipped;
        //origWeaponPos = weapon.transform.localPosition;
        origCameraPosition = cam.transform.localPosition;
        //origWeaponRotation = weapon.transform.localRotation;
       // weaponRotationTarget = origWeaponRotation;
        kick = new Vector3(0f, 0f, -kickAmount);
       // weaponKickTarget = origWeaponPos;
        camKickTarget = origCameraPosition;
       // weaponKickedPosition = weaponKickTarget - kick;
        camKickedPosition = camKickTarget - camsKickVertical;

	}
	void WeaponRecoil() {
		if (shooting == false) {
            if(shotCount == 1)
            {
                shotCount = -1;
            }
            else
            {
                shotCount = 1;
            }
            //weaponKickTarget = weaponKickedPosition;
            camKickTarget = camKickedPosition;
            camRotationHorizontal = camRotationHorizontal * shotCount;
           // weaponRotationTarget = weaponkickedRotation;
			shooting = true;
		}
	}
}

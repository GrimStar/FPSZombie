using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControl : MonoBehaviour {
    public GameObject cam;
    public float targetXRotation;
    public float targetYRotation;
    public float targetXRotationV;
    public float targetYRotationV;
    public float rotateSpeed = 0.3f;
    public float holdHeight = -0.5f;
    public float holdSide = 0.5f;
    public float ratioHipHold = 1f;
    public float hipToAimSpeed = 0.1f;
    public float ratioHipHoldV;
    
    public float recoilAmount = 0.1f;
    public float recoilRecoveryTime = 0.2f;
    public float currentRecoilZPos;
    public float currentRecoilZPosV;
    public float shootAngleRandomizationAiming = 3f;
    public float shootAngleRandomizationNotAiming = 10f;
    public Transform[] itemsArray;
    private Dictionary<int, Transform> Items = new Dictionary<int, Transform>();
    public WeaponDataPack weaponData;
    private Transform equipped;
    public PlayerShootControlV2 playerShootScript;
    public bool melee = true;
    
    
    // Use this for initialization
    void Start () {
        Setup();
	}
    private void Setup()
    {
        int count = 0;
        foreach(Transform _item in itemsArray)
        {
            Items.Add(count, _item);
            count++;
        }
    }
    public void EquipWeapon(int _id)
    {
        
        Transform item = Items[_id];
        if (item.gameObject.activeSelf)
        {
            item.gameObject.SetActive(false);
            equipped = null;
            melee = true;
        }
        else
        {
            if (equipped != null)
            {
                equipped.gameObject.SetActive(false);
            }
            item.gameObject.SetActive(true);
            weaponData = item.GetComponent<ItemData>().weaponData;
            equipped = item;
            melee = weaponData.Melee;
            UpdateSpecs();
        }
    }
    private void UpdateSpecs()
    {
        Debug.Log("UpdateSpecs");
        if(playerShootScript != null)
        {
            playerShootScript.UpdateEquipped();
        }
        //Update fire rate in PlayerShootControl
        //Update shootRandomization and recoilAmount in this script
    }
    public void Shoot()
    {
        //currentRecoilZPos -= recoilAmount;
        currentRecoilZPos -= weaponData.RecoilAmount;
        targetXRotation += (Random.value - 0.5f) * Mathf.Lerp(weaponData.RecoilAngleAim, weaponData.RecoilAngleHip, ratioHipHold);
        targetYRotation += (Random.value - 0.5f) * Mathf.Lerp(weaponData.RecoilAngleAim, weaponData.RecoilAngleHip, ratioHipHold);
        // targetXRotation += (Random.value - 0.5f) * Mathf.Lerp(shootAngleRandomizationAiming, shootAngleRandomizationNotAiming, ratioHipHold);
        // targetYRotation += (Random.value - 0.5f) * Mathf.Lerp(shootAngleRandomizationAiming, shootAngleRandomizationNotAiming, ratioHipHold);
    }
    // Update is called once per frame
   
    void Update () {
        if (Input.GetButton("Fire2"))
        {
            ratioHipHold = Mathf.SmoothDamp(ratioHipHold, 0, ref ratioHipHoldV, hipToAimSpeed);
        }
        if(!Input.GetButton("Fire2"))
        {
            ratioHipHold = Mathf.SmoothDamp(ratioHipHold, 1, ref ratioHipHoldV, hipToAimSpeed);
        }
        
        
        transform.position = cam.transform.position + (Quaternion.Euler(0, targetYRotation, 0) * new Vector3(holdSide * ratioHipHold, holdHeight * ratioHipHold, 0) + Quaternion.Euler(targetXRotation, targetYRotation, 0) * new Vector3(0, 0, currentRecoilZPos));
        PlayerController scrpt = cam.transform.root.transform.GetComponent<PlayerController>();
        

        //targetXRotation = Mathf.SmoothDamp(targetXRotation, scrpt.currentXRotation, ref targetXRotationV, rotateSpeed);
        //targetYRotation = Mathf.SmoothDamp(targetYRotation, scrpt.currentYRotation, ref targetYRotationV, rotateSpeed);
        //targetXRotation = Mathf.Lerp(targetXRotation, scrpt.xRotation, rotateSpeed);
        //targetYRotation = Mathf.Lerp(targetYRotation, scrpt.yRotation, rotateSpeed);


        currentRecoilZPos = Mathf.SmoothDamp(currentRecoilZPos, 0, ref currentRecoilZPosV, recoilRecoveryTime);
        ////transform.position = cam.transform.position + (Quaternion.Euler(0, targetYRotation, 0) * new Vector3(holdSide * ratioHipHold, holdHeight * ratioHipHold, 0) + Quaternion.Euler(targetXRotation, targetYRotation, 0) * new Vector3(0, 0, currentRecoilZPos));
        transform.rotation = Quaternion.Euler(targetXRotation, targetYRotation, 0);

    }
    private void LateUpdate()
    {
        
        PlayerController scrpt = cam.transform.root.transform.GetComponent<PlayerController>();
        
        //targetXRotation = Mathf.SmoothDamp(targetXRotation, scrpt.xRotation, ref targetXRotationV, rotateSpeed);
        //targetYRotation = Mathf.SmoothDamp(targetYRotation, scrpt.yRotation, ref targetYRotationV, rotateSpeed);
        ////transform.rotation = Quaternion.Euler(targetXRotation, targetYRotation, 0);
        //// transform.position = cam.transform.position + (Quaternion.Euler(targetXRotation, targetYRotation, 0) * new Vector3(holdSide * ratioHipHold, holdHeight * ratioHipHold, 0) + Quaternion.Euler(targetXRotation, targetYRotation, 0) * new Vector3(0, 0, currentRecoilZPos));
        targetXRotation = Mathf.SmoothDamp(targetXRotation, scrpt.currentXRotation, ref targetXRotationV, rotateSpeed);
        targetYRotation = Mathf.SmoothDamp(targetYRotation, scrpt.currentYRotation, ref targetYRotationV, rotateSpeed);

    }
}

using UnityEngine;
using System.Collections;

public class ItemData : MonoBehaviour {
    public StructureDataPack _structureData;
    public int _structureType;
    //-------------------------------------
    public IKPositions _ikPositions;
    public GameObject _leftHand;
    public GameObject _rightHand;
    //----------------------------------
	public ItemDataPack _item;
	public int _id;
	public int _amount;
	public int _type;
    public bool _hasIK;
    public bool _stackable;
	//--------------------------------
	public WeaponDataPack weaponData;
	public float range;
	public int damage;
	public float fireRate;
	public bool melee = true;
	public bool fireArm = false;
	public bool autoFire = false;
	public bool singleFire = true;
	public float drop;
    public float recoilAmount = 0f;
    public float recoilAngleHip = 0f;
    public float recoilAngleAim = 0f;
    public int sightType = 0;
    public int weaponAttachmentType = 0;
    public int weaponType = 0;
    public int soundFXID = 0;
    public bool throwable = false;
    public int explosivesID = 0;
    //--------------------------------
    public WeaponAttachmentsDataPack weaponAttachmentData;
    public int maxSight;
    public int maxGrip;
    public int muzzleType;
    public int stockType;
    public int magazineType;
    //-----------------------------
    public AttachmentDataPack attachmentData;
    public int attachmentType;
    public int attachmentId;
    public int attachmentSight;

    //----------------
    public AttachedDataPack attachedData;
    public int _sight = 0;
    public int _muzzle = 0;
    public int _grip = 0;
    public int _mag = 0;
    public int _stock = 0;


    public bool disable = false;
	// Use this for initialization
	void Awake () {
        _structureData.StructureType = _structureType;
        _ikPositions.LeftHandIK = _leftHand;
        _ikPositions.RightHandIK = _rightHand;
		_item.Id = _id;
		_item.Amount = _amount;
		_item.Type = _type;
        _item.HasIK = _hasIK;

        weaponData.RecoilAngleAim = recoilAngleAim;
        weaponData.RecoilAngleHip = recoilAngleHip;
        weaponData.RecoilAmount = recoilAmount;
		weaponData.Damage = damage;
		weaponData.Range = range;
		weaponData.FireRate = fireRate;
		weaponData.Melee = melee;
		weaponData.FireArm = fireArm;
		weaponData.AutoFire = autoFire;
		weaponData.SingleFire = singleFire;
        weaponData.SightType = sightType;
        weaponData.WeaponAttachmentType = weaponAttachmentType;
        weaponData.WeaponType = weaponType;
        weaponData.SoundFXID = soundFXID;
        weaponData.Throwable = throwable;
        weaponData.ExplosiveID = explosivesID;

        weaponAttachmentData.magazineType = magazineType;
        weaponAttachmentData.muzzleType = muzzleType;
        weaponAttachmentData.maxSight = maxSight;
        weaponAttachmentData.maxGrip = maxGrip;
        weaponAttachmentData.stockType = stockType;

        attachmentData.Type = attachmentType;
        attachmentData.Id = attachmentId;
        attachmentData.AttachmentSightType = attachmentSight;

        attachedData.sight = _sight;
        attachedData.muzzle = _muzzle;
        attachedData.mag = _mag;
        attachedData.stock = _stock;
        attachedData.grip = _grip;

		if (disable) {
			transform.gameObject.SetActive (false);
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}

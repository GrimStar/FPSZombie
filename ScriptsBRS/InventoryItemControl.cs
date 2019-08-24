using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections.Generic;
public class InventoryItemControl : MonoBehaviour {
	public Transform[] Inventory;
	public Transform[] secondInventorySlots;
    [SerializeField]
    Transform[] explosiveInventory;

    public GameObject hotbar;
	public GameObject secondarySlot;
    public GameObject primarySlotOne;
    public GameObject primarySlotTwo;
    public GameObject meleeSlot;
    public GameObject explosiveSlotOne;
    public GameObject explosiveSlotTwo;
    public Dictionary<int, GameObject> explosivePrefabs = new Dictionary<int, GameObject>();
    [SerializeField]
    GameObject[] explosivePrefabsArray;
	public Dictionary<int, GameObject> itemIconDict = new Dictionary<int, GameObject> ();
	public GameObject[] ItemInvIcons;

    public Dictionary<int, GameObject> attachmentIconDict = new Dictionary<int, GameObject>();
    public GameObject[] AttachmentInvIcons;

    public Dictionary<int, GameObject> structureIconDict = new Dictionary<int, GameObject>();
    [SerializeField]
    private GameObject[] structureInvIcons;
	public static InventoryItemControl instance;
	public Dictionary<int, int> inventoryContents = new Dictionary<int, int> ();
	public bool needsContentCheck = false;
	//private ItemDataPack newContents;
	private bool _primary = true;
	// Use this for initialization
	void Start () {
		if (instance != null) {
			return;

		} else {
			instance = this;
		}
		Setup ();
	}

	public void Setup() {

		hotbar = CanvasControl._hotbar;
        primarySlotOne = CanvasControl._primarySlotOne;
        primarySlotTwo = CanvasControl._primarySlotTwo;
        secondarySlot = CanvasControl._secondarySlot;
        meleeSlot = CanvasControl._meleeSlot;
		int i = 1;
		foreach (GameObject icon in ItemInvIcons) {
			itemIconDict.Add (i, icon);
			i++;
		}
        i = 0;
        foreach(GameObject icon in AttachmentInvIcons)
        {
            attachmentIconDict.Add(i, icon);
            i++;
        }
		i = 0;
		foreach (Transform slot in Inventory) {
			Inventory [i] = CanvasControl._inventory.transform.GetChild (i);
			i++;
		}
        i = 0;
        foreach(Transform slot in secondInventorySlots)
        {
            secondInventorySlots[i] = hotbar.transform.GetChild(i);
            i++;
        }
        i = 1;
        foreach (GameObject icon in structureInvIcons)
        {
            structureIconDict.Add(i, icon);
            i++;
        }
        i = 1;
        foreach(GameObject prefab in explosivePrefabsArray)
        {
            explosivePrefabs.Add(i, prefab);
            i++;
        }
	}
	// Update is called once per frame
	void Update () {
		if (needsContentCheck) {
			needsContentCheck = false;
			//CheckContents (_primary);
		}
	}
	void CheckContents (ItemData object_) {

        //ItemData itemDataScript = object_.GetComponent<ItemData>();
        ItemData itemDataScript = object_;
        ItemDataPack itemPack = itemDataScript._item;
        
        ///if the item is a weapon
		if (itemPack.Type == 1) {
            WeaponDataPack weaponPack = itemDataScript.weaponData;
            //if item is a primary weapon
            if (weaponPack.WeaponType == 1)
            {
                if (primarySlotOne.transform.childCount == 0)
                {
                    GameObject itemToInstantiate = null;
                    if (itemPack.Type == 1)
                    {
                        itemToInstantiate = itemIconDict[itemPack.Id];
                    }
                    GameObject i = Instantiate(itemToInstantiate);
                    i.transform.SetParent(primarySlotOne.transform);
                    InventoryStackControl scrpt = i.GetComponent<InventoryStackControl>();
                    scrpt.IncreaseStack(itemPack.Amount);
                    return;
                }
                if (primarySlotTwo.transform.childCount == 0)
                {
                    GameObject itemToInstantiate = null;
                    if (itemPack.Type == 1)
                    {
                        itemToInstantiate = itemIconDict[itemPack.Id];
                    }
                    GameObject i = Instantiate(itemToInstantiate);
                    i.transform.SetParent(primarySlotTwo.transform);
                    InventoryStackControl scrpt = i.GetComponent<InventoryStackControl>();
                    scrpt.IncreaseStack(itemPack.Amount);
                    return;
                }
                if (GameData.instance.currentSlot < 2)
                {
                    GameObject itemToInstantiate = null;
                    if (itemPack.Type == 1)
                    {
                        itemToInstantiate = itemIconDict[itemPack.Id];
                    }
                    Destroy(primarySlotOne.transform.GetChild(0).gameObject);
                    GameObject i = Instantiate(itemToInstantiate);
                    i.transform.SetParent(primarySlotOne.transform);
                    InventoryStackControl scrpt = i.GetComponent<InventoryStackControl>();
                    scrpt.IncreaseStack(itemPack.Amount);
                    
                    GameData.instance.weaponHolder.GetComponent<GunControlV2>().EquipWeapon(i);
                    return;
                }
                if (GameData.instance.currentSlot == 2)
                {
                    GameObject itemToInstantiate = null;
                    if (itemPack.Type == 1)
                    {
                        itemToInstantiate = itemIconDict[itemPack.Id];
                    }
                    Destroy(primarySlotTwo.transform.GetChild(0).gameObject);
                    GameObject i = Instantiate(itemToInstantiate);
                    i.transform.SetParent(primarySlotTwo.transform);
                    InventoryStackControl scrpt = i.GetComponent<InventoryStackControl>();
                    scrpt.IncreaseStack(itemPack.Amount);
                    GameData.instance.weaponHolder.GetComponent<GunControlV2>().EquipWeapon(i);
                    return;
                }


            }
            if(weaponPack.WeaponType == 2)
            {
                if(secondarySlot.transform.childCount == 0)
                {
                    
                    return;
                }
            }
            if(weaponPack.WeaponType == 3)
            {
                CheckSlots(object_, explosiveInventory);
                return;
            }
            ////
			
		} else {
            CheckSlots(object_, Inventory);
		}

	}
    void CheckSlots(ItemData itemData, Transform[] _inventory)
    {
        ItemData itemDataScript = itemData.GetComponent<ItemData>();
        ItemDataPack itemPack = itemDataScript._item;
        foreach (Transform slot in _inventory)
        {
            if (slot.childCount > 0)
            {
                ItemDataPack _item = slot.GetChild(0).GetComponent<ItemData>()._item;
                if (_item.Type == itemPack.Type)
                {
                    if (_item.Id == itemPack.Id)
                    {
                        InventoryStackControl scrpt = slot.GetChild(0).GetComponent<InventoryStackControl>();
                        if (scrpt != null)
                        {
                            scrpt.IncreaseStack(itemPack.Amount);
                            Debug.Log(scrpt.amount.ToString());
                        }
                        //child.GetChild(0).GetComponent<Text> ().text = "" + tcount;
                        //DataBox.text = "" + item.gameObject.name;
                        Debug.Log("returned");
                        return;
                    }
                }
            }
        }
        foreach (Transform slot in _inventory)
        {
            if (slot.childCount == 0)
            {
                GameObject itemToInstantiate = null;
                if (itemPack.Type == 1)
                {
                    itemToInstantiate = itemIconDict[itemPack.Id];
                }
                if (itemPack.Type == 2)
                {
                    itemToInstantiate = attachmentIconDict[itemPack.Id];
                }
                GameObject i = Instantiate(itemToInstantiate);
                i.transform.SetParent(slot);
                InventoryStackControl scrpt = i.GetComponent<InventoryStackControl>();
                scrpt.IncreaseStack(itemPack.Amount);
                return;
            }
        }
    }
	public void AddItem (ItemData _object) {
		
		
		CheckContents (_object);


	}

}

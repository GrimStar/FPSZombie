using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
public class PickItUp_DEDI : MonoBehaviour {
	private GameObject object_;
	private bool canPickUp = false;
    private bool checking = false;
    [SerializeField]
    private float checkCooldown;
    [SerializeField]
    private float pickUpDistance;
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private LayerMask mask;
    private bool canMount = false;
    private Transform mountPosition;
    private Rigidbody rb;
    PlayerHUDOffline playerHud;
    bool canInteract = false;
    bool canPickUpKey = false;
    GameObject key;
    GameObject door;
	// Use this for initialization
	void Start () {
        mask = ~mask;
        rb = GetComponent<Rigidbody>();
        playerHud = GetComponent<PlayerHUDOffline>();
	}
	
	// Update is called once per frame
	void Update () {
        if (canPickUpKey)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if(key != null)
                {
                    GameData.instance.player.GetComponent<InventoryContents>().keyCount[key.GetComponent<KeyCard>().KeyLevel - 1] += 1;
                    Destroy(key);
                }
            }
        }
        if (canInteract)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (door != null)
                {
                    door.transform.GetComponent<SlideDoors>().OperateDoor();
                }
            }
        }
        if(object_ != null)
        {
            if(playerHud != null)
            {
                playerHud.pickUpText.text = " " + object_.transform.GetChild(0).gameObject.name;
                if (!playerHud.pickUpPanel.activeSelf)
                {
                    playerHud.pickUpPanel.SetActive(true);
                }
            }

        }
        else
        {
            if(playerHud != null)
            {
                if (playerHud.pickUpPanel.activeSelf)
                {
                    playerHud.pickUpPanel.SetActive(false);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (canPickUp)
            {
                PickUp();
            }
            
        }
        
        if(!checking)
        {
            checking = true;
            CheckForObject();
            StartCoroutine(CheckCoolDown());
        }
	
	}
	void PickUp () {

        ItemData _data = object_.GetComponent<ItemData>();
        if(_data != null)
        {
            InventoryItemControl.instance.AddItem(_data);
            Destroy(object_);
        }
        
    }
	
    
    void CheckForObject ()
    {
        RaycastHit hit;
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, pickUpDistance, mask))
        {
            if(hit.collider.tag == "Item")
            {
                
                object_ = hit.transform.gameObject;
                canPickUp = true;

            }
            else
            {
                canPickUp = false;
                object_ = null;
            }
            if(hit.collider.tag == "Interactable")
            {
                InteractableObjectDataPack pack = hit.transform.GetComponent<InteractableObjectControl>().dataPack;
                if (pack.Door)
                {
                    canInteract = true;
                    door = hit.transform.gameObject;
                    
                }
            }
            else
            {
                canInteract = false;
                door = null;
            }
            if(hit.transform.tag == "KeyCard")
            {
                key = hit.transform.gameObject;
                canPickUpKey = true;
            }
            else
            {
                key = null;
                canPickUpKey = false;
            }
            
        }
        else
        {
            canPickUp = false;
            object_ = null;
        }
    
    }
    IEnumerator CheckCoolDown()
    {
        yield return new WaitForSeconds(checkCooldown);
        checking = false;

    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pick_It_Up_Offline : MonoBehaviour {

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
    private Transform topPosition;
    private Transform bottomPosition;
    private Rigidbody rb;
    private bool mounted = false;
    public float climbSpeed = 2f;
    // Use this for initialization
    void Start()
    {
        mask = ~mask;
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        if (mounted)
        {
            if (transform.position.y > bottomPosition.position.y && transform.position.y < topPosition.position.y)
            {
                float moveY = Input.GetAxis("Vertical");
                Vector3 velocity = new Vector3(0, climbSpeed * moveY, 0);
                rb.MovePosition(rb.position + velocity * Time.deltaTime);
            }
            else
            {
                UnMountObject();
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            
            if (canMount)
            {
                if(!mounted)
                {
                    MountObject();
                    return;
                }
            }
            if (mounted)
            {
                UnMountObject();
            }
        }
        
        if (!checking)
        {
            checking = true;
            CheckForObject();
            StartCoroutine(CheckCoolDown());
        }

    }
    void UnMountObject()
    {
        mounted = false;
        rb.AddForce(transform.forward * 1f, ForceMode.Impulse);
        PlayerState scrpt = GetComponent<PlayerState>();
        if(scrpt != null)
        {
            scrpt.mounted = false;
        }
        
        rb.useGravity = true;
    }
    void MountObject()
    {
        mounted = true;
        rb.useGravity = false;
        PlayerState scrpt = GetComponent<PlayerState>();
        if(scrpt != null)
        {
            scrpt.mounted = true;
        }
        transform.rotation = Quaternion.Euler(transform.rotation.x, mountPosition.root.eulerAngles.y, transform.rotation.z);
        transform.position = new Vector3(mountPosition.position.x, transform.position.y, mountPosition.position.z);
        
    }
    void CheckForObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, pickUpDistance, mask))
        {
            if (hit.collider.tag == "Item")
            {
                object_ = hit.transform.gameObject;
                canPickUp = true;
            }
            if (hit.transform.root.tag == "Interactable")
            {
                InteractableObjectDataPack pack = hit.transform.root.GetComponent<InteractableObjectControl>().dataPack;
                if (pack.Mountable)
                {
                    if (pack.MountType == 1)
                    {
                        canMount = true;
                        mountPosition = pack.MountPosition;
                        topPosition = pack.TopPosition;
                        bottomPosition = pack.BottomPosition;
                    }
                }
            }
        }
        else
        {
            canPickUp = false;
            object_ = null;
            canMount = false;
        }

    }
    IEnumerator CheckCoolDown()
    {
        yield return new WaitForSeconds(checkCooldown);
        checking = false;

    }
}

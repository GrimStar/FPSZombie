using UnityEngine;
using System.Collections;

public class InteractableObjectControl : MonoBehaviour {
	public InteractableObjectDataPack dataPack;
	public bool mountable;
    public int mountType;
    public Transform mountPosition;
    public Transform topPosition;
    public Transform bottomPosition;
    public bool door = false;
	// Use this for initialization
	void Start () {
        dataPack.MountType = mountType;
		dataPack.Mountable = mountable;
        dataPack.MountPosition = mountPosition;
        dataPack.Door = door;
        if (topPosition != null)
        {
            dataPack.TopPosition = topPosition;
            dataPack.BottomPosition = bottomPosition;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CanvasControl : MonoBehaviour {
	public static CanvasControl instance;
	[SerializeField]
	public static Text[] attachables;
	public Text[] _attachables;
	public GameObject usrInput;
	public GameObject player;
	public static GameObject _inventory;
	public static GameObject _secondarySlot;
	public static GameObject _hotbar;
    public static GameObject _primarySlotOne;
    public static GameObject _primarySlotTwo;
    public static GameObject _meleeSlot;
    public GameObject _explosiveOne;
    public GameObject _explosiveTwo;
    public ScreenImageAlphaControl _speedImage;
	public GameObject playerInventory;
	public GameObject playerHotbar;
    public GameObject primarySlotOne;
    public GameObject primarySlotTwo;
	public GameObject secondarySlot;
    public GameObject meleeSlot;
	// Use this for initialization
	void Awake () {
		_inventory = playerInventory;
		_secondarySlot = secondarySlot;
		_hotbar = playerHotbar;
        _primarySlotOne = primarySlotOne;
        _primarySlotTwo = primarySlotTwo;
        _meleeSlot = meleeSlot;
		if (instance == null) {
			instance = this;
		}
		attachables = new Text[4];

		Setup ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void Setup () {
		int i = 0;
		foreach (Text _text in _attachables) {
			attachables [i] = _text;
			i++;
		}
	}

	
}

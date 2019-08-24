using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class InventoryStackControl : MonoBehaviour {
	public int amount = 1;
	public Text displayAmount;
	public int stackLimit = 0;
	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
	
	}
	public void DecreaseStack (int _amount) {
		Debug.Log ("DecreaseStack");
		amount -= _amount;
		if (amount <= 0) {
			Destroy (this.gameObject);
		}
		displayAmount.text = amount.ToString ();
	}
	public void IncreaseStack (int _amount) {
		Debug.Log ("IncreaseStack");
		amount += _amount;
		displayAmount.text = amount.ToString ();
	}

}

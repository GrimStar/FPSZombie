using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
public class Player : NetworkBehaviour {


	[SerializeField]
	private int maxHealth = 100;
	[SyncVar]
	private int currentHealth;

	[SyncVar]
	private bool _isDead = false;
	public bool isDead
	{
		get{ return _isDead; }
		protected set { _isDead = value;}
	}
	[SerializeField]
	private Behaviour[] disableOnDeath;
	private bool[] wasEnabled;
	public int playerID;
	public void Setup() {
		wasEnabled = new bool[disableOnDeath.Length];
		for (int i = 0; i < wasEnabled.Length; i++) {
			wasEnabled [i] = disableOnDeath [i].enabled;
		}
		SetDefaults ();
		Debug.Log ("Defaults Set");
	}

	IEnumerator Respawn() {

		yield return new WaitForSeconds (GameManager.instance.matchSettings.respawnTime);

		SetDefaults ();
		Transform _startPosition = NetworkManager.singleton.GetStartPosition ();
		transform.position = _startPosition.position;
		transform.rotation = _startPosition.rotation;

		Debug.Log (transform.name + " Respawned!");
	}

	[ClientRpc]
	public void RpcTakeDamage(int _amount) {
		if (isDead) {
			return;
		}

		currentHealth -= _amount;
		Debug.Log (currentHealth);

		if (currentHealth <= 0) {

			Die();

		}
	}
	private void Die() {

		isDead = true;

		//Disable components
		Debug.Log(transform.name + " is DEAD!");
		for (int i = 0; i < disableOnDeath.Length; i++) {
			disableOnDeath [i].enabled = false;
		}
		Collider _col = GetComponent<Collider> ();
		if (_col != null) {

			_col.enabled = false;
		}
		StartCoroutine (Respawn ());
	}
	public void SetDefaults() {

		isDead = false;

		currentHealth = maxHealth;

		for (int i = 0; i < disableOnDeath.Length; i++) {
			disableOnDeath [i].enabled = wasEnabled [i];
		}
		Collider _col = GetComponent<Collider> ();
		if (_col != null) {

			_col.enabled = true;
		}
	}
}

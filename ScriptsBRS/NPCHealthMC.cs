using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHealthMC : MonoBehaviour, IDamagable {

    public bool dead = false;
    const float maxHealth = 200;
    private float currentHealth;
    
    float takingDamageTimer = 0f;
    [SerializeField]
    float addToDamageTimer = 0.5f;
    public bool takingDamage = false;
	// Use this for initialization
	void Start () {
        currentHealth = maxHealth;
        
	}
	
	// Update is called once per frame
	void Update () {
		if(takingDamageTimer <= 0)
        {
            takingDamage = false;
            takingDamageTimer = 0f;
        }
        if(takingDamageTimer > 0)
        {
            takingDamage = true;
            takingDamageTimer -= Time.deltaTime;
        }
	}
    public void ITakeDamage(int _damage)
    {
        if (!dead)
        {
            takingDamageTimer += addToDamageTimer;
            currentHealth -= _damage;
            if (currentHealth <= 0)
            {
                dead = true;
                ProgressTracker.instance.Kills += 1;

            }
        }
    }
}

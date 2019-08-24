using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour {

    public static GameData instance;
    public GameObject weaponHolder;
    public GameObject player;
    public Transform closestEnemy;
    public int waveCount;
    public List<WeaponSpawner> weaponSpawners = new List<WeaponSpawner>();
    public int currentSlot = 0;
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
}

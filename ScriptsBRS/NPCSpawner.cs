using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour {

    [SerializeField]
    float spawnInterval = 10f;
    [SerializeField]
    int maxSpawns = 5;
    int spawns = 0;
    bool readyToSpawn = true;
    [SerializeField]
    GameObject defPrefab;
  
    [SerializeField]
    Transform[] destinations;
    [SerializeField]
    float startDelay = 0f;
    [SerializeField]
    AudioSource _audio;
    [SerializeField]
    AudioClip spawnSound;
	// Use this for initialization
	void Start () {
        readyToSpawn = false;
        StartCoroutine(StartDelay());
	}
	
	// Update is called once per frame
	void Update () {
        if (readyToSpawn)
        {
            if (!ProgressTracker.instance.changingWave)
            {
                if (ProgressTracker.instance.Spawned < ProgressTracker.instance.waveEmemyCount)
                {
                    StartCoroutine(SpawnInterval());
                    readyToSpawn = false;
                }
            }
        }
		
	}
    void SpawnNPC(GameObject _prefab)
    {

        ProgressTracker.instance.Spawned += 1;
        spawns++;
        GameObject newObject = (GameObject)Instantiate(_prefab);
        newObject.transform.position = transform.position;
        NPCZombie zombieScript = newObject.GetComponent<NPCZombie>();
        NPCHumanoidRobot robotScript = newObject.GetComponent<NPCHumanoidRobot>();
        if (zombieScript != null)
        {
            zombieScript.destinations = destinations;
            if (ProgressTracker.instance.spawned == ProgressTracker.instance.waveEmemyCount)
            {
                zombieScript.lastSpawn = true;
            }
        }        
        if(robotScript != null)
        {
            robotScript.destinations = destinations;
            if (ProgressTracker.instance.spawned == ProgressTracker.instance.waveEmemyCount)
            {
                robotScript.lastSpawn = true;
            }
        }
        _audio.clip = spawnSound;
        _audio.Play();
        
        
    }
    IEnumerator SpawnInterval()
    {
        yield return new WaitForSeconds(spawnInterval);
        if (ProgressTracker.instance.Spawned < ProgressTracker.instance.waveEmemyCount)
        {


            SpawnNPC(defPrefab);

        }
        readyToSpawn = true;
    }
    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(startDelay);
        readyToSpawn = true;
    }
}

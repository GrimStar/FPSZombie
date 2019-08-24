using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCard : MonoBehaviour {

    [SerializeField]
    Material[] mats;
    public int keyLevel = 0;
    public int KeyLevel
    {
        get
        {
            return keyLevel;
        }
        set
        {
            keyLevel = value;
            
        }
    }
    

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
   
}

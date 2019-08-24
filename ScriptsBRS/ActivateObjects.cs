using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateObjects : MonoBehaviour {

    [SerializeField]
    Transform[] objectsToActivate;
	// Use this for initialization
	
    public void Activate()
    {
        foreach(Transform _object in objectsToActivate)
        {
            if (!_object.gameObject.activeSelf)
            {
                _object.gameObject.SetActive(true);
            }
        }
    }
}

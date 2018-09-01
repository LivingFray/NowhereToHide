using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    //temp
    public GameObject pistol;

    //The gameobject that contains weapons in the view model
    public GameObject gunHolder;

    Gun gun;

    // Use this for initialization
    void Start () {
        gun = pistol.GetComponent<Gun>();
	}
	
	// Update is called once per frame
	void Update () {
        gun.triggerHeld = Input.GetButton("Fire1");
	}
}

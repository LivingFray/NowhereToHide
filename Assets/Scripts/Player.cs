using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity {
    //temp
    public GameObject pistol;

    //The gameobject that contains weapons in the view model
    public GameObject gunHolder;

    Gun gun;

    bool mouseLocked;

    // Use this for initialization
    protected override void OnStart () {
        gun = pistol.GetComponent<Gun>();
        mouseLocked = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    protected override void OnUpdate() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            mouseLocked = !mouseLocked;
            if (mouseLocked) {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            } else {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
	
	// Update is called once per frame
	//void Update () {
     //   gun.triggerHeld = Input.GetButton("Fire1");
	//}
}

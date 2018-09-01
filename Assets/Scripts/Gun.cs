using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

    public GunScriptable gunProperties;

    int ammo;
    int clip;
    float lastFired;

    public bool triggerHeld;

    void Start () {
        ammo = gunProperties.maxAmmo;
        clip = gunProperties.maxClip;
        lastFired = 0;
	}

    //Try to shoot after we know if the trigger is down
    void LateUpdate() {
        CheckFire();
    }

    void CheckFire() {
        lastFired += Time.deltaTime;
        //Check if trigger is held
        if (!triggerHeld) {
            return;
        }
        //Check if gun can fire yet
        if (lastFired > gunProperties.fireRate) {
            //Reset timer
            lastFired = 0;
            //Actually shoot
            Fire();
            //If not full auto, prevent next shot
            if (!gunProperties.auto) {
                triggerHeld = false;
            }
        }
    }

    void Fire() {
        Debug.Log("Pew");
    }
}

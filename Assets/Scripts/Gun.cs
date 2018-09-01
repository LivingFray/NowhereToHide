using System;
using UnityEngine;

public class Gun : MonoBehaviour {

    public GunScriptable gunProperties;

    public GameObject projectileOrigin;

    public ParticleSystem muzzle;

    //[HideInInspector]
    public Entity owner;

    Transform projTransform;

    int ammo;
    int clip;
    float lastFired;

    public bool triggerHeld;

    float reloading;

    void Start () {
        ammo = gunProperties.maxAmmo;
        clip = gunProperties.maxClip;
        lastFired = 0;
        reloading = 0.0f;
        projTransform = projectileOrigin.transform;
	}

    //Try to shoot after we know if the trigger is down
    void LateUpdate() {
        AttemptReload();
        CheckFire();
    }

    void AttemptReload() {
        if(reloading > 0.0f) {
            reloading -= Time.deltaTime;
            if(reloading <= 0.0f) {
                reloading = 0.0f;
                ammo = Math.Min(clip, gunProperties.maxAmmo);
                clip -= ammo;
                Debug.Log(ammo + "/" + clip);
            }
        }
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
        if(reloading > 0.0f) {
            return;
        }
        if(ammo == 0) {
            return;
        }
        GameObject proj = Instantiate(gunProperties.projectile, projTransform.position + projTransform.forward * 0.5f, projTransform.rotation);
        Projectile projectile = proj.GetComponent<Projectile>();
        projectile.damage = gunProperties.damage;
        projectile.owner = owner;
        projectile.startPos = muzzle.transform.position;
        ammo--;
        muzzle.Play();
        Debug.Log(ammo + "/" + clip);
        if(ammo == 0 && clip > 0) {
            reloading = gunProperties.reloadTime;
        }
    }
}

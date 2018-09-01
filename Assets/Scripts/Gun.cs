using UnityEngine;

public class Gun : MonoBehaviour {

    public GunScriptable gunProperties;

    public GameObject projectileOrigin;

    public ParticleSystem muzzle;

    AudioSource audioSource;

    //[HideInInspector]
    public Entity owner;

    Transform projTransform;

    int ammo;
    int clip;
    float lastFired;

    public bool triggerHeld;

    float reloading;

    bool hasShot;

    void Start () {
        ammo = gunProperties.maxAmmo;
        clip = gunProperties.maxClip;
        lastFired = 0;
        reloading = 0.0f;
        projTransform = projectileOrigin.transform;
        audioSource = GetComponent<AudioSource>();
        hasShot = false;
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
                ammo = System.Math.Min(clip, gunProperties.maxAmmo);
                clip -= ammo;
                Debug.Log(ammo + "/" + clip);
            }
        }
    }

    void CheckFire() {
        lastFired += Time.deltaTime;
        //Check if trigger is held
        if (!triggerHeld) {
            hasShot = false;
            return;
        }
        if (hasShot && !gunProperties.auto) {
            return;
        }
        //Check if gun can fire yet
        if (lastFired > gunProperties.fireRate) {
            //Reset timer
            lastFired = 0;
            //Actually shoot
            Fire();
            //If not full auto, prevent next shot
            hasShot = true;
        }
    }

    void Fire() {
        //Checks that gun can be shot
        if(reloading > 0.0f || ammo == 0) {
            audioSource.pitch = Random.Range(gunProperties.emptyPitchMin, gunProperties.emptyPitchMax);
            audioSource.PlayOneShot(gunProperties.empty, gunProperties.emptyVolume);
            return;
        }
        //Create projectile
        GameObject proj = Instantiate(gunProperties.projectile, projTransform.position + projTransform.forward * 0.5f, projTransform.rotation);
        Projectile projectile = proj.GetComponent<Projectile>();
        projectile.damage = gunProperties.damage;
        projectile.owner = owner;
        projectile.startPos = muzzle.transform.position;
        //Expend ammo
        ammo--;
        Debug.Log(ammo + "/" + clip);
        //Pretty effects here
        muzzle.Play();
        audioSource.pitch = Random.Range(gunProperties.firePitchMin, gunProperties.firePitchMax);
        audioSource.PlayOneShot(gunProperties.fire, gunProperties.fireVolume);
        //Auto-reload
        if (ammo == 0 && clip > 0) {
            reloading = gunProperties.reloadTime;
        }
    }
}

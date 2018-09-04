using UnityEngine;

public class Gun : MonoBehaviour {

    public GunScriptable gunProperties;

    public GameObject projectileOrigin;

    public ParticleSystem muzzle;

    AudioSource audioSource;

    //[HideInInspector]
    public Entity owner;

    Transform projTransform;

    public int Ammo { get; private set; }
    public int Clip { get; private set; }
    float lastFired;

    public bool triggerHeld;

    float reloading;

    bool hasShot;

    void Start () {
        Ammo = gunProperties.maxAmmo;
        Clip = gunProperties.maxClip;
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

    public void Respawn() {
        Ammo = gunProperties.maxAmmo;
        Clip = gunProperties.maxClip;
    }

    public bool PickupAmmo(int count) {
        if(Clip == gunProperties.maxClip) {
            return false;
        }
        Clip += count;
        if(Clip > gunProperties.maxClip) {
            Clip = gunProperties.maxClip;
        }
        return true;
    }

    void AttemptReload() {
        if(reloading > 0.0f) {
            reloading -= Time.deltaTime;
            if(reloading <= 0.0f) {
                reloading = 0.0f;
                Ammo = System.Math.Min(Clip, gunProperties.maxAmmo);
                Clip -= Ammo;
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
        if(owner.isXRaying) {
            return;
        }
        //Checks that gun can be shot
        if(reloading > 0.0f || Ammo == 0) {
            audioSource.pitch = Random.Range(gunProperties.emptyPitchMin, gunProperties.emptyPitchMax);
            audioSource.PlayOneShot(gunProperties.empty, gunProperties.emptyVolume);
            return;
        }
        Quaternion spread = Quaternion.Euler(new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0.0f).normalized * gunProperties.inaccuracy);
        //Create projectile
        GameObject proj = Instantiate(gunProperties.projectile, projTransform.position + projTransform.forward * 0.5f, projTransform.rotation * spread);
        Projectile projectile = proj.GetComponent<Projectile>();
        projectile.damage = gunProperties.damage;
        projectile.owner = owner;
        projectile.startPos = muzzle.transform.position;
        //Expend ammo
        Ammo--;
        //Pretty effects here
        muzzle.Play();
        audioSource.pitch = Random.Range(gunProperties.firePitchMin, gunProperties.firePitchMax);
        audioSource.PlayOneShot(gunProperties.fire, gunProperties.fireVolume);
        //Recoil
        owner.ApplyRecoil(gunProperties.recoil);
        //Auto-reload
        if (Ammo == 0 && Clip > 0) {
            reloading = gunProperties.reloadTime;
        }
    }

    public void Reload() {
        if(reloading <= 0.0f) {
            Clip += Ammo;
            Ammo = 0;
            reloading = gunProperties.reloadTime;
            audioSource.pitch = Random.Range(gunProperties.reloadPitchMin, gunProperties.reloadPitchMax);
            audioSource.PlayOneShot(gunProperties.reload, gunProperties.reloadVolume);
        }
    }
}

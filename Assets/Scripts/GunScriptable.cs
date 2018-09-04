using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Weapons/Gun")]
public class GunScriptable : ScriptableObject {
    //The amount of damage the weapon deals on hit
    public int damage;
    //The angle change recoil causes
    public float recoil;
    //The total amount of ammo held in 1 clip
    public int maxAmmo;
    //The maximum amount of ammo stored in reserve
    public int maxClip;
    //Whether the gun is automatic
    public bool auto;
    //The time between shots
    public float fireRate;
    //The gameobject firing the gun spawns
    public GameObject projectile;
    //The maximum angle a projectile may deviate by
    public float inaccuracy;
    //The time it takes to reload the gun
    public float reloadTime;

    public AudioClip fire;
    public float firePitchMin;
    public float firePitchMax;
    public float fireVolume;

    public AudioClip reload;
    public float reloadPitchMin;
    public float reloadPitchMax;
    public float reloadVolume;

    public AudioClip empty;
    public float emptyPitchMin;
    public float emptyPitchMax;
    public float emptyVolume;
}

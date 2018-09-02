using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour {

    public int ammo;

    public float minRespawn;

    public float maxRespawn;

    float pickup;

    public void OnTriggerEnter(Collider other) {
        Entity ent = other.GetComponent<Entity>();
        if (ent != null) {
            if(ent.equippedGun.PickupAmmo(ammo)) {
                transform.position -= new Vector3(0.0f, 1000.0f, 0.0f);
                pickup = Random.Range(minRespawn, maxRespawn);
            }
        }
    }

    private void Update() {
        if (pickup > 0.0f) {
            pickup -= Time.deltaTime;
            if(pickup <= 0.0f) {
                transform.position += new Vector3(0.0f, 1000.0f, 0.0f);
                pickup = 0.0f;
            }
        }
    }
}

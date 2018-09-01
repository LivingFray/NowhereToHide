using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public int damage;

    public GameObject owner;

    protected void OnHit(GameObject target) {
        Entity entity = target.GetComponent<Entity>();
        if(entity != null) {
            entity.Damage(damage);
        }
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public int damage;

    public Entity owner;

    public Vector3 startPos;

    protected void OnHit(GameObject target) {
        Entity entity = target.GetComponent<Entity>();
        if(entity != null) {
            owner.OnHit();
            if(entity.Damage(damage)) {
                //Update scores
            }
        }
    }
}

using UnityEngine;

public class RayProjectile : Projectile {

	void Start () {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit)) {
            OnHit(hit.transform.gameObject);
        }
	}
}

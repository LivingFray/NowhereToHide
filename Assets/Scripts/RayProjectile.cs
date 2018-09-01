using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayProjectile : Projectile {

    public GameObject temp;

	void Start () {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit)) {
            Instantiate(temp, hit.point, Quaternion.identity);
        }
	}
}

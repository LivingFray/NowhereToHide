using UnityEngine;

public class RayProjectile : Projectile {

    public float duration;

	void Start () {
        RaycastHit hit;
        Vector3 dest = transform.position + transform.forward * 400.0f;
        if(Physics.Raycast(transform.position, transform.forward, out hit, 400.0f, ~(1 << LayerMask.NameToLayer("LevelGeometry")))) {
            OnHit(hit.transform.gameObject);
            dest = hit.point;
        }
        GetComponent<LineRenderer>().SetPositions(new Vector3[]{ startPos, dest});
        Destroy(gameObject, duration);
	}
}

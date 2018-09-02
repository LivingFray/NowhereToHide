using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : ScriptableObject {

    public float speed;

    public virtual void OnUpdate(Entity entity) {}
    public virtual void OnFixedUpdate(Entity entity) {}
    public virtual void OnDied(Entity entity) {
        entity.OnDied();
        entity.gameController.EntityDied(entity);
    }
    public virtual void OnRespawn(Entity entity) {}

    protected void UpdateRotation(Entity entity) {
        //Clamp angles
        entity.lookAngle.x = Mathf.Clamp(entity.lookAngle.x, -90.0f, 90.0f);

        if (entity.lookAngle.y > 360.0f) {
            entity.lookAngle.y -= 360.0f;
        }

        if (entity.lookAngle.y < 0.0f) {
            entity.lookAngle.y += 360.0f;
        }

        entity.head.transform.localRotation = Quaternion.Euler(new Vector3(entity.lookAngle.x, 0.0f, 0.0f));
        entity.rigidbody.MoveRotation(Quaternion.Euler(new Vector3(0.0f, entity.lookAngle.y, 0.0f)));
    }
}

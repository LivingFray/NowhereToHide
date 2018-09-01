using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : ScriptableObject {

    public float speed;

    public virtual void OnUpdate(Entity entity) {}
    public virtual void OnFixedUpdate(Entity entity) {}
    public virtual void OnDied(Entity entity) {}
}

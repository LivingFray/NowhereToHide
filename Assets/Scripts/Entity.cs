using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Entity : MonoBehaviour {

    public int Health { get; private set; }

    public EntityController entityController;

    public GameObject head;

    [HideInInspector]
    new public Rigidbody rigidbody;

    public Vector3 lookAngle;

	// Use this for initialization
	void Start () {
        Health = 100;
        rigidbody = GetComponent<Rigidbody>();
        OnStart();
	}

    protected virtual void OnStart() { }
    protected virtual void OnUpdate() { }

    void Update () {
        entityController.OnUpdate(this);
        OnUpdate();
	}

    void FixedUpdate() {
        entityController.OnFixedUpdate(this);
    }

    public void Damage(int damage) {
        Health -= damage;
        if(Health < 0) {
            Health = 0;
            //TODO: Respawn
            //entityController.OnDied(this);
        }
    }
}

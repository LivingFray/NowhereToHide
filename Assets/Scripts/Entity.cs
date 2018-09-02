using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class Entity : MonoBehaviour {

    public int Health { get; private set; }

    public EntityController entityController;

    public GameObject head;

    [HideInInspector]
    new public Rigidbody rigidbody;

    [HideInInspector]
    new public CapsuleCollider collider;

    public Vector3 lookAngle;

    public Gun equippedGun;

    //Useful information for controlling entities
    [HideInInspector]
    public bool canJump;
    [HideInInspector]
    public Entity currentTarget;
    [HideInInspector]
    public float targettingTime;
    [HideInInspector]
    public Vector3 currentGoal;
    [HideInInspector]
    public bool wandering;
    [HideInInspector]
    public float idleTimer;
    [HideInInspector]
    public float fireDelay;
    [HideInInspector]
    public bool hasShot;

    [HideInInspector]
    public NavMeshAgent navMeshAgent;

    [HideInInspector]
    public GameController gameController;

    private void Awake() {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    void Start() {
        Health = 100;
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        currentGoal = transform.position;
        targettingTime = 0.0f;
        idleTimer = 0.0f;
        OnStart();
    }

    protected virtual void OnStart() { }
    protected virtual void OnUpdate() { }

    void Update() {
        canJump = Physics.CheckCapsule(collider.bounds.center, new Vector3(collider.bounds.center.x, collider.bounds.min.y - 0.1f, collider.bounds.center.z), collider.radius / 2.0f, 1 << LayerMask.NameToLayer("LevelGeometry"));
        entityController.OnUpdate(this);
        OnUpdate();
    }

    void FixedUpdate() {
        entityController.OnFixedUpdate(this);
    }

    //Returns true if the enemy was killed
    public virtual bool Damage(int damage) {
        Health -= damage;
        if (Health <= 0) {
            Health = 0;
            //TODO: Respawn
            entityController.OnDied(this);
            return true;
        }
        return false;
    }

    //Called when entity successfully shoots a target
    public virtual void OnHit() { }

    public void ApplyRecoil(float recoil) {
        lookAngle.x -= recoil;
    }
}

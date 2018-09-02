using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class Entity : MonoBehaviour {

    public int Health { get; private set; }

    public EntityController entityController;

    public GameObject xray;

    public GameObject head;

    [HideInInspector]
    public GameController gameController;

    public Vector3 lookAngle;

    public Gun equippedGun;

    //Useful information for controlling entities
    [HideInInspector]
    public bool canJump;
    [HideInInspector]
    public Entity currentTarget;
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
    public Vector3 lastSeen;
    [HideInInspector]
    public Vector3 lastMoving;
    [HideInInspector]
    public float guessingTime;
    [HideInInspector]
    public float respawnTime;
    [HideInInspector]
    public Vector3 velocity;
    [HideInInspector]
    public bool isXRaying;

    public int kills;
    public int deaths;

    //Component References
    [HideInInspector]
    public NavMeshAgent navMeshAgent;
    [HideInInspector]
    new public Rigidbody rigidbody;
    [HideInInspector]
    new public CapsuleCollider collider;

    private void Awake() {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Start() {
        currentGoal = transform.position;
        OnStart();
    }

    public virtual void OnRespawn() {
        idleTimer = 0.0f;
        Health = 100;
        currentTarget = null;
        wandering = false;
        idleTimer = 0.0f;
        fireDelay = 0.0f;
        hasShot = false;
        guessingTime = 0.0f;
        respawnTime = 0.0f;
        entityController.OnRespawn(this);
    }

    protected virtual void OnStart() { }
    protected virtual void OnUpdate() { }

    void Update() {
        canJump = Physics.CheckCapsule(collider.bounds.center, new Vector3(collider.bounds.center.x, collider.bounds.min.y - 0.1f, collider.bounds.center.z), collider.radius / 2.0f, 1 << LayerMask.NameToLayer("LevelGeometry"));
        entityController.OnUpdate(this);
        OnUpdate();
        xray.SetActive(isXRaying);
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
    public virtual void OnHit() {}

    //Called when entity dies
    public virtual void OnDied() {}

    public void ApplyRecoil(float recoil) {
        lookAngle.x -= recoil;
    }
}

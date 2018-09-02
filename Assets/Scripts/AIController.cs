using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "New AI Controller", menuName = "Controllers/AI")]
public class AIController : EntityController {

    public float goalDistanceFudge;

    public float distanceWeighting;

    public float angleWeighting;

    public float wanderRange;

    public float mapSizeX;

    public float mapSizeZ;

    public float maxTurnSpeed;

    public float minTargetTime;

    public float maxTargetTime;

    public float accuracy;

    public float continuedAccuracy;

    public float minFireDelay;

    public float maxFireDelay;

    public float maxGuessTime;

    public float moveCloserChance;

    public override void OnDied(Entity entity) {
        entity.gameObject.SetActive(false);
    }

    public override void OnUpdate(Entity entity) {
        //Use a decision tree to determine current action
        HandleAI(entity);
        //Update velocity
        entity.velocity = entity.navMeshAgent.velocity;
    }

    void HandleAI(Entity entity) {
        if (entity.currentTarget == null) { //No Target
            Debug.Log("AI: Searching for new target");
            FindTarget(entity);
            if (entity.currentTarget == null) { //No new targets
                Debug.Log("AI: Wandering");
                Wander(entity);
            } else { //Tracking new target
                Debug.Log("AI: Target found");
                //TODO: Move to target
                entity.idleTimer = Random.Range(minTargetTime, maxTargetTime);
                entity.fireDelay = Random.Range(minFireDelay, maxFireDelay);
            }
        } else { //Continue tracking
            Debug.Log("AI: Tracking Target");
            AimAtTarget(entity);
            //Temp?
            Wander(entity);
        }
    }

    void FindTarget(Entity entity) {
        //Loop through and find all visible entities
        List<Entity> possibleTargets = new List<Entity>();
        foreach (Entity ent in entity.gameController.entities) {
            if (EntityVisible(entity, ent)) {
                possibleTargets.Add(ent);
            }
        }
        //Don't target anything if noone is visible
        if (possibleTargets.Count == 0) {
            return;
        }
        //Get "closest" target
        float lowest = Mathf.Infinity;
        Entity closest = possibleTargets[0];
        foreach (Entity ent in possibleTargets) {
            Vector3 dif = (ent.transform.position - entity.transform.position);
            float dist = dif.magnitude;
            float w = dist * -distanceWeighting;
            float cosAng = Vector3.Dot(entity.transform.forward, dif) / dist;
            w += angleWeighting * (1 - cosAng);
            if (w < lowest) {
                lowest = w;
                closest = ent;
            }
        }
        entity.currentTarget = closest;
        entity.wandering = false;
    }

    bool EntityVisible(Entity from, Entity to) {
        //Gonna have to settle with raycasts from eye to eye, even though this won't be quite true
        Vector3 offset = to.gameObject.transform.position - from.gameObject.transform.position;
        return !Physics.Raycast(from.gameObject.transform.position, offset, offset.magnitude, (1 << LayerMask.NameToLayer("LevelGeometry")));
    }

    void Wander(Entity entity) {
        //If reached current goal position, go somewhere else
        if (!entity.wandering || (entity.wandering && (entity.transform.position - entity.currentGoal).sqrMagnitude < goalDistanceFudge)) {
            CreateWanderGoal(entity);
            entity.wandering = true;
        }
    }

    void CreateWanderGoal(Entity entity) {
        Vector2 randMove = Random.insideUnitCircle * wanderRange;

        if (randMove.x < -mapSizeX / 2) {
            randMove.x += mapSizeX;
        } else if (randMove.x > mapSizeX / 2) {
            randMove.x += -mapSizeX;
        }
        if (randMove.y < -mapSizeZ / 2) {
            randMove.y += mapSizeZ;
        }
        if (randMove.y < -mapSizeZ / 2) {
            randMove.y += mapSizeZ;
        }

        Vector3 target = new Vector3(randMove.x, 0.0f, randMove.y);

        Vector3 centre;

        if(entity.currentTarget != null && Random.Range(0, 100) < moveCloserChance) {
            centre = entity.currentTarget.transform.position;
        } else {
            centre = entity.transform.position;
        }

        NavMeshHit hit;
        NavMesh.SamplePosition(target + centre, out hit, wanderRange, -1);

        entity.currentGoal = hit.position;
        entity.navMeshAgent.SetDestination(hit.position);
    }

    void AimAtTarget(Entity entity) {
        //Release trigger for non automatic
        if (entity.equippedGun.triggerHeld && !entity.equippedGun.gunProperties.auto) {
            entity.equippedGun.triggerHeld = false;
            entity.idleTimer = entity.equippedGun.gunProperties.fireRate;
        }
        //Release trigger for empty guns
        if (entity.equippedGun.Ammo == 0) {
            entity.idleTimer = entity.equippedGun.gunProperties.reloadTime + Random.Range(minTargetTime, maxTargetTime);
        }

        float dist = 0;

        //Check target is visible
        if(EntityVisible(entity, entity.currentTarget)) {
            dist = LookAtTarget(entity);
            entity.guessingTime = 0.0f;
            entity.lastSeen = entity.currentTarget.transform.position;
            entity.lastMoving = entity.currentTarget.velocity;
        } else {
            entity.guessingTime += Time.deltaTime;
            dist = GuessTarget(entity);
        }
       
        //Shoot periodically
        entity.idleTimer -= Time.deltaTime;
        float threshold = entity.hasShot ? continuedAccuracy : accuracy;
        if (entity.idleTimer <= 0.0f) {
            if (dist < threshold * threshold) {
                ShootAtTarget(entity);
            } else {
                entity.fireDelay = Random.Range(minFireDelay, maxFireDelay);
                entity.hasShot = false;
            }
        }
    }

    float LookAtTarget(Entity entity, Vector3 target) {
        //Rotate camera
        Vector3 diff = target - entity.transform.position;

        diff = diff.normalized;

        Vector3 lookDir = Quaternion.Euler(entity.lookAngle) * new Vector3(0.0f, 0.0f, 1.0f);

        Vector3 change = Vector3.RotateTowards(lookDir, diff, Mathf.Deg2Rad * maxTurnSpeed * Time.deltaTime, 0.0f);

        float horDist = change.z * change.z + change.x * change.x;
        horDist = Mathf.Sqrt(horDist);

        float dX = -Mathf.Atan2(change.y, horDist) * Mathf.Rad2Deg;
        float dY = Mathf.Atan2(change.x, change.z) * Mathf.Rad2Deg;

        float dist = (diff - change).magnitude;

        entity.lookAngle = new Vector3(dX, dY, 0.0f);
        UpdateRotation(entity);

        return dist;
    }

    float LookAtTarget(Entity entity) {
        return LookAtTarget(entity, entity.currentTarget.transform.position);
    }

    float GuessTarget(Entity entity) {
        Vector3 guess = entity.lastSeen + entity.lastMoving * entity.guessingTime;
        if(entity.guessingTime > maxGuessTime) {
            entity.currentTarget = null;
            Debug.Log("AI: Lost sight of target");
        }
        return LookAtTarget(entity, guess);
    }

    void ShootAtTarget(Entity entity) {
        entity.fireDelay -= Time.deltaTime;
        //First shot must have a delay to aim
        if (entity.fireDelay <= 0) {
            entity.equippedGun.triggerHeld = true;
            entity.hasShot = true;
        }
    }
}
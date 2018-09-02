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

    public override void OnDied(Entity entity) {
        entity.gameObject.SetActive(false);
    }

    public override void OnUpdate(Entity entity) {
        //Use a decision tree to determine current action
        HandleAI(entity);
    }

    void HandleAI(Entity entity) {
        if (entity.currentTarget == null) { //No Target
            Debug.Log("AI: Searching for new target");
            FindTarget(entity);
            if(entity.currentTarget == null) { //No new targets
                Debug.Log("AI: Wandering");
                Wander(entity);
            } else { //Tracking new target
                Debug.Log("AI: Target found");
                //TODO: Move to target
                entity.idleTimer = 0.0f;
            }
        } else { //Continue tracking

        }
    }

    void FindTarget(Entity entity) {
        //Loop through and find all visible entities
        List<Entity> possibleTargets = new List<Entity>();
        foreach(Entity ent in entity.gameController.entities) {
            if(EntityVisible(entity, ent)) {
                possibleTargets.Add(ent);
            }
        }
        //Don't target anything if noone is visible
        if(possibleTargets.Count == 0) {
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
            if(w < lowest) {
                lowest = w;
                closest = ent;
            }
        }
        //entity.currentTarget = closest;
        //entity.wandering = false;
    }

    bool EntityVisible(Entity from, Entity to) {
        //Gonna have to settle with raycasts from eye to eye, even though this won't be quite true
        //TODO: Maybe ai not see entities behind?
        return Physics.Raycast(from.gameObject.transform.position, to.gameObject.transform.position);
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

        if(randMove.x < -mapSizeX / 2) {
            randMove.x += mapSizeX;
        }else if (randMove.x > mapSizeX / 2) {
            randMove.x += -mapSizeX;
        }
        if (randMove.y < -mapSizeZ / 2) {
            randMove.y += mapSizeZ;
        }
        if (randMove.y < -mapSizeZ / 2) {
            randMove.y += mapSizeZ;
        }

        Vector3 target = new Vector3(randMove.x, 0.0f, randMove.y);

        NavMeshHit hit;
        NavMesh.SamplePosition(target + entity.transform.position, out hit, wanderRange, -1);

        entity.currentGoal = hit.position;
        entity.navMeshAgent.SetDestination(hit.position);
    }
}

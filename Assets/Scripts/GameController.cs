using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    [HideInInspector]
    public List<Entity> entities;

    public GameObject[] spawnLocations;

    public GameObject ai;

    public int numPlayers;

    public float respawnTime;

    void Start() {
        //Prevent players colliding
        int lp = LayerMask.NameToLayer("LocalPlayer");
        int ep = LayerMask.NameToLayer("Enemy");
        Physics.IgnoreLayerCollision(lp, ep);
        Physics.IgnoreLayerCollision(ep, ep);
        Physics.IgnoreLayerCollision(lp, lp);
        entities = new List<Entity> {
            //Add the player
            GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>()
        };
        //Add AIs
        for(int i = 0; i < numPlayers - 1; i++) {
            entities.Add(Instantiate(ai).GetComponent<Entity>());
        }
        foreach(Entity ent in entities) {
            RespawnEntity(ent);
        }
    }

    public void RespawnEntity(Entity entity) {
        //For now just pick a random spawn, there are many better ways to do this
        GameObject spawnAt = spawnLocations[Random.Range(0, spawnLocations.Length)];
        entity.transform.position = spawnAt.transform.position;
        entity.transform.rotation = spawnAt.transform.rotation;
        //Reinitialise
        entity.OnRespawn();
    }

    public void EntityDied(Entity entity) {
        entity.deaths++;
        entity.transform.position = new Vector3(0.0f, -100.0f, 0.0f);
        entity.respawnTime = respawnTime;
    }

    private void Update() {
        foreach(Entity ent in entities) {
            if(ent.respawnTime > 0.0f) {
                ent.respawnTime -= Time.deltaTime;
                if(ent.respawnTime <= 0.0f) {
                    RespawnEntity(ent);
                }
            }
        }
    }
}

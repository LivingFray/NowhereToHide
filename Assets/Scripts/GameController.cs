using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    [HideInInspector]
    public List<Entity> entities;

    public GameObject[] spawnLocations;

    public GameObject[] ammo;

    public GameObject ai;

    public Text scores;

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
        entities[0].name = "Player";
        //Add AIs
        for(int i = 1; i < numPlayers; i++) {
            entities.Add(Instantiate(ai).GetComponent<Entity>());
            entities[i].name = "Bot " + i;
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
        string sb = "Name\t\tKills\t\tDeaths";
        foreach(Entity ent in entities) {
            sb += "\n" + ent.name;
            for(int i = 0; i < 3 - (ent.name.Length / 4); i++) {
                sb += "\t";
            }
            sb += ent.kills + "\t\t\t" + ent.deaths;
            if(ent.respawnTime > 0.0f) {
                ent.respawnTime -= Time.deltaTime;
                if(ent.respawnTime <= 0.0f) {
                    RespawnEntity(ent);
                }
            }
        }
        scores.text = sb;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    [HideInInspector]
    public List<Entity> entities;

    public GameObject[] spawnLocations;

    public GameObject ai;

    public int numPlayers;

    void Start() {
        entities = new List<Entity> {
            //Add the player
            GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>()
        };
        //Add AIs
    }

    public void RespawnEntity(Entity entity) {
        //For now just pick a random spawn, there are many better ways to do this
        GameObject spawnAt = spawnLocations[Random.Range(0, spawnLocations.Length)];
        entity.transform.position = spawnAt.transform.position;
        entity.transform.rotation = spawnAt.transform.rotation;
        //Reinitialise
        
    }
}

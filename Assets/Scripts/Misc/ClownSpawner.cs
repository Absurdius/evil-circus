using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ClownSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public float spawnCooldown = 60f;
    public GameObject clownToSpawn;

    private float timeUntilSpawn;

    void Start()
    {
        timeUntilSpawn = spawnCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        timeUntilSpawn -= Time.deltaTime;
        if (timeUntilSpawn < 0)
        {
            timeUntilSpawn = spawnCooldown;
            SpawnClown();
        }
    }

    public void SpawnClown()
    {/*
        // find spawnpoint furthest away from player;
        Vector3 furthestAway = spawnPoints[0].position;
        for (int i = 1; i < spawnPoints.Length; i++)
        {

        } */
        Instantiate(clownToSpawn, spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)].position, transform.rotation);
    }
}

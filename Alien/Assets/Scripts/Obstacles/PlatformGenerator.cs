using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public float timeBetweenSpawns;
    public Vector3 direction;
    public float speed;
    public float despawnTime;

    private float spawnTimer;
    private Vector3 movement;

    void Start()
    {
        spawnTimer = timeBetweenSpawns;
        direction = direction.normalized;
        movement = direction * speed;
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0) {
            spawnTimer = timeBetweenSpawns;
            GameObject platform = Instantiate(gameObject);
            Destroy(platform.GetComponent<PlatformGenerator>());
            LinearController platController = platform.AddComponent<LinearController>();
            platController.movement = movement;
            platController.despawnTimer = despawnTime;
        }
            
    }

    
}

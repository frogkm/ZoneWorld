using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private List<Waypoint> waypoints;
    [SerializeField] private float precision = 0.05f;
    [SerializeField] private float spawnDelay = 3f;

    GameObject platformPrefab;

    private void SpawnPlatform() {
        GameObject platform = Instantiate(platformPrefab);
        platform.SetActive(true);
    }

    void Awake() {
        
    }

    void Start() {
        gameObject.SetActive(false);
        platformPrefab = Instantiate(gameObject);
        platformPrefab.transform.position = transform.position;
        gameObject.SetActive(true);
        Destroy(platformPrefab.GetComponent<PlatformSpawner>());
        MovingPlatform mvplatform = platformPrefab.AddComponent<MovingPlatform>();
        mvplatform.InitValues(waypoints, PlatformBehavior.destroy, precision);
        mvplatform.SetupPlatform();

        InvokeRepeating("SpawnPlatform", spawnDelay, spawnDelay);
        
    }

    
}

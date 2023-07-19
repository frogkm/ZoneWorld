using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] private float repathTime;
    [SerializeField] private Transform target;
    [SerializeField] private float speed;
    [SerializeField] private GridController gridController;

    private PathController pathController;
    //private float repathTimer;
    PathNode[] path;

    void Start()
    {
        pathController = GetComponent<PathController>();
        //repathTimer = repathTime;

        InvokeRepeating("TestPath", repathTime, repathTime);

    }

    private void TestPath() {
        var watch = System.Diagnostics.Stopwatch.StartNew();
        path = pathController.GetPath(target.position);
        watch.Stop();
        var elapsedMs = watch.ElapsedMilliseconds;
        Debug.Log(elapsedMs);
    }

    void Update()
    {
        if (path != null) {
            Vector3 direction = (gridController.GetNodePosition(path[0]) - transform.position).normalized;
            //transform.position = transform.position + (speed * Time.deltaTime * direction);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum PlatformBehavior {
    stop,
    destroy,
    reverse,
    repeat
}

[Serializable]
public class MovingPlatform : MonoBehaviour
{
    
    [SerializeField] private List<Waypoint> waypoints;
    [SerializeField] private PlatformBehavior behavior;
    [SerializeField] private float precision = 0.05f;
    //[SerializeField] private float spawnDelay = 3f;

    private int wpIdx;
    private Vector3 moveVector;
    private Vector3 startPosition;
    private bool platformSetup = false;

    public void InitValues(List<Waypoint> waypoints, PlatformBehavior behavior, float precision) {
        this.waypoints = waypoints;
        this.behavior = behavior;
        this.precision = precision;
    }

    private void Unparent() {
        if (transform.childCount > 0) {
            for (int i = 0; i < transform.childCount; i++) {
               if (transform.GetChild(i).tag == "Player") {
                    transform.GetChild(i).parent = null;
               } 
            }
        }
    }



    private void EndBehavior() {
        switch(behavior) {
            case PlatformBehavior.stop:
                this.enabled = false;
                break;
            case PlatformBehavior.destroy:
                Unparent();
                Destroy(gameObject);
                break;
            case PlatformBehavior.reverse:
                float holder = waypoints[0].speedTowards;
                for (int i = 0; i < waypoints.Count - 1; i++) {
                    waypoints[i].speedTowards = waypoints[i + 1].speedTowards;
                }
                waypoints[waypoints.Count - 1].speedTowards = holder;
                waypoints.Reverse();
                wpIdx = 1;
                AdjustMoveVector();
                break;
            case PlatformBehavior.repeat:
                wpIdx = 1;
                Unparent();
                transform.position = waypoints[0].wpTrans.position;
                AdjustMoveVector();
                break;

        }
    }

    private void AdjustMoveVector() {
        Vector3 direction = (waypoints[wpIdx].wpTrans.position - transform.position).normalized;
        moveVector = direction * waypoints[wpIdx].speedTowards;
    }

    //private void SpawnPlatform() {
    //    behavior = PlatformBehavior.destroy;
    //    //GameObject platform = Instantiate(gameObject);
    //    behavior = PlatformBehavior.spawner;
    //}

    private void InsertFirstWaypoint() {
        GameObject firstWp = new GameObject();
        firstWp.transform.position = transform.position;
        firstWp.transform.rotation = transform.rotation;
        firstWp.transform.localScale = transform.localScale;

        waypoints.Insert(0, new Waypoint(firstWp.transform, 0));
    }

    public void SetupPlatform() {
        platformSetup = true;
        InsertFirstWaypoint();
        wpIdx = 1;

        if (waypoints.Count == 1) {
            Debug.Log("ERND");
            EndBehavior();
            return;
        }
        
        AdjustMoveVector();
    }


    void Start()
    {
        //if (behavior == PlatformBehavior.spawner) {
        //    InvokeRepeating("SpawnPlatform", spawnDelay, spawnDelay);
        //}

        if (!platformSetup) 
            SetupPlatform();

    }

    void Update()
    {

        //if (behavior != PlatformBehavior.spawner) {
            
        if (Vector3.Distance(transform.position, waypoints[wpIdx].wpTrans.position) <= precision) {
            transform.position = waypoints[wpIdx].wpTrans.position;
            wpIdx += 1;
            if (wpIdx > waypoints.Count - 1) {
                EndBehavior();
                return;
            }
            AdjustMoveVector();
        }
        //}
    }

    void FixedUpdate() {
        //if (behavior != PlatformBehavior.spawner) {
        transform.position += moveVector * Time.fixedDeltaTime;
        //}
    }

    private void OnCollisionEnter(Collision other) {
        if (other.transform.tag == "Player") {
            for (int i = 0; i < other.contactCount; i++) {
                ContactPoint ctPoint = other.GetContact(i);
                if (ctPoint.normal.y < 0) {
                    other.transform.SetParent(transform);
                    break;
                }
            }
        }
    }

    private void OnCollisionExit(Collision other) {
        if (other.transform.tag == "Player" && other.transform.parent == transform) {
            other.transform.SetParent(null);
        }
    }
}

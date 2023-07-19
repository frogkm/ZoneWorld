using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Waypoint
{
    [SerializeField] public Transform wpTrans;
    [SerializeField] public float speedTowards = 2f;
    [SerializeField] public float waitTime = 0.5f;

    public Waypoint(Transform trans, float speed) {
        wpTrans = trans;
        speedTowards = speed;
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructorController : MonoBehaviour
{
    private void OnCollisionEnter(Collision other) {
        Destroy(other.gameObject);
        Debug.Log("HIT");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearController : MonoBehaviour
{
    public Vector3 movement;
    public float despawnTimer;

    void Start()
    {
        
    }

    void Update()
    {
        despawnTimer -= Time.deltaTime;
        if (despawnTimer <= 0) {
            Destroy(gameObject);
        }
    }

    void FixedUpdate() {
        transform.position += movement * Time.fixedDeltaTime;
    }

    private void OnCollisionEnter(Collision other) {
        //Debug.Log("HIT");
        if (other.transform.tag == "Player") {
            other.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision other) {
        if (other.transform.tag == "Player" && other.transform.parent == transform) {
            other.transform.SetParent(null);
        }
    }
}

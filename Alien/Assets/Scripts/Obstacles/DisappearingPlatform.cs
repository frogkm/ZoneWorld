using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    private void OnCollisionEnter(Collision other) {
        if (other.transform.tag == "Player") {
            GetComponent<Animator>().SetBool("touched", true);
        }
    }

    private void Disappear() {
        if (transform.childCount > 0) {
            for (int i = 0; i < transform.childCount; i++) {
               if (transform.GetChild(i).tag == "Player") {
                    transform.GetChild(i).parent = null;
               } 
            }
        }
        Destroy(gameObject);
    }
}

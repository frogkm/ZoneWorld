using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sucker : MonoBehaviour
{
    public Transform suckPoint;
    public Transform suckEnd;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToObstacle;

        RaycastHit[] hits = Physics.CapsuleCastAll(suckPoint.position, suckEnd.position, 2f, suckPoint.forward, 100f);

        foreach(RaycastHit hit in hits){

            //distanceToObstacle = hit.distance;
            Trash trash = hit.transform.GetComponent<Trash>();
            if (trash != null) {
                if (Vector3.Distance(hit.transform.position, suckPoint.position) < 2f) {
                    Destroy(hit.transform.gameObject);
                    continue;
                }
                //trash.transform.position += (new Vector3(0, 0.2f, 0));
                //Debug.Log("NICE");
                trash.transform.GetComponent<Rigidbody>().AddForce(-suckPoint.forward * Time.deltaTime * 300f * (1 / (Mathf.Sqrt(Vector3.Distance(hit.transform.position, suckPoint.position) + 1))), ForceMode.Impulse);// * (1 / Mathf.Sqrt(Mathf.Abs(hit.distance))));
            }
            
        }
        


    }

    void OnDrawGizmos()
    {
        
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(suckPoint.position, 1f);
        Gizmos.DrawSphere(suckEnd.position, 1f);

        Gizmos.color = Color.red;
        for (int i = 1; i < 15; i++) {
            Gizmos.DrawSphere(suckPoint.position + suckPoint.forward * i * (Vector3.Distance(suckPoint.position, suckEnd.position) / 15), 1f);
        }
    
    }
}

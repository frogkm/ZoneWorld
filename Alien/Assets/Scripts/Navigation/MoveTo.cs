// MoveTo.cs
using UnityEngine;
using UnityEngine.AI;

public class MoveTo : MonoBehaviour {
    
    public Transform goal;

    private NavMeshAgent agent;
    
    void Start () {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = goal.position; 
    }

    void Update() {
        agent.destination = goal.position; 
    }
}

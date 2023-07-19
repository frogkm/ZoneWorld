using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
    private SearchAlgorithm searchAlg;
    [SerializeField] private GridController gridController;
    //[SerializeField] private Transform targetTransform;
    private bool drawn = false;
    private PathNode[,] lastGrid;
    private PathNode[] lastPath;
    private PathNode lastStart;
    private PathNode lastTarget;


    void Start() {
        searchAlg = new AStarSearch(gridController.grid);
    }

    void Update() {
    }

    

    public PathNode[] GetPath(Vector3 target) {
        PathNode startNode = gridController.NodeFromWorldPoint(transform.position);
        PathNode targetNode = gridController.NodeFromWorldPoint(target);

        bool isGridSame = (lastStart != null && startNode.IsEqual(startNode) && lastTarget != null && lastTarget.IsEqual(targetNode)
                          && lastGrid != null && GridController.GridsEqual(lastGrid, gridController.grid));

        if (isGridSame) {
            Debug.Log("SAME");
            return lastPath;
            
        }
        
        lastGrid = GridController.CopyGrid(gridController.grid);
        lastStart = startNode;
        lastTarget = targetNode;
        lastPath = searchAlg.GetPath(startNode, targetNode);
        return lastPath;
    }

    void OnDrawGizmos() {
        /*
        if (searchAlg != null && !drawn) {
            drawn = true;
            path = GetPath(targetTransform.position);     
        }
        if (path != null) {
            foreach(PathNode node in path) {
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(gridController.GetNodePosition(node) + new Vector3(0, 1f, 0), gridController.nodeRadius);
            }
        }
        */

    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controls size, position, nodeSize, nodeCount, and more for a pathfinding grid
public class GridController : MonoBehaviour
{
    [Tooltip("Layer of obstacle objects")]
    [SerializeField] LayerMask unwalkableMask;
    [Tooltip("Real dimensions of grid")]
    [SerializeField] private Vector2 gridWorldSize;
    [Tooltip("Radius of each node on grid. This determines pathfinding resolution")]
    [SerializeField] public float nodeRadius;

    //PathNode array grid that this controller is responsible for
    public PathNode[,] grid;
    //Store nodeDiameter so it doesn't need recalculation
    private float nodeDiameter;
    //Store grid dimensions so they don't need recalculation
    private int numNodesX, numNodesY;
    //Store bottom left position of grid for easier calculations
    private Vector3 bottomLeft;

    //Set all utility variables
    void Awake() {
        nodeDiameter = nodeRadius * 2;
        numNodesX = (int) (gridWorldSize.x / nodeDiameter);
        numNodesY = (int) (gridWorldSize.y / nodeDiameter);
        grid = new PathNode[numNodesX, numNodesY];
        bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int i = 0; i < numNodesX; i++) {
            for (int j = 0; j < numNodesY; j++) {
                Vector3 worldPoint = bottomLeft + Vector3.right * (i * nodeDiameter + nodeRadius) + Vector3.forward * (j * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                //if (!walkable) {
                //    Debug.Log("NOT");
                //}
                grid[i,j] = new PathNode(grid, walkable, i, j);
            }
        } 
    }

    void Start() {
        
    }

    public PathNode NodeFromWorldPoint(Vector3 worldPosition) {
        worldPosition -= bottomLeft;

        float percentX = worldPosition.x / gridWorldSize.x;
        float percentY = worldPosition.z / gridWorldSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = (int)((numNodesX ) * percentX);
        int y = (int)((numNodesY ) * percentY);

        x = Mathf.Min(x, numNodesX - 1);
        y = Mathf.Min(y, numNodesY - 1);

        return grid[x, y];
    }

    public Vector3 GetNodePosition(PathNode node) {
        return new Vector3(bottomLeft.x + node.x * nodeDiameter + nodeRadius, transform.position.y, bottomLeft.z + node.y * nodeDiameter + nodeRadius);
    }

    public static PathNode[,] CopyGrid(PathNode[,] grid) {
        PathNode[,] newGrid = new PathNode[grid.GetLength(0), grid.GetLength(1)];
        for (int i = 0; i < grid.GetLength(0); i++) {
            for (int j = 0; j < grid.GetLength(1); j++) {
                newGrid[i,j] = new PathNode(newGrid, grid[i,j].walkable, grid[i,j].x, grid[i,j].y);
            }
        }
        return newGrid;
    }

    public static bool GridsEqual(PathNode[,] oldGrid, PathNode[,] newGrid) {
        if (oldGrid.GetLength(0) != newGrid.GetLength(0) || oldGrid.GetLength(1) != newGrid.GetLength(1)){
            return false;
        }

        for (int i = 0; i < oldGrid.GetLength(0); i++) {
            for (int j = 0; j < oldGrid.GetLength(1); j++) {
                if (!oldGrid[i,j].IsEqual(newGrid[i,j])) {
                    return false;
                }
            }
        }
        return true;
    }


    void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (grid != null) {
            //PathNode playerNode = NodeFromWorldPoint(player.position);
            //int[] gridPos = NodeToGridPos(playerNode);
            //Debug.Log(GetNodePosition(gridPos[0], gridPos[1]));
            foreach(PathNode node in grid) {
                Gizmos.color = (node.walkable)?Color.green:Color.red;
                //if (playerNode == node) {
                //    Gizmos.color = Color.cyan;
                //}
                Gizmos.DrawCube(GetNodePosition(node), new Vector3(nodeDiameter - 0.1f, nodeDiameter - 0.1f, nodeDiameter - 0.1f));
                
            }
        }

    }
}

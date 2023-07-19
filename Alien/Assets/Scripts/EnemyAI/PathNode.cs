using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  
PathNode object for denoting a position on a walkable grid  
GridController keeps an array of PathNodes
grid is a reference to this array so each pathNode can know it's own surrondings  
*/
public class PathNode {
    public bool walkable;
    public readonly int x;
    public readonly int y;
    private PathNode[,] grid;

    public PathNode(PathNode[,] grid, bool walkable, int x, int y) {
        this.grid = grid;
        this.walkable = walkable;
        this.x = x;
        this.y = y;
    }

    public bool IsEqual(PathNode other) {
        return (this.x == other.x && this.y == other.y && this.walkable == other.walkable);
    }
}

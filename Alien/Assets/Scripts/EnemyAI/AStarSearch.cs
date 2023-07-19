using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarSearch : SearchAlgorithm
{

    /*
    SearchNode is a type of node private to AStarSearch
    It denotes a node suited for astar search through an array of pathNodes
    Each SearchNode has a one to one relationship with a pathNode
    */
    private class SearchNode {
        public PathNode pathNode;
        public SearchNode parent;
        public float g;
        public float h;
        public float f;

        public SearchNode(PathNode pathNode, float h) {
            this.pathNode = pathNode;
            this.h = h;
        }

        public SearchNode(PathNode pathNode, float h, SearchNode parent){
            this.pathNode = pathNode;
            this.h = h;
            this.parent = parent;
        }
    }

    private SearchNode[,] grid;

    private SearchNode currStart;
    private SearchNode currTarget;

    public AStarSearch(PathNode[,] pathGrid) {
        grid = GenSearchGrid(pathGrid);
    }

    //Convert PathNode array into SearchNode array for use in this class
    private SearchNode[,] GenSearchGrid(PathNode[,] grid) {
        SearchNode[,] searchGrid = new SearchNode[grid.GetLength(0), grid.GetLength(1)];

        for (int i = 0; i < grid.GetLength(0); i++) {
            for (int j = 0; j < grid.GetLength(1); j++) {
                searchGrid[i, j] = new SearchNode(grid[i, j], 0);
            }
        }

        return searchGrid;
    }

    //Returns a SearchNode at specified x and y, or null if x or y is outside of range
    private SearchNode GetSearchNode(int x, int y) {
        if (x < 0 || x >= grid.GetLength(0) || y < 0 || y >= grid.GetLength(1)) {
            return null;
        }
        return grid[x, y];
    }

    //Returns all walkable neighbors of a given SearchNode
    //As of now it returns diagonals
    //may need to change this later to only retur
    private List<SearchNode> GetNeighbors(SearchNode node) {
        List<SearchNode> neighbors = new List<SearchNode>();

        for (int i = -1; i <= 1; i++) {
            for (int j = -1; j <= 1; j++) {
                int new_x = node.pathNode.x + i;
                int new_y = node.pathNode.y + j;
                SearchNode temp = GetSearchNode(new_x, new_y);

                bool isReachable = (temp != null) && temp.pathNode.walkable && !(i != 0 && j != 0 && !grid[new_x, new_y - j].pathNode.walkable && !grid[new_x - i, new_y].pathNode.walkable);

                if (isReachable || temp == currTarget) {
                    neighbors.Add(temp);
                }
            }
        }

        return neighbors;
    }

    //Loop back through all parent nodes till start node is reached
    private PathNode[] MakePath(SearchNode node) {
        List<PathNode> path = new List<PathNode>();

        while (node.parent != null) {
            path.Insert(0, node.pathNode);
            node = node.parent;
        }

        return path.ToArray();
    }


    //Returns a lisst of PathNodes denoting a path from start to target
    //Currently also includes start and target nodes, but may want to remove that in future
    public override PathNode[] GetPath(PathNode startNode, PathNode targetNode) {
        //Initialize frontier and closedSet
        List<SearchNode> frontier = new List<SearchNode>();
        HashSet<SearchNode> closedSet = new HashSet<SearchNode>();

        //Grab references to converted start and target nodes
        SearchNode start = grid[startNode.x, startNode.y];
        SearchNode target = grid[targetNode.x, targetNode.y];

        currStart = start;
        currTarget = target;

        //Set some inital values for start node
        start.h = Vector2.Distance(new Vector2(start.pathNode.x, start.pathNode.y), new Vector2(target.pathNode.x, target.pathNode.y));
        start.f = 0;
        start.g = 0;
        
        //Add start to frontier
        frontier.Add(start);

        //While frontier isn't empty, perform algorithm
        while (frontier.Count > 0) {

            //Grab node from frontier with lowest f cost (most likely to be closest to target)
            SearchNode currentNode = frontier[0];
            for (int i = 1; i < frontier.Count; i++) {
                if (frontier[i].f < currentNode.f || frontier[i].f == currentNode.f && frontier[i].h < currentNode.h) {
                    currentNode = frontier[i];
                }
            }

            //Remove currentNode from frontier
            frontier.Remove(currentNode);
            
            //Get nieghbors of currentNode
            List<SearchNode> neighbors = GetNeighbors(currentNode);

            //For each neighbor
            foreach (SearchNode neighbor in neighbors) {
                //If neighbor is target, we are done
                if (neighbor == target) {
                    neighbor.parent = currentNode;
                    return MakePath(neighbor);
                }

                //Calculate new possible g value for neighbor
                float g = currentNode.g + Vector2.Distance(new Vector2(neighbor.pathNode.x, neighbor.pathNode.y), new Vector2(currentNode.pathNode.x, currentNode.pathNode.y));
                
                //If new g is smaller than old g, set it and f to new values
                if ((neighbor.g == 0 || g < neighbor.g) && neighbor != start) {

                    //This is the new best path to this node, so set parent to currentNode
                    neighbor.parent = currentNode;
                    neighbor.g = g;

                    //If h hasn't been set yet, set it.  
                    //h doesn't ever change, but not all h's need to be set because not all nodes need to be explored
                    if (neighbor.h == 0) {
                        neighbor.h = Vector2.Distance(new Vector2(neighbor.pathNode.x, neighbor.pathNode.y), new Vector2(target.pathNode.x, target.pathNode.y));
                    }
                    neighbor.f = neighbor.g + neighbor.h;
                }

                //Booleans to see if node is in frontier or in closed set
                bool inFrontier = frontier.Contains(neighbor);
                bool inClosed = closedSet.Contains(neighbor);

                if (!inFrontier && !inClosed) {
                    frontier.Add(neighbor);
                }
                
            }

            //After we are done with currentNode, add it to closed set
            closedSet.Add(currentNode);

        }

        //Return null when a path is not possible
        return null;
    }
}

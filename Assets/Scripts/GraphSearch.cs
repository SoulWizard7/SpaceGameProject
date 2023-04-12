
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEditor;

public class GraphSearch
{

    public static BFSResult BFSGetRange(Dictionary<Vector2Int, HexData> grid, HexData startPoint, int movementPoints)
    {
        Dictionary<HexData, HexData?> visitedNodes = new Dictionary<HexData, HexData?>();
        Dictionary<HexData, int> costSoFar = new Dictionary<HexData, int>();
        Queue<HexData> nodesToVisitQueue = new Queue<HexData>();
        
        nodesToVisitQueue.Enqueue(startPoint);
        costSoFar.Add(startPoint, 0);
        visitedNodes.Add(startPoint, null);

        while (nodesToVisitQueue.Count > 0)
        {
            HexData currentNode = nodesToVisitQueue.Dequeue();
            foreach (var hex in currentNode.sHexData.neighbors)
            {
                if (!hex.sHexData.walkable) continue;

                int nodeCost = hex.GetCost();
                int currentCost = costSoFar[currentNode];
                int newCost = currentCost + nodeCost;

                if (newCost <= movementPoints)
                {
                    if (!visitedNodes.ContainsKey(hex))
                    {
                        visitedNodes[hex] = currentNode;
                        costSoFar[hex] = newCost;
                        nodesToVisitQueue.Enqueue(hex);
                    }
                    else if (costSoFar[hex] > newCost)
                    {
                        costSoFar[hex] = newCost;
                        visitedNodes[hex] = currentNode;
                    }
                }
            }
        }

        return new BFSResult{visitedNodesDict = visitedNodes};
    }

    public static List<HexData> GeneratePathBFS(HexData endHex, [ItemCanBeNull] Dictionary<HexData, HexData> visitedNodesDict)
    {
        List<HexData> path = new List<HexData>();
        
        if (!visitedNodesDict.ContainsKey(endHex))
        {
            return path;
        }
            
        path.Add(endHex);
        

        while (visitedNodesDict[endHex] != null)
        {
            path.Add(visitedNodesDict[endHex]);
            endHex = visitedNodesDict[endHex];
        }
        path.Reverse();
        return path;
        //return path.Skip(1).ToList();
    }
}

public struct BFSResult
{
    [ItemCanBeNull] public Dictionary<HexData, HexData> visitedNodesDict;

    public List<HexData> GetPathTo(HexData destination)
    {
        if (visitedNodesDict.ContainsKey(destination) == false)
            return new List<HexData>();
        return GraphSearch.GeneratePathBFS(destination, visitedNodesDict);
    }

    public bool IsHexPosInRange(HexData position)
    {
        return visitedNodesDict.ContainsKey(position);
    }

    public IEnumerable<HexData> GetRangePositions() => visitedNodesDict.Keys;
}

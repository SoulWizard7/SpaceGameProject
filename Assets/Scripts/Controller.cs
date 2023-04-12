using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Material defaultMaterial;
    public Material selectedMaterial;
    public Material pathMaterial;
    public UnitBase selectedUnit;
    public HexData selectedHex;
    public HexGridLayoutV2 grid;
    public List<HexData> neighbors;
    public List<HexData> path;
    
    BFSResult curResult;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            selectedUnit.MoveTo(path);
        }
        
        
    }

    public void ShowPossibleMovementHex(HexData hex)
    {
        if (selectedHex)
        {
            selectedHex.SetMaterial(defaultMaterial);

            if (neighbors != null)
            {
                foreach (var h in neighbors)
                {
                    h.SetMaterial(defaultMaterial);
                }
            }
        }

        curResult = GraphSearch.BFSGetRange(grid.grid, hex, selectedUnit.movementPoints);
        neighbors = new List<HexData>(curResult.GetRangePositions());

        foreach (var n in neighbors)
        {
            n.SetMaterial(selectedMaterial);
        }
    }

    public void ShowPath()
    {
        ShowPossibleMovementHex(selectedUnit.curHexData);
        
        path = GraphSearch.GeneratePathBFS(selectedHex, curResult.visitedNodesDict);
        foreach (var n in path)
        {
            n.SetMaterial(pathMaterial);
        }
    }
}

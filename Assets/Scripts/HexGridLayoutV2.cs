using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;

[RequireComponent(typeof(Controller))]
public class HexGridLayoutV2 : MonoBehaviour
{
    [Header("Grid Settings")] 
    public Vector2Int gridSize;
    public GameObject hex;
    public GameObject unitBase;
    
    [Header("Tile Settings")]
    public Material material;
    public float innerSize = 0f;
    public float outerSize = 1f;
    public float tileOffset = 0.1f;
    public bool isFlatTopped;

    private Controller _controller;

    public Dictionary<Vector2Int, HexData> grid = new Dictionary<Vector2Int, HexData>();

    private void Awake()
    {
        _controller = GetComponent<Controller>();
        _controller.grid = this;
        LayoutGrid();
    }

    private void LayoutGrid()
    {
        Vector2Int pos = new Vector2Int( );

        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                GameObject tile = Instantiate(hex, transform);
                tile.name = $"Hex {x},{y}";
                tile.transform.position = GetPositionForHexGromCordinate(new Vector2Int(x, y));
                
                tile.transform.Rotate(Vector3.right, 90);

                if (isFlatTopped)
                {
                    tile.transform.Rotate(Vector3.forward, 90f);
                }

                pos.x = x;
                pos.y = y;

                int tileCost =  Random.Range(1,4);
                SHexData sHex = new SHexData(pos, tileCost);

                //tile.GetComponentInChildren<TMP_Text>().text = tileCost.ToString();

                HexData hexData = tile.GetComponent<HexData>();
                hexData.sHexData = sHex;
                hexData._controller = _controller;
                
                grid.Add(pos, hexData);
            }
        }

        foreach (var key in grid)
        {
            pos = key.Key;
            HexData hexData;
            key.Value.SetMaterial(_controller.defaultMaterial);
            

            int offset = ((pos.y % 2) == 0) ? 0 : -1;

            if (pos.y >= 1)
            {
                //top left
                if (pos.x + offset >= 0 && pos.x + offset < gridSize.x)
                {
                    hexData = grid[pos + new Vector2Int(0 + offset, -1)];
                    key.Value.sHexData.neighbors.Add(hexData);
                }
                //top right
                if (pos.x + 1 + offset < gridSize.x)
                {
                    hexData = grid[pos + new Vector2Int(1 + offset, -1)];
                    key.Value.sHexData.neighbors.Add(hexData);
                }
            }
            
            // left
            if (pos.x - 1 >= 0)
            {
                hexData = grid[pos + new Vector2Int(-1, 0)];
                key.Value.sHexData.neighbors.Add(hexData);
            }
            // right
            if (pos.x + 1 < gridSize.x)
            {
                hexData = grid[pos + new Vector2Int(1, 0)];
                key.Value.sHexData.neighbors.Add(hexData);
            }

            if (pos.y + 1 < gridSize.y)
            {
                //bottom left
                if (pos.x + offset >= 0 && pos.x + offset < gridSize.x)
                {
                    hexData = grid[pos + new Vector2Int(0 + offset, 1)];
                    key.Value.sHexData.neighbors.Add(hexData);
                }
                //bottom right
                if (pos.x + 1 + offset < gridSize.x)
                {
                    hexData = grid[pos + new Vector2Int(1 + offset, 1)];
                    key.Value.sHexData.neighbors.Add(hexData);
                }
            }
        }

        Vector2Int unitPos = new Vector2Int(Random.Range(0, gridSize.x), Random.Range(0, gridSize.y));
        Vector3 a = grid[unitPos].transform.position;
        a += Vector3.up;

        UnitBase unit = Instantiate(unitBase, a, Quaternion.identity).GetComponent<UnitBase>();
        unit.curHexData = grid[unitPos];
        unit.position = unit.curHexData.sHexData.tilePos;
        unit.controller = _controller;


    }

    private Vector3 GetPositionForHexGromCordinate(Vector2Int xy)
    {
        int column = xy.x;
        int row = xy.y;
        float width;
        float height;
        float xPosition;
        float yPosition;
        bool shouldOffset;
        float horizontalDistance;
        float verticalDistance;
        float offset;
        float size = outerSize;

        if (!isFlatTopped)
        {
            shouldOffset = (row % 2) == 0;
            width = Mathf.Sqrt(3) * size;
            height = 2f * size;

            horizontalDistance = width;
            verticalDistance = height * (3f / 4f);

            offset = (shouldOffset) ? width / 2 : 0;

            xPosition = (column * (horizontalDistance)) + offset;
            yPosition = row * verticalDistance;
        }
        else
        {
            shouldOffset = (column % 2) == 0;
            width = 2f * size;
            height = Mathf.Sqrt(3f) * size;
            
            horizontalDistance = width * (3f / 4f);
            verticalDistance = height;

            offset = (shouldOffset) ? height / 2 : 0;

            xPosition = column * horizontalDistance;
            yPosition = (row * verticalDistance) + offset;
        }

        //return new Vector3(xPosition + (tileOffset * xy.x), 0, -yPosition - (tileOffset * xy.y));
        return new Vector3(xPosition, 0, -yPosition);
    }
}

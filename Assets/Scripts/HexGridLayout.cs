using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGridLayout : MonoBehaviour
{
    [Header("Grid Settings")] 
    public Vector2Int gridSize;
    
    [Header("Tile Settings")]
    public Material material;
    public float innerSize = 0f;
    public float outerSize = 1f;
    public float height = 0f;
    public float tileOffset = 0.1f;
    public bool isFlatTopped;
    

    private void OnEnable()
    {
        LayoutGrid();
    }
    /*
    public void OnValidate()
    {
        if (Application.isPlaying)
        {
            LayoutGrid();
        }
    }*/

    private void LayoutGrid()
    {
        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                GameObject tile = new GameObject($"Hex {x},{y}", typeof(HexRenderer));
                tile.transform.position = GetPositionForHexGromCordinate(new Vector2Int(x, y));

                HexRenderer hexRenderer = tile.GetComponent<HexRenderer>();
                hexRenderer.isFlatTopped = isFlatTopped;
                hexRenderer.outerSize = outerSize;
                hexRenderer.innerSize = innerSize;
                hexRenderer.SetMaterial(material);
                hexRenderer.DrawMesh();

                tile.transform.SetParent(transform, true);

            }
        }
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

        return new Vector3(xPosition + (tileOffset * xy.x), 0, -yPosition - (tileOffset * xy.y));
    }
    
}

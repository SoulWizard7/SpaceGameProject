using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SHexData
{
    public Vector2Int tilePos;
    public int tileCost;
    public List<HexData> neighbors;
    public bool walkable;

    public SHexData(Vector2Int TilePos, int TileCost)
    {
        tilePos = TilePos;
        tileCost = TileCost;
        neighbors = new List<HexData>();
        walkable = true;
    } 
}

public class HexData : MonoBehaviour
{
    public SHexData sHexData;

    private SpriteRenderer _spriteRenderer;
    public Controller _controller;

    public int GetCost() => sHexData.tileCost;

    public void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetMaterial(Material newMaterial)
    {
        _spriteRenderer.material = newMaterial;
    }

    private void OnMouseUp()
    {
        _controller.selectedHex = this;
        _controller.ShowPath();
    }
}

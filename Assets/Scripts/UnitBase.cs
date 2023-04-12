using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    public Vector2Int position;
    public HexData curHexData;
    public Controller controller;
    public int movementPoints;

    private List<Vector3> movePath = new List<Vector3>();
    private bool move = false;
    private float lerpValue = 0;
    public float lerpSpeed = 3;
    private Vector3 curPos;
    private Vector3 newPos;
    private int pathPosIndex;
    

    public void MoveTo(List<HexData> path)
    {
        
        movePath.Clear();
        
        for (int i = 0; i < path.Count; i++)
        {
            movePath.Add(new Vector3(path[i].transform.position.x, 1, path[i].transform.position.z));
        }

        pathPosIndex = 1;

        curHexData = path[path.Count - 1];
        curPos = movePath[0];
        newPos = movePath[1];
        move = true;
    }


    private void Update()
    {
        if (move)
        {
            lerpValue += Time.deltaTime * lerpSpeed;
            transform.position = Vector3.Lerp(curPos, newPos, lerpValue);
            if (lerpValue > 1f)
            {
                pathPosIndex++;
                if (pathPosIndex < movePath.Count)
                {
                    curPos = newPos;
                    newPos = movePath[pathPosIndex];
                    lerpValue = 0f;
                }
                else
                {
                    move = false;
                    controller.ShowPossibleMovementHex(curHexData);
                }
            }
        }
    }

    private void OnMouseUp()
    {
        controller.selectedUnit = this;
        controller.ShowPossibleMovementHex(curHexData);
    }
}

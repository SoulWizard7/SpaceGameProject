using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float angleDegrees;
    public float dist;
    public GameObject playerGameObject;
    public float curDist;
    public float maxDist;
    public float lerpSpeed;

    private Vector3 offsetDir;
    
    public static Vector2 AngToDir(float angleRad) => new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    public static float TAU = 6.28f;

    public static float DegreesToRadians(float angleInDegrees)
    {
        return angleInDegrees * (TAU / 360);
    }

    private void Start()
    {
        Vector2 angToDir = AngToDir(DegreesToRadians(angleDegrees));
        transform.position = new Vector3(0, angToDir.y, angToDir.x) * dist + playerGameObject.transform.position;
    }

    void Update()
    {
        Vector3 playerPos = playerGameObject.transform.position;
        Vector3 currentPos = transform.position;
        Vector2 angToDir = AngToDir(DegreesToRadians(angleDegrees));
        Vector3 offset = new Vector3(0, angToDir.y, angToDir.x) * dist;
        Vector3 desiredPos = playerPos + offset;
        
        //transform.position = _player.transform.position + new Vector3(0, angToDir.y, angToDir.x) * dist;
        transform.rotation = Quaternion.AngleAxis(180 - angleDegrees, Vector3.right);
        curDist = Vector3.Distance(desiredPos, currentPos);

        if (curDist > maxDist)
        {
            transform.position = Vector3.Lerp(currentPos, desiredPos, Time.deltaTime * lerpSpeed);
        }
    }
}

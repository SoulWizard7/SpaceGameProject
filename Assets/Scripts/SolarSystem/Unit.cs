using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private SolarSystem _solarSystem;
    private bool isMoving;
    
    
    public float speed;
    private float curSpeed;
    
    
    private void Awake()
    {
        _solarSystem = GameObject.Find("SolarSystem").GetComponent<SolarSystem>();
    }

    private void Update()
    {
        if (isMoving)
        {
            switch (_solarSystem.simSpeed)
            {
                case SimSpeed.stop :
                    curSpeed = 0;
                    break;
                case SimSpeed.normal :
                    curSpeed = 1f;
                    break;
                case SimSpeed.fast :
                    curSpeed = 100f;
                    break;
            }

            transform.position += moveDir * curSpeed/10000;

            float dist = MyDistance(transform.position, moveToPos);

            if (dist < 0.5f)
            {
                isMoving = false;
                _solarSystem.SetState(SimSpeed.normal);
            }
        }

    }

    private Vector3 moveToPos;
    private Vector3 moveDir;

    public void MoveToPos(Vector3 worldPos)
    {
        moveToPos = worldPos;
        moveToPos.y = 0;
        
        moveDir = moveToPos - transform.position;
        moveDir.Normalize();
        isMoving = true;
        _solarSystem.SetState(SimSpeed.fast);
    }
    
    public static float MagnitudeVector3(Vector3 a)
    {
        float squaredLength = (a.x * a.x) + (a.y * a.y) + (a.z * a.z);
        return Mathf.Sqrt(squaredLength);
    }

    public static float MyDistance(Vector3 a, Vector3 b)
    {
        return MagnitudeVector3(a - b);
    }
}

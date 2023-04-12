using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlanetBase : MonoBehaviour
{
    public float distanceFromSun;
    public float orbitSpeed;
    private Vector3 startPos;
    
    public static Vector2 AngRadToDir(float angleRad) => new (Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    public static Vector2 AngDegToDir(float angleDeg) =>  new (Mathf.Cos(angleDeg * Mathf.Deg2Rad), Mathf.Sin(angleDeg * Mathf.Deg2Rad));

    public SolarSystem solarSystem;
    private Vector3 solarSystemCenter;
    
    private float time;

    

    public void SetupPlanet(float distance, Vector3 solarSystemPos)
    {
        Vector3 pos = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        transform.position += pos * distance;
        solarSystemCenter = solarSystemPos;
    }

    private void Update()
    {
        switch (solarSystem.simSpeed)
        {
            case SimSpeed.stop : 
                break;
            case SimSpeed.normal : transform.RotateAround(solarSystemCenter, Vector3.up, orbitSpeed / distanceFromSun * Time.deltaTime);
                break;
            case SimSpeed.fast : transform.RotateAround(solarSystemCenter, Vector3.up, orbitSpeed / distanceFromSun * Time.deltaTime * solarSystem.fastTimeMultiplier);
                break;
        }
    }
}

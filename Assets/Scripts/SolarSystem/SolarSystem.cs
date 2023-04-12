using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum SimSpeed
{
    stop,
    normal,
    fast,
}

public class SolarSystem : MonoBehaviour
{
    public SimSpeed simSpeed = SimSpeed.normal; 
    
    public int planets;
    public GameObject planetPrefab;
    public GameObject sunPrefab;
    public Material planetLineMaterial;
    public List<Sprite> planetTypes;

    public List<PlanetBase> planetList;
    public bool bIsSimulating;

    public float time;
    public float fastTimeMultiplier = 1000;

    public float lineWidth = 10f;

    private void Start()
    {
        CreateSolarSystem();
    }

    private void Update()
    {
        switch (simSpeed)
        {
            case SimSpeed.stop : StopSim();
                break;
            case SimSpeed.normal : SimNormal();
                break;
            case SimSpeed.fast : SimFast();
                break;
        }
    }

    void SimNormal()
    {
        time += Time.deltaTime;
    }
    
    void SimFast()
    {
        time += Time.deltaTime * fastTimeMultiplier;
    }

    void StopSim()
    {
        
    }

    public void SetState(SimSpeed speedMode)
    {
        simSpeed = speedMode;
    }


    void CreateSolarSystem()
    {
        Vector3 solarSystemPos = transform.position;
        Instantiate(sunPrefab, solarSystemPos, Quaternion.identity);
        
        float totalDistance = 5;
        
        for (int i = 0; i < planets; i++)
        {
            GameObject planet = Instantiate(planetPrefab, transform);
            PlanetBase planetBase = planet.GetComponent<PlanetBase>();

            totalDistance += Random.Range(5f, 20f);
            planetBase.distanceFromSun = totalDistance;
            
            planetBase.SetupPlanet(totalDistance, solarSystemPos);
            planetBase.orbitSpeed = Random.Range(.070f, .140f);
            planetBase.solarSystem = this;
            planetList.Add(planetBase);
            
            CreateCircle(planet, totalDistance, lineWidth);
        }
    }
    
    public void CreateCircle(GameObject planet, float radius, float lineWidth)
    {
        var child = new GameObject();
        
        var segments = 360;
        var line = child.AddComponent<LineRenderer>();
        var lineController = child.AddComponent<LineRendererWidthController>();
        lineController.Settings(lineWidth, Camera.main.gameObject.transform.parent.gameObject, line);
        
        //planet.AddComponent<PlanetLookAtCamera>();
        var spriterender = planet.AddComponent<SpriteRenderer>();
        spriterender.sortingOrder = 1;
        int random = Random.Range(0, planetTypes.Count);
        spriterender.sprite = planetTypes[random];

        line.material = planetLineMaterial;
        line.useWorldSpace = true;
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.positionCount = segments + 1;

        var pointCount = segments + 1; // add extra point to make startpoint and endpoint the same to close the circle
        var points = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            var rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector3(Mathf.Sin(rad) * radius, 0, Mathf.Cos(rad) * radius) - transform.position;
        }
        
        planet.transform.Rotate(Vector3.right,90);
        planet.transform.position += Vector3.up * 0.1f;

        line.SetPositions(points);
    }
}

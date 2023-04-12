using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetLookAtCamera : MonoBehaviour
{
    private GameObject cameraPos;
    void Start()
    {
        cameraPos = Camera.main.gameObject.transform.parent.gameObject;
    }

    private void Update()
    {
        transform.LookAt(cameraPos.transform);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIButtons : MonoBehaviour
{
    private SolarSystem _solarSystem;
    public TMP_Text text;

    private void Update()
    {
        text.text = _solarSystem.time.ToString();
    }

    private void Awake()
    {
        _solarSystem = GameObject.Find("SolarSystem").GetComponent<SolarSystem>();
    }

    public void StopSim()
    {
        _solarSystem.simSpeed = SimSpeed.stop;
    }

    public void NormalSim()
    {
        _solarSystem.simSpeed = SimSpeed.normal;
    }

    public void FastSim()
    {
        _solarSystem.simSpeed = SimSpeed.fast;
    }
}

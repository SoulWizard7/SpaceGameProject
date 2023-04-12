using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShipController : MonoBehaviour
{
    private Vector3 screenPos;
    private Vector3 worldPos;
    private Vector3 currentPosition;
    public GameObject movementPointer;

    private Plane plane = new Plane(Vector3.up, 0);

    public Unit spaceShip;

    private void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            screenPos = Input.mousePosition;

            Ray ray = Camera.main.ScreenPointToRay(screenPos);

            if (plane.Raycast(ray, out float distance))
            {
                worldPos = ray.GetPoint(distance);
            }

            if (Input.GetMouseButtonUp(0))
            {
                movementPointer.transform.position = worldPos;
                currentPosition = worldPos;
            }

            if (Input.GetMouseButtonUp(1))
            {
                spaceShip.MoveToPos(currentPosition);
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float camSpeed = 1;
    public float rotSpeed = 1;
    public float zoom;
    private Vector3 pos;
    private float curCamSpeed;

    private void Awake()
    {
        pos = transform.position;
        zoom = transform.position.y;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            curCamSpeed = camSpeed * 2;
        }
        else
        {
            curCamSpeed = camSpeed;
        }
        
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        zoom -= Input.mouseScrollDelta.y;
        pos.y = zoom;

        pos += transform.right * (hor * curCamSpeed * Time.deltaTime);
        pos += transform.forward * (ver * curCamSpeed * Time.deltaTime);

        transform.position = pos;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void LeftMouseButton();
public delegate void DodgeButton();

public class InputHandler : MonoBehaviour
{
    public static Vector2 WASD { get; private set; }
    public static Vector3 MousePosition{ get; private set; }

    public static bool mouseRightHold;
    public static bool mouseLeftHold;

    public static LeftMouseButton leftMouseButtonDown;
    public static DodgeButton dodgeButton;

    void Update()
    {
        float hor = Input.GetAxis("HorizontalWASD");
        float ver = Input.GetAxis("VerticalWASD");
        
        WASD = new Vector2(hor, ver);

        MousePosition = Input.mousePosition;
        mouseLeftHold = Input.GetButton("Fire1");
        mouseRightHold = Input.GetButton("Fire2");

        if (Input.GetButtonDown("Fire1")) leftMouseButtonDown.Invoke();
        if (Input.GetButtonDown("Dodge")) dodgeButton.Invoke();
    }

    public Vector2 GetWASD()
    {
        return WASD;
    }
}

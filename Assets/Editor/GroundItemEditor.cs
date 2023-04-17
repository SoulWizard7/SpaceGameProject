using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(GroundItem))]
public class GroundItemEditor : Editor
{
    [TextArea(15, 20)] 
    public string description;

    private SerializedObject so;
    private SerializedProperty propItemObject;
    
    private void OnEnable()
    {
        so = serializedObject;
        propItemObject = so.FindProperty("itemObject");
    }

    public override void OnInspectorGUI()
    {
        so.Update();

        EditorGUILayout.PropertyField(propItemObject);
        
        GUILayout.Space(10);
        if (so.ApplyModifiedProperties())
        {
            GroundItem groundItem = target as GroundItem;
            groundItem.RefreshItem();
        }
        
        base.OnInspectorGUI();
        

        // explicit positioning using Rect
        // GUI
        // GUILayout

        // implicit positioning, auto-layout
        // EditorGUI
        // EditorGUILayout
    }
}

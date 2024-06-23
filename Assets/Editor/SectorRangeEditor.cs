using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(SectorRange))]
public class SectorRangeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SectorRange sectorRange = (SectorRange)target;

        EditorGUILayout.Space();

        if (GUILayout.Button("Create Sector"))
        {
            GameObject sectorObj = sectorRange.CreateSector();
        }
    }
}

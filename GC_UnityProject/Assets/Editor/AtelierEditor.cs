using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Atelier))]
public class AtelierEditor : Editor
{

    public override void OnInspectorGUI()
    {   
        Atelier atelier = (Atelier)target;
        if (GUILayout.Button("Fill ingredients"))
        {
            atelier.ingredients = atelier.GetComponentsInChildren<Obstacle>();
        }

        DrawDefaultInspector();
    }

}

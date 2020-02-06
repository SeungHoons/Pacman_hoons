using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapToolMain : EditorWindow
{
    public string fileName = "Input Name";

    private MapToolMain _myWindow;

    [MenuItem("Window/MapTool")]
    public static void ShowWindow()
    {
        GetWindow<MapToolMain>("MapTool");
    }

    private void OnGUI()
    {
        GUILayout.Label("File Info",EditorStyles.boldLabel);
        fileName = EditorGUILayout.TextField("Name", fileName);

        using (new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Save"))
            {
                Debug.Log("Save");
            }

            if (GUILayout.Button("New"))
            {
                Debug.Log("new");
            }
        }

    }
}

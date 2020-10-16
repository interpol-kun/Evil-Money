using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerInformation), true)]
public class AudioEventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);
        if (GUILayout.Button("Clear stats"))
        {
            ((PlayerInformation)target).Clear();
        }
        EditorGUI.EndDisabledGroup();
    }
}


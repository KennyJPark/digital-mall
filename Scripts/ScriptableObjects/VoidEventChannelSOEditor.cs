using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(VoidEventChannelSO))]
public class VoidEventChannelSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        VoidEventChannelSO eventChannel = (VoidEventChannelSO)
        target;
        if (GUILayout.Button("Raise Event"))
        {
            eventChannel.RaiseEvent();
        }
    }
}
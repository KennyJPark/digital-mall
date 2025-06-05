using UnityEditor;

/*
[CustomEditor](typeof(Patron))]
public class PatronConfigEditor : Editor
{
    private Editor editorInstance;

    private void OnEnable()
    {
        editorInstance = null;
    }
    public override void OnInspectorGUI()
    {
        if (editorInstance == null)
            //editorInstance = Editor.CreateEditor(patronConfig);
    }
}
*/
/*
using UnityEditor;
[CustomEditor(typeof(NPCHealth))]
public class NPCHealthEditor : Editor
{
    private Editor editorInstance;
    private void OnEnable()
    {
        // reset the editor instance
        editorInstance = null;
    }
    public override void OnInspectorGUI()
    {
        // the inspected target component
        NPCHealth npcHealth = (NPCHealth)target;
        if (editorInstance == null)
        editorInstance = Editor.
        CreateEditor(npcHealth.config);
        // show the variables from the MonoBehaviour
        base.OnInspectorGUI();
        // draw the ScriptableObjects inspector
        editorInstance.DrawDefaultInspector();
    }
}
*/
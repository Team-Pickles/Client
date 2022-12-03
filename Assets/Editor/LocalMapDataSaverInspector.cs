using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LocalMapDataSaver))]
public class LocalMapDataSaverInspector : Editor
{
    public LocalMapDataSaver current {
        get {
            return (LocalMapDataSaver)target;
        }
    }

    public override void OnInspectorGUI()
    {
       DrawDefaultInspector();
       if(GUILayout.Button("Save(DB)"))
           current.Save(false);
       if(GUILayout.Button("Save(FILE)"))
           current.Save(true);
    }
}

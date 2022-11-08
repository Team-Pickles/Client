using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapDataSaverWOTilemap))]
public class MapDataSaverInspector : Editor
{
    public MapDataSaverWOTilemap current {
        get {
            return (MapDataSaverWOTilemap)target;
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if(GUILayout.Button("Save"))
            current.Save();
    }
}

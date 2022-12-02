using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapDataSave))]
public class MapDataSaverInspector : Editor
{
    public MapDataSave current {
        get {
            return (MapDataSave)target;
        }
    }

    //public override void OnInspectorGUI()
    //{
    //    DrawDefaultInspector();
    //    if(GUILayout.Button("Save(DB)"))
    //        current.Save(false);
    //    if(GUILayout.Button("Save(FILE)"))
    //        current.Save(true);
    //}
}

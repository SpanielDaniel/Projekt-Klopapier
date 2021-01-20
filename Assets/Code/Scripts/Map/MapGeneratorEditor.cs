
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Code.Scripts.Map
{
    
    [CustomEditor(typeof(MapGenerator))]
    [CanEditMultipleObjects]
    public class MapGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            MapGenerator script = (MapGenerator) target;

            if(GUILayout.Button("Create World"))
            {
                script.Init();
                
            }
            if(GUILayout.Button("Delete World"))
            {
                script.DeleteMap();
            }
        }
    }
}

#endif
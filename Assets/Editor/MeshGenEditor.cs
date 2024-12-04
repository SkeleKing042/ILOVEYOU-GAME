using UnityEditor;
using UnityEngine;

namespace ILOVEYOU.Environment
{
    [CustomEditor(typeof(EditorTimeMeshGenerator)),CanEditMultipleObjects]
    public class MeshGenEditor : Editor
    {
        EditorTimeMeshGenerator m_target;

        private void OnEnable()
        {
            m_target = (EditorTimeMeshGenerator)target;
        }
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("This script will allow you to convert concave meshes into convex ones.\nThis will create a new asset in a folder next to this scene, and will overwrite pre-existing ones, so make sure to use different names for different objects.", MessageType.None);
            if (GUILayout.Button("Generate Meshes"))
            {
                m_target.GenerateMeshes();
            }
            base.OnInspectorGUI();
        }
    }
}
using UnityEditor;
using UnityEngine;

namespace ILOVEYOU.Environment
{
    [CustomEditor(typeof(EditorTimeMeshGenerator))]
    public class MeshGenEditor : Editor
    {
        EditorTimeMeshGenerator m_target;

        private void OnEnable()
        {
            m_target = (EditorTimeMeshGenerator)target;
        }
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Generate Meshes"))
            {
                m_target.GenerateMeshes();
            }
            base.OnInspectorGUI();
        }
    }
}
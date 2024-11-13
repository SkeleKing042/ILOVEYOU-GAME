using ILOVEYOU.Management;
using UnityEditor;
using UnityEngine;

namespace ILOVEYOU
{
    namespace EditorScript
    {

        [CustomEditor(typeof(GameManager))]
        public class GameManagerEditor : Editor
        {
            GameManager m_target;
            
            private void OnEnable()
            {
                m_target = (GameManager)target;
            }
            public override void OnInspectorGUI()
            {
                if (m_target.IsDev)
                {
                    if (GUILayout.Button("Start Game"))
                    {
                        Debug.Log("Manual game start called");
                        m_target.BeginSetup();
                    }
                }
                else
                {
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.TextField("Enable DevMode to start game manually.");
                    EditorGUI.EndDisabledGroup();
                }
                base.OnInspectorGUI();
            }
        }
    }
}
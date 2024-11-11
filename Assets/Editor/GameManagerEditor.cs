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
            SerializedProperty m_devMode;
            
            private void OnEnable()
            {
                m_target = (GameManager)target;
                m_devMode = serializedObject.FindProperty("m_devMode");
            }
            public override void OnInspectorGUI()
            {
                serializedObject.Update();
                if (!m_target.enabled)
                {
                    string s = "You shouldn't be reading this...";
                    if (ControllerManager.Instance != null && m_target.IsDev && ControllerManager.Instance.ControllerCount > 0)
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
                        if (!m_target.IsDev)
                            s = "Please enable DevMode to start the game manually.";
                        else if (!ControllerManager.Instance)
                            s = "Please start the game.";
                        else if (ControllerManager.Instance.ControllerCount == 0)
                            s = "Please connect a controller.";
                        EditorGUILayout.TextField(s);
                        EditorGUI.EndDisabledGroup();
                    }
                    m_devMode.boolValue = GUILayout.Toggle(m_devMode.boolValue, new GUIContent("Dev Mode"));
                }
                else
                {
                    m_devMode.boolValue = false;
                }
                serializedObject.ApplyModifiedProperties();
                base.OnInspectorGUI();
            }
        }
    }
}
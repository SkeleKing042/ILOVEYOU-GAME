using ILOVEYOU.Management;
using UnityEditor;
using UnityEngine;

namespace ILOVEYOU.EditorScript
{
    [CustomEditor(typeof(GameSettings))]
    [CanEditMultipleObjects]
    public class GameSettingsEditor : Editor
    {
        GameSettings m_target;
        SerializedProperty diffCapProp;
        private void OnEnable()
        {
            m_target = (GameSettings)target;
            diffCapProp = serializedObject.FindProperty("m_difficultyCap");
        }
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("These are the settings that will be used in-game. To assign settings, find the \"settings\" variable in the GameManager, or click assign to change settings in realtime\n(Assigned settings will reset on scene start).", MessageType.Info);
            if (EditorGUILayout.LinkButton("Assign"))
            {
                m_target.Assign();
            }
            if(EditorGUILayout.LinkButton("Initialize color preferences"))
            {
                m_target.InitalizePrefs();
            }

            serializedObject.Update();

            EditorGUILayout.Slider(diffCapProp, 0, 60, new GUIContent("Difficulty Cap"));

            serializedObject.ApplyModifiedProperties();
            //base.OnInspectorGUI();
        }
    }
}
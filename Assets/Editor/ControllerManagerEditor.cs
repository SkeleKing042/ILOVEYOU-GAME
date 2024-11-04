using ILOVEYOU.Management;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ILOVEYOU
{
    namespace EditorScript
    {
        [CustomEditor(typeof(ControllerManager))]
        public class ControllerManagerEditor : Editor
        {
            ControllerManager m_target;
            SerializedProperty m_playerRef;
            SerializedProperty m_controllerRefs;

            private void OnEnable()
            {
                m_target = (ControllerManager)target;
                m_playerRef = serializedObject.FindProperty("m_playerPrefab");
                m_controllerRefs = serializedObject.FindProperty("m_controllers");
            }
            public override void OnInspectorGUI()
            {
                serializedObject.Update();

                if (EditorGUILayout.LinkButton("Join Players - make sure controllers are connected!"))
                {
                    Debug.Log("Attempting to Join Players");
                    m_target.JoinPlayers();
                }
                EditorGUILayout.ObjectField(m_playerRef, new GUIContent("Player Prefab"));

                GUI.enabled = false;
                EditorGUILayout.PropertyField(m_controllerRefs);
                GUI.enabled = true;


                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
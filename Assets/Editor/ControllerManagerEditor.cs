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
            SerializedProperty m_playerMaterials;

            private void OnEnable()
            {
                m_target = (ControllerManager)target;
                m_playerRef = serializedObject.FindProperty("m_playerPrefab");
                m_controllerRefs = serializedObject.FindProperty("m_controllers");
                m_playerMaterials = serializedObject.FindProperty("m_playerMaterials");
            }
            public override void OnInspectorGUI()
            {
                serializedObject.Update();

                if (EditorGUILayout.LinkButton("Join Players - make sure controllers are connected!"))
                {
                    Debug.Log("Attempting to Join Players");
                    m_target.JoinPlayers(m_target.GetControllers.Count);
                }
                EditorGUILayout.ObjectField(m_playerRef, new GUIContent("Player Prefab"));

                EditorGUILayout.PropertyField(m_playerMaterials);

                GUI.enabled = false;
                EditorGUILayout.PropertyField(m_controllerRefs);
                GUI.enabled = true;


                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
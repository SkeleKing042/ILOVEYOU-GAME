using System;
using UnityEditor;
using UnityEngine;

using EGL = UnityEditor.EditorGUILayout;

namespace ILOVEYOU.Audio
{
    [CustomEditor(typeof(SoundManagerInitializer))]
    public class SoundManagerInitializerEditor : Editor
    {
        private SoundManagerInitializer m_init;
        SerializedProperty m_managers;
        public override void OnInspectorGUI()
        {
            m_init = target as SoundManagerInitializer;


            using (new EditorGUI.DisabledScope(true))
            {
                EGL.ObjectField("Script", MonoScript.FromMonoBehaviour((MonoBehaviour)target), GetType(), false);
                EGL.ObjectField("Editor", MonoScript.FromScriptableObject(this), GetType(), false);
            }

            // get properties
            m_managers = serializedObject.FindProperty("m_data");

            EditorGUI.BeginChangeCheck();

            OnGUI();

            if (EditorGUI.EndChangeCheck())
            {
                //Undo.RecordObject(target, "Changed " + Enum.GetNames(typeof(SoundTag))[i]);
                EditorUtility.SetDirty(target);
                serializedObject.ApplyModifiedProperties();
            }

        }


        protected virtual void OnGUI()
        {
            for (int i = 0; i < m_managers.arraySize; i++)
            {
                serializedObject.ApplyModifiedProperties();

                EGL.PropertyField(m_managers.GetArrayElementAtIndex(i), new GUIContent(Enum.GetNames(typeof(SoundTag))[i]));

            }
        }
    }
}

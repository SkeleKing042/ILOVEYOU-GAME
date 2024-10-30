using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
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
            using (new EditorGUI.DisabledScope(true))
            {
                EGL.ObjectField("Script", MonoScript.FromMonoBehaviour((MonoBehaviour)target), GetType(), false);
                EGL.ObjectField("Editor", MonoScript.FromScriptableObject(this), GetType(), false);
            }

            // get properties
            m_managers = serializedObject.FindProperty("Managers");


            if (m_managers.arraySize == 0)
                ResetValues();

            for (int i = 0; i < m_managers.arraySize; i++)
            {
                EGL.LabelField("Manager " + (i + 1).ToString());
                EGL.PropertyField(m_managers.GetArrayElementAtIndex(i), GUIContent.none);
            }
            
            m_init = target as SoundManagerInitializer;

            //EGL.PropertyField(m_managers);

            //base.OnInspectorGUI();
        }

        public void ResetValues()
        {
            m_init.Managers = new SoundManager[5];

            m_init.Managers[0] = new SoundManager("None");
            m_init.Managers[1] = new SoundManager("SFX");
            m_init.Managers[2] = new SoundManager("Music");
            m_init.Managers[3] = new SoundManager("UI");
            m_init.Managers[4] = new SoundManager("Environment");

        }
    }
}

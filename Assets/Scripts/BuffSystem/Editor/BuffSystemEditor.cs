using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BuffSystem))]
public class BuffSystemEditor : Editor
{
    private readonly string[] m_effectNames = new string[] { "Stat Increase", "Unity Event", "Blah blah blah" };
    private BuffSystem m_system;


    public override void OnInspectorGUI()
    {


        for (int i = 0; i < m_system.GetData.Length; i++)
        {
            EditorGUI.BeginChangeCheck();

            BuffSystem.BuffData current = m_system.GetData[i];

            int buffType = current.GetBuffType;

            buffType = EditorGUILayout.Popup("Buff Type:", buffType, m_effectNames);

            if (EditorGUI.EndChangeCheck())
            {
                current.SetBuffType(buffType);
            }
        }


        

    }

    private void OnEnable()
    {
        m_system = target as BuffSystem;
    }
}

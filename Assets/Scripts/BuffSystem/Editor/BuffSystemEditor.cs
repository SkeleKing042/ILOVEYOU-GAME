using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace ILOVEYOU
{
    namespace BuffSystem
    {
        [CustomEditor(typeof(BuffDataSystem))]
        public class BuffSystemEditor : Editor
        {
            private bool DebugMode = true;

            private readonly string[] m_effectNames = new string[] { "Stat Change", "Unity Event" };
            private readonly string[] m_stackNames = new string[] { "Don't Stack", "Extend Current", "Override", "Stack" };
            private bool[] m_foldOuts;
            private bool[] m_foldOutDebugs;
            private BuffDataSystem m_system;


            public override void OnInspectorGUI()
            {
                //gets the main script (the reason this isn't called on OnEnable() is cause there is an error or two caused by it for some reason IDK why but yeah
                m_system = target as BuffDataSystem; //<-- this aint getting called in the game build so *shrugs*
                Array.Resize(ref m_foldOuts, m_system.GetData.Length);
                //enable or disable debug mode, by default its on
                DebugMode = EditorGUILayout.Toggle("Debug Mode: ", DebugMode);

                //debug shtuff;
                if (DebugMode && Application.isPlaying) _Debug();
                if (!Application.isPlaying) EditorGUILayout.LabelField("| Debug Stuff Will show here in play mode |");

                //display and edit data
                EditorGUILayout.Space(10);
                EditorGUILayout.LabelField("----Buff Data----");
                for (int i = 0; i < m_system.GetData.Length; i++)
                {
                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.BeginHorizontal();
                    //replaces effect title with a default name if no name is present
                    string title = (m_system.GetData[i].GetName == "") ? "Status Effect " + i + ":" : m_system.GetData[i].GetName + ":";
                    //sets foldout bool
                    m_foldOuts[i] = EditorGUILayout.Foldout(m_foldOuts[i], title);
                    //destroys the selected effect
                    if (GUILayout.Button("Remove"))
                    {
                        _Delete(i);

                        break;
                    }

                    EditorGUILayout.EndHorizontal();
                    //if the foldout is currently expanded
                    if (m_foldOuts[i])
                    {
                        EditorGUILayout.Space();

                        EditorGUI.BeginChangeCheck();

                        BuffDataSystem.BuffData current = m_system.GetData[i];

                        string name = current.GetName;
                        int buffID = current.GetBuffID;
                        int isStackable = current.GetIsStackable;
                        int buffType = current.GetBuffType;
                        bool isPermanent = current.GetPermanent;
                        float time = current.GetTime;
                        SerializedProperty particleEffect = serializedObject.FindProperty("m_buffData").GetArrayElementAtIndex(i).FindPropertyRelative("m_particleEffect"); ;

                        float maxHealthValue = current.GetMaxHealth;
                        float moveSpeedValue = current.GetMoveSpeed;
                        float shootSpeedValue = current.GetShootSpeed;
                        float damageValue = current.GetDamage;

                        name = EditorGUILayout.TextField("Name: ", name);
                        buffID = EditorGUILayout.IntField("ID: ", buffID);
                        EditorGUILayout.Space();
                        isStackable = EditorGUILayout.Popup("Does stack: ", isStackable, m_stackNames);
                        isPermanent = EditorGUILayout.Toggle("Is Permanent:", isPermanent);
                        if (!isPermanent) time = EditorGUILayout.FloatField("Time:", time);
                        EditorGUILayout.Space();
                        EditorGUILayout.PropertyField(particleEffect, new GUIContent("Particle Effect: "));
                        EditorGUILayout.Space();
                        buffType = EditorGUILayout.Popup("Buff Type:", buffType, m_effectNames);

                        EditorGUILayout.Space();

                        switch (buffType)
                        {
                            case 0://Stat Change
                                EditorGUILayout.LabelField("Player Stats");
                                maxHealthValue = EditorGUILayout.FloatField("Max Health Add: ", maxHealthValue);
                                moveSpeedValue = EditorGUILayout.FloatField("Move speed Add: ", moveSpeedValue);
                                shootSpeedValue = EditorGUILayout.FloatField("Shoot Speed Multiplier Add: ", shootSpeedValue);
                                damageValue = EditorGUILayout.FloatField("Damage Multiplier Add: ", damageValue);
                                break;

                            case 1://unity event

                                SerializedProperty onActivate = serializedObject.FindProperty("m_buffData").GetArrayElementAtIndex(i).FindPropertyRelative("m_onActivate");
                                SerializedProperty onExpire = serializedObject.FindProperty("m_buffData").GetArrayElementAtIndex(i).FindPropertyRelative("m_onExpire");
                                EditorGUILayout.LabelField("Unity Events");
                                EditorGUILayout.PropertyField(onActivate);
                                if (!isPermanent) EditorGUILayout.PropertyField(onExpire);
                                break;
                            default:
                                Debug.LogError("Idk how but you borked the Buff System");
                                break;
                        }

                        

                        if (EditorGUI.EndChangeCheck())
                        {
                            Undo.RecordObject(target, "Changed Status Effect " + i);

                            current.SetName(name);
                            current.SetBuffID(buffID);
                            current.SetIsStackable(isStackable);
                            current.SetBuffType(buffType);
                            current.SetPermanent(isPermanent);
                            if (!isPermanent) current.SetTime(time);

                            if(current.GetBuffType == 0)
                            {
                                current.SetMaxHealth(maxHealthValue);
                                current.SetMoveSpeed(moveSpeedValue);
                                current.SetShootSpeed(shootSpeedValue);
                                current.SetDamage(damageValue);
                            }

                            serializedObject.ApplyModifiedProperties();
                        }
                    }

                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Space();
                }


                if (GUILayout.Button("Create new buff"))
                {
                    Undo.RecordObject(target, "Added new Status Effect");
                    m_system.CreateData();
                }

            }


            private void _Delete(int dataToRemove)
            {

                BuffDataSystem.BuffData[] newData = new BuffDataSystem.BuffData[m_system.GetData.Length - 1];
                int count = 0;

                for (int i = 0; i < m_system.GetData.Length; i++)
                {
                    if (dataToRemove == i) continue;

                    newData[count] = m_system.GetData[i];
                    count++;
                }

                Undo.RecordObject(target, "Removed Status Effect " + dataToRemove);
                m_system.ChangeData(newData);

            }

            /// <summary>
            /// Just for showing data in the inspector. This isn't as good as like debug logs or whatever as this can't be called in update (you need to wave your mouse around for it to work) but it is more readable.
            /// </summary>
            private void _Debug()
            {
                //Debug.Log("I'm Playing wowie!!!");

                Array.Resize(ref m_foldOutDebugs, m_system.GetActiveBuffs.Count);

                for (int i = 0; i < m_system.GetActiveBuffs.Count; i++)
                {
                    //replaces title with a default name if no name is present
                    string title = (m_system.GetActiveBuffs[i].GetData.GetName == "") ? "Status Effect " + i + ":" : m_system.GetActiveBuffs[i].GetData.GetName + " (" + i + "):";

                    EditorGUILayout.BeginHorizontal();

                    m_foldOutDebugs[i] = EditorGUILayout.Foldout(m_foldOutDebugs[i], title);
                    if (m_foldOutDebugs[i])
                    {
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.LabelField("ID: " + m_system.GetActiveBuffs[i].GetData.GetBuffID);

                        if (!m_system.GetActiveBuffs[i].GetData.GetPermanent) EditorGUILayout.LabelField("Buff time remaining: " + m_system.GetActiveBuffs[i].CurrentTime);
                        else EditorGUILayout.LabelField("Buff time remaining: N/A");

                        switch (m_system.GetActiveBuffs[i].GetData.GetBuffType)
                        {
                            case 0:
                                EditorGUILayout.LabelField("Buff Type: Change Player Stats");
                                EditorGUILayout.Space();
                                EditorGUILayout.LabelField("Max Health Add: " + m_system.GetActiveBuffs[i].GetData.GetMaxHealth);
                                EditorGUILayout.LabelField("Move speed Add: " + m_system.GetActiveBuffs[i].GetData.GetMoveSpeed);
                                EditorGUILayout.LabelField("Shoot Speed Multiplier Add: " + m_system.GetActiveBuffs[i].GetData.GetShootSpeed);
                                EditorGUILayout.LabelField("Damage Multiplier Add: " + m_system.GetActiveBuffs[i].GetData.GetDamage);
                                break;
                            case 1:
                                EditorGUILayout.LabelField("Buff Type: Invoke Unity Events");
                                break;
                            default:
                                EditorGUILayout.LabelField("Buff Type: Uhhh this is brokeden :(((");
                                break;
                        }

                        EditorGUILayout.Space();
                    }
                    else
                    {
                        if (!m_system.GetActiveBuffs[i].GetData.GetPermanent) EditorGUILayout.LabelField("Time: " + m_system.GetActiveBuffs[i].CurrentTime);
                        EditorGUILayout.EndHorizontal();
                    }
                }
            }

            
        }

    }
}


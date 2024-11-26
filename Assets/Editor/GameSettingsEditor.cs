using ILOVEYOU.Management;
using ILOVEYOU.Tools;
using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace ILOVEYOU.EditorScript
{
    [CustomEditor(typeof(GameSettings))]
    [CanEditMultipleObjects]
    public class GameSettingsEditor : Editor
    {
        GameSettings m_target;
        SerializedProperty m_announcProp;
        SerializedProperty m_diffCapProp;
        SerializedProperty m_maxTasksProp;
        SerializedProperty m_taskListProp;
        SerializedProperty m_tasksHealProp;
        SerializedProperty m_cardCountProp;
        SerializedProperty m_cardTimeOutProp;
        SerializedProperty m_cardDataProp;
        SerializedProperty m_doAllowDoubleProp;
        SerializedProperty m_playerHealthProp;
        SerializedProperty m_iFramesProp;
        SerializedProperty m_playerSpeedProp;
        SerializedProperty m_playerShootingProp;
        SerializedProperty m_useUnseenProp;
        SerializedProperty m_unseenCardsProp;
        SerializedProperty m_unseenRateProp;
        SerializedProperty m_knockbackWindowProp;
        SerializedProperty m_knockbackStrengthProp;
        SerializedProperty m_knockbackRadiusProp;
        SerializedProperty m_knockbackStunProp;
        SerializedProperty m_enemyGroupsProp;
        SerializedProperty m_spawnRangeMinProp;
        SerializedProperty m_spawnRangeMaxProp;
        SerializedProperty m_spawnTimeProp;
        SerializedProperty m_spawnCapProp;
        SerializedProperty m_modListProp;
        SerializedProperty m_modChanceProp;
        SerializedProperty m_modCapProp;
        SerializedProperty m_colorsProp;
        //Dictionary<string, object> SavedValues = new();
        //bool cardsEnabled = true;
        //bool cardsUpdated = true;

        private ReorderableList m_taskList;

        bool m_displayTaskSettings = false;
        /*bool m_tasksAreEnabled
        {
            get { return m_taskListProp.arraySize > 0; }
            set
            {
                switch (value)
                {
                    case true:
                        if (m_taskListProp.arraySize == 0)
                        {
                            m_taskListProp.InsertArrayElementAtIndex(0);
                        }
                        break;
                    case false:
                        m_taskListProp.ClearArray();
                        break;
                }
            }
        }*/
        bool m_displayCardSettings = false;
        bool m_cardsAreEnabled
        {
            get { return m_cardDataProp.arraySize > 0; }
            set
            {
                switch (value)
                {
                    case true:
                        if (m_cardDataProp.arraySize == 0)
                        {
                            m_cardDataProp.InsertArrayElementAtIndex(0);
                        }
                        break;
                    case false:
                        m_cardDataProp.ClearArray();
                        break;
                }
            }
        }
        bool m_displayPlayerSettings = false;
        bool m_displayUnseenSettings = false;
        bool m_displayKnockbackSettings = false;
        bool m_usingKnockback
        {
            get { return m_knockbackStrengthProp.vector2Value.x >= 0 && m_knockbackStrengthProp.vector2Value.y >= 0 && m_usingCardBurst; }
            set
            {
                switch (value)
                {
                    case true:
                        if (m_knockbackStrengthProp.vector2Value.x < 0 || m_knockbackStrengthProp.vector2Value.y < 0)
                            m_knockbackStrengthProp.vector2Value = Vector2.zero;
                        m_usingCardBurst = true;
                        break;
                    case false:
                        m_knockbackStrengthProp.vector2Value = new Vector2(-1, -1);
                        break;
                }
            }
        }
        bool m_displayCardBurstSettings = false;
        bool m_usingCardBurst
        {
            get { return m_knockbackRadiusProp.floatValue > 0 || m_knockbackWindowProp.floatValue > 0; }
            set
            {
                switch (value)
                {
                    case true:
                        if (m_knockbackRadiusProp.floatValue <= 0)
                            m_knockbackRadiusProp.floatValue = 1;
                        if (m_knockbackWindowProp.floatValue <= 0)
                            m_knockbackWindowProp.floatValue = 1;
                        break;
                    case false:
                        m_knockbackRadiusProp.floatValue = 0;
                        m_knockbackWindowProp.floatValue = 0;
                        break;
                }
            }
        }
        bool m_displayEnemySettings = false;
        bool m_displayEnemyModifiers = false;
        bool m_usingEnemyModifiers
        {
            get { return m_modListProp.arraySize > 0; }
            set
            {
                switch (value)
                {
                    case true:
                        if (m_modListProp.arraySize == 0)
                        {
                            m_modListProp.InsertArrayElementAtIndex(0);
                        }
                        break;
                    case false:
                        m_modListProp.ClearArray();
                        break;
                }
            }
        }
        bool m_displayColorStyles = false;
        private void OnEnable()
        {
            m_target = (GameSettings)target;
            m_announcProp = serializedObject.FindProperty("m_announcement");
            m_diffCapProp = serializedObject.FindProperty("m_difficultyCap");
            m_maxTasksProp = serializedObject.FindProperty("m_maxTaskCount");
            m_taskListProp = serializedObject.FindProperty("m_taskList");
            m_tasksHealProp = serializedObject.FindProperty("m_tasksCanHeal");
            m_cardCountProp = serializedObject.FindProperty("m_numberOfCardToGive");
            m_cardTimeOutProp = serializedObject.FindProperty("m_cardTimeOut");
            m_cardDataProp = serializedObject.FindProperty("m_cardData");
            m_doAllowDoubleProp = serializedObject.FindProperty("m_allowDoubleUps");
            m_playerHealthProp = serializedObject.FindProperty("m_playerHealth");
            m_iFramesProp = serializedObject.FindProperty("m_iframes");
            m_playerSpeedProp = serializedObject.FindProperty("m_playerSpeed");
            m_playerShootingProp = serializedObject.FindProperty("m_playerShootingPattern");
            m_useUnseenProp = serializedObject.FindProperty("m_useUnseen");
            m_unseenCardsProp = serializedObject.FindProperty("m_unseenCards");
            m_unseenRateProp = serializedObject.FindProperty("m_unseenRate");
            m_knockbackWindowProp = serializedObject.FindProperty("m_knockbackWindow");
            m_knockbackStrengthProp = serializedObject.FindProperty("m_knockbackStrength");
            m_knockbackRadiusProp = serializedObject.FindProperty("m_knockbackRadius");
            m_knockbackStunProp = serializedObject.FindProperty("m_knockbackStunDuration");
            m_enemyGroupsProp = serializedObject.FindProperty("m_enemyGroups");
            m_spawnRangeMinProp = serializedObject.FindProperty("m_spawnRangeMin");
            m_spawnRangeMaxProp = serializedObject.FindProperty("m_spawnRangeMax");
            m_spawnTimeProp = serializedObject.FindProperty("m_spawnTime");
            m_spawnCapProp = serializedObject.FindProperty("m_spawnCap");
            m_modListProp = serializedObject.FindProperty("m_modList");
            m_modChanceProp = serializedObject.FindProperty("m_modChanceOverTime");
            m_modCapProp = serializedObject.FindProperty("m_modCountOverTime");
            m_colorsProp = serializedObject.FindProperty("m_prefColors");
        }
        public override void OnInspectorGUI()
        {
            m_taskList = new(serializedObject, serializedObject.FindProperty("m_taskList"), true, true, true, true);

            EditorGUILayout.HelpBox("These are the settings that will be used in-game. To assign settings, find the \"settings\" variable in the GameManager, or click assign to change settings in realtime\n(Assigned settings will reset on scene start).", MessageType.Info);
            EditorGUILayout.BeginHorizontal();
            float scale = EditorGUIUtility.currentViewWidth / 3 - 10;
            if (GUILayout.Button("Assign", GUILayout.Width(scale)))
            {
                m_target.Assign();
            }
            if(GUILayout.Button("Initialize color prefs", GUILayout.Width(scale)))
            {
                m_target.InitalizePrefs();
            }
            if(GUILayout.Button("Export to JSON", GUILayout.Width(scale)))
            {
                JsonHandler.WriteData(m_target, m_target.name);
            }
            EditorGUILayout.EndHorizontal();

            serializedObject.Update();

            #region misc
            //title
            EditorGUILayout.PropertyField(m_announcProp, new GUIContent("Announcement title"));
            //difficulty
            EditorGUILayout.LabelField("Difficulty Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_diffCapProp, new GUIContent("Max Difficulty Value"));
            if (m_diffCapProp.floatValue < 0) m_diffCapProp.floatValue = 0;
            #endregion
            #region Task Settings
            GUILayout.Space(16);
            m_displayTaskSettings = EditorGUILayout.Foldout(m_displayTaskSettings, "Task settings");
            if (m_displayTaskSettings)
            {
                EditorGUILayout.PropertyField(m_tasksHealProp, new GUIContent("Can tasks heal"));
                
                //Draw the elements in the array
                m_taskList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    rect.y += 2;
                    var element = m_taskList.serializedProperty.GetArrayElementAtIndex(index);
                    float valueSize = 100;
                    //type
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width - valueSize - 5, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("m_type"), GUIContent.none);
                    //value
                    EditorGUI.PropertyField(new Rect(rect.x + rect.width - valueSize, rect.y, valueSize, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("m_capValue"), GUIContent.none);
                    //only if tasks should heal...
                    if (m_tasksHealProp.boolValue)
                    {
                        //...should the heal value be drawn.
                        rect.y += EditorGUIUtility.singleLineHeight + 2;
                        EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("m_healAmount"), new GUIContent("Heal amount"));
                    }


                };
                if (m_tasksHealProp.boolValue)
                    m_taskList.elementHeight = EditorGUIUtility.singleLineHeight * 2 + 5;

                m_taskList.drawHeaderCallback = (Rect rect) =>
                {
                    EditorGUI.LabelField(rect, "Task list");
                };

                m_taskList.DoLayoutList();

            }
            if(m_taskListProp.arraySize < 2)
            {
                Debug.LogWarning("Array requires at least 2 tasks. The first is used at the start and only then. Everything is randomly used afterwards.");
                if (m_taskListProp.arraySize < 1)
                {
                    Debug.LogError("I'm gonna get you.");
                    m_taskListProp.InsertArrayElementAtIndex(0);
                }
                m_taskListProp.InsertArrayElementAtIndex(1);

            }
            #endregion
            #region Card Settings
            GUILayout.Space(16);
            if (m_cardsAreEnabled)
            {
                EditorGUILayout.BeginHorizontal();
                m_displayCardSettings = EditorGUILayout.Foldout(m_displayCardSettings, new GUIContent("Card Settings"));
                if (GUILayout.Button("Disable cards"))
                {
                    m_cardsAreEnabled = false;
                    m_displayCardSettings = false;
                }
                EditorGUILayout.EndHorizontal();

                if (m_displayCardSettings)
                {
                    EditorGUILayout.PropertyField(m_cardDataProp);
                    if (m_target.GetCardData[0].DisruptCard != null)
                    {
                        //EditorGUILayout.IntSlider(m_cardCountProp, 0, 3, new GUIContent("Card Cap"));
                        EditorGUILayout.PropertyField(m_cardTimeOutProp, new GUIContent("Card timeout"));
                        EditorGUILayout.PropertyField(m_doAllowDoubleProp, new GUIContent("Allow double ups"));
                    }
                }
            }
            else if (GUILayout.Button("Use cards"))
            {
                m_cardsAreEnabled = true;
            }
            #endregion
            #region Player Settings
            GUILayout.Space(16);
            m_displayPlayerSettings = EditorGUILayout.Foldout(m_displayPlayerSettings, new GUIContent("Player Settings"));
            if (m_displayPlayerSettings)
            {
                EditorGUILayout.PropertyField(m_playerHealthProp, new GUIContent("Player Health"));
                EditorGUILayout.PropertyField(m_iFramesProp, new GUIContent("iFrame Duration"));
                EditorGUILayout.PropertyField(m_playerSpeedProp, new GUIContent("Player Speed"));
                EditorGUILayout.PropertyField(m_playerShootingProp, new GUIContent("Player Shooting Pattern"));
            }
            #endregion
            #region Unseen AI Settings
            GUILayout.Space(16);
            if (m_useUnseenProp.boolValue)
            {
                EditorGUILayout.BeginHorizontal();
                m_displayUnseenSettings = EditorGUILayout.Foldout(m_displayUnseenSettings, new GUIContent("Unseen AI Settings"));
                if (GUILayout.Button("Disable Unseen AI"))
                {
                    m_useUnseenProp.boolValue = false;
                    m_displayUnseenSettings = false;
                }
                EditorGUILayout.EndHorizontal();

                if (m_displayUnseenSettings)
                {
                    if (m_unseenCardsProp.arraySize == 0)
                        m_unseenCardsProp.InsertArrayElementAtIndex(0);
                    EditorGUILayout.PropertyField(m_unseenRateProp, new GUIContent("Card trigger rate"));
                    EditorGUILayout.PropertyField(m_unseenCardsProp, new GUIContent("Cards to use"));
                }
            }
            else
            {
                m_unseenCardsProp.ClearArray();
                if (GUILayout.Button("Use Unseen AI"))
                {
                    m_useUnseenProp.boolValue = true;
                    m_displayUnseenSettings = true;
                }
            }
            #endregion
            #region Card Burst Settings
            GUILayout.Space(16);
            if (m_usingCardBurst)
            {
                EditorGUILayout.BeginHorizontal();
                m_displayCardBurstSettings = EditorGUILayout.Foldout(m_displayCardBurstSettings, new GUIContent("Card Burst Settings"));
                if (GUILayout.Button("Disable Card Burst"))
                {
                    m_usingCardBurst = false;
                    m_displayCardBurstSettings = false;
                    m_displayKnockbackSettings = false;
                }
                EditorGUILayout.EndHorizontal();

                if (m_displayCardBurstSettings)
                {
                    EditorGUILayout.PropertyField(m_knockbackWindowProp, new GUIContent("Knockback Window"));
                    if(m_knockbackWindowProp.floatValue <= 0)
                    {
                        Debug.LogWarning("Knockback window time invalid! Click disable knockback if you want to turn knockback off.");
                        m_knockbackWindowProp.floatValue = 0.001f;
                    }
                    EditorGUILayout.PropertyField(m_knockbackRadiusProp, new GUIContent("Knockback Radius"));
                    if (m_knockbackRadiusProp.floatValue <= 0)
                    {
                        Debug.LogWarning("Knockback radius invalid! Click disable knockback if you want to turn knockback off.");
                        m_knockbackRadiusProp.floatValue = 0.001f;
                    }
                }
                if (m_usingKnockback)
                {
                    EditorGUILayout.BeginHorizontal();
                    m_displayKnockbackSettings = EditorGUILayout.Foldout(m_displayKnockbackSettings, new GUIContent("Knockback Settings"));
                    if (GUILayout.Button("Disable Knockback"))
                    {
                        m_usingKnockback = false;
                        m_displayKnockbackSettings = false;
                    }
                    EditorGUILayout.EndHorizontal();

                    if (m_displayKnockbackSettings)
                    {
                        EditorGUILayout.PropertyField(m_knockbackStrengthProp, new GUIContent("Knockback Strength"));
                        EditorGUILayout.PropertyField(m_knockbackStunProp, new GUIContent("Knockback Stun Duration"));
                        if (m_knockbackStunProp.floatValue < 0)
                        {
                            m_knockbackStunProp.floatValue = 0;
                        }
                    }

                }
                else if(GUILayout.Button("Enable Knockback"))
                {
                    m_usingKnockback = true;
                    m_displayKnockbackSettings = true;
                }
            }
            else if (GUILayout.Button("Enable Card Burst"))
            {
                m_usingCardBurst = true;
                m_displayCardBurstSettings = true;
            }
            #endregion
            #region Enemy Settings
            GUILayout.Space(16);
            m_displayEnemySettings = EditorGUILayout.Foldout(m_displayEnemySettings, new GUIContent("Enemy settings"));
            if (m_displayEnemySettings)
            {
                EditorGUILayout.PropertyField(m_enemyGroupsProp, new GUIContent("Enemy Groups"));
                int width = Screen.width;
                if (m_enemyGroupsProp.arraySize > 0)
                {
                    //EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"Spawn Range | {Mathf.Round((1 - m_spawnRangeMinProp.floatValue/m_spawnRangeMaxProp.floatValue) * 100)}% of maximum range");
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(m_spawnRangeMinProp, new GUIContent("Min"));//, GUILayout.ExpandWidth(false), GUILayout.MaxWidth(60));
                    if(m_spawnRangeMinProp.floatValue < 0)
                    {
                        Debug.Log("The minimum spawn range cannot be less then zero");
                        m_spawnRangeMinProp.floatValue = 0;
                    }
                    if(m_spawnRangeMinProp.floatValue > m_spawnRangeMaxProp.floatValue)
                    {
                        m_spawnRangeMaxProp.floatValue = m_spawnRangeMinProp.floatValue;
                    }

                    EditorGUILayout.PropertyField(m_spawnRangeMaxProp, new GUIContent("Max"));//, GUILayout.ExpandWidth(false), GUILayout.MaxWidth(60));
                    if (m_spawnRangeMaxProp.floatValue < m_spawnRangeMinProp.floatValue)
                    {
                        m_spawnRangeMinProp.floatValue = m_spawnRangeMaxProp.floatValue;
                    }
                    EditorGUILayout.EndHorizontal();
                    
                    EditorGUILayout.LabelField("Enemy spawn rate and cap");
                    EditorGUILayout.PropertyField(m_spawnTimeProp, new GUIContent("Rate"));
                    EditorGUILayout.PropertyField(m_spawnCapProp, new GUIContent("Cap"));
                }

                if (m_usingEnemyModifiers)
                {
                    m_displayEnemyModifiers =  EditorGUILayout.Foldout(m_displayEnemyModifiers, new GUIContent("Enemy Modifiers"));
                    if (m_displayEnemyModifiers)
                    {
                        EditorGUILayout.PropertyField(m_modListProp, new GUIContent("Mod list"));
                        EditorGUILayout.PropertyField(m_modChanceProp, new GUIContent("Mod chance"));
                        EditorGUILayout.PropertyField(m_modCapProp, new GUIContent("Maximum Mod Count"));
                        if(GUILayout.Button("Remove Enemy Modifiers"))
                        {
                            m_usingEnemyModifiers = false;
                        }
                    }
                }
                else
                {
                    if(GUILayout.Button("Use Enemy Modifiers"))
                    {
                        m_usingEnemyModifiers = true;
                    }
                }
            }
            #endregion
            #region Colour Settings
            GUILayout.Space(16);
            m_displayColorStyles = EditorGUILayout.Foldout(m_displayColorStyles, new GUIContent("Colors"));
            if (m_displayColorStyles)
            {
                EditorGUILayout.PropertyField(m_colorsProp, new GUIContent("Game Color"));
            }
            #endregion

            serializedObject.ApplyModifiedProperties();
            //base.OnInspectorGUI();
        }
    }
}
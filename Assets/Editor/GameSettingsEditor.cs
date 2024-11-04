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
        SerializedProperty m_diffCapProp;
        SerializedProperty m_maxTasksProp;
        SerializedProperty m_taskListProp;
        SerializedProperty m_cardCountProp;
        SerializedProperty m_cardTimeOutProp;
        SerializedProperty m_cardDataProp;
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
        SerializedProperty m_colorsProp;
        //Dictionary<string, object> SavedValues = new();
        //bool cardsEnabled = true;
        //bool cardsUpdated = true;

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
        bool m_displayKnockbackSettings = false;
        bool m_usingKnockback
        {
            get { return m_knockbackRadiusProp.floatValue > 0 || m_knockbackWindowProp.floatValue > 0; }
            set
            {
                switch (value) { 
                    case true:
                        if (m_knockbackRadiusProp.floatValue <= 0)
                        {
                            m_knockbackRadiusProp.floatValue = 1;
                        }
                        if(m_knockbackWindowProp.floatValue <= 0)
                        { 
                            m_knockbackWindowProp.floatValue = 1;
                        }
                        break;
                    case false:
                        m_knockbackRadiusProp.floatValue = 0;
                        m_knockbackWindowProp.floatValue = 0;
                        break;
                }
            }
        }
        bool m_displayEnemySettings = false;
        bool m_displayColorStyles = false;
        private void OnEnable()
        {
            m_target = (GameSettings)target;
            m_diffCapProp = serializedObject.FindProperty("m_difficultyCap");
            m_maxTasksProp = serializedObject.FindProperty("m_maxTaskCount");
            m_taskListProp = serializedObject.FindProperty("m_taskList");
            m_cardCountProp = serializedObject.FindProperty("m_numberOfCardToGive");
            m_cardTimeOutProp = serializedObject.FindProperty("m_cardTimeOut");
            m_cardDataProp = serializedObject.FindProperty("m_cardData");
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
            m_colorsProp = serializedObject.FindProperty("m_prefColors");
        }
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("These are the settings that will be used in-game. To assign settings, find the \"settings\" variable in the GameManager, or click assign to change settings in realtime\n(Assigned settings will reset on scene start).", MessageType.Info);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Assign"))
            {
                m_target.Assign();
            }
            if(GUILayout.Button("Initialize color preferences"))
            {
                m_target.InitalizePrefs();
            }
            EditorGUILayout.EndHorizontal();

            serializedObject.Update();

            //difficulty
            EditorGUILayout.LabelField("Difficulty Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_diffCapProp, new GUIContent("Max Difficulty Value"));
            if (m_diffCapProp.floatValue < 0) m_diffCapProp.floatValue = 0;

            //task
            GUILayout.Space(16);
            m_displayTaskSettings = EditorGUILayout.Foldout(m_displayTaskSettings, "Task settings");
            if (m_displayTaskSettings)
            {
                //EditorGUILayout.PropertyField(m_maxTasksProp, new GUIContent("Max Task Count"));
                EditorGUILayout.PropertyField(m_taskListProp, new GUIContent("Task List"));
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

            //card
            GUILayout.Space(16);
            if (m_cardsAreEnabled)
            {
                m_displayCardSettings = EditorGUILayout.Foldout(m_displayCardSettings, new GUIContent("Card Settings"));
                if (m_displayCardSettings)
                {
                    EditorGUILayout.PropertyField(m_cardDataProp);
                    if (m_target.GetCardData[0].DisruptCard != null)
                    {
                        //EditorGUILayout.IntSlider(m_cardCountProp, 0, 3, new GUIContent("Card Cap"));
                        EditorGUILayout.PropertyField(m_cardTimeOutProp, new GUIContent("Card timeout"));
                    }
                    if (GUILayout.Button("Remove cards"))
                        m_cardsAreEnabled = false;
                }
            }
            else
            {
                if (GUILayout.Button("Use cards"))
                {
                    m_cardsAreEnabled = true;
                }
            }

            //player
            GUILayout.Space(16);
            m_displayPlayerSettings = EditorGUILayout.Foldout(m_displayPlayerSettings, new GUIContent("Player Settings"));
            if (m_displayPlayerSettings)
            {
                EditorGUILayout.PropertyField(m_playerHealthProp, new GUIContent("Player Health"));
                EditorGUILayout.PropertyField(m_iFramesProp, new GUIContent("iFrame Duration"));
                EditorGUILayout.PropertyField(m_playerSpeedProp, new GUIContent("Player Speed"));
                EditorGUILayout.PropertyField(m_playerShootingProp, new GUIContent("Player Shooting Pattern"));
            }

            //unseen ai
            GUILayout.Space(16);
            m_useUnseenProp.boolValue = EditorGUILayout.ToggleLeft(new GUIContent("Use Unseen AI"), m_useUnseenProp.boolValue);
            if (m_useUnseenProp.boolValue)
            {
                if (m_unseenCardsProp.arraySize == 0)
                    m_unseenCardsProp.InsertArrayElementAtIndex(0);
                EditorGUILayout.PropertyField(m_unseenRateProp, new GUIContent("Card trigger rate"));
                EditorGUILayout.PropertyField(m_unseenCardsProp, new GUIContent("Cards to use"));
            }
            else
            {
                m_unseenCardsProp.ClearArray();
            }

            //knockback
            GUILayout.Space(16);
            if (m_usingKnockback)
            {
                m_displayKnockbackSettings = EditorGUILayout.Foldout(m_displayKnockbackSettings, new GUIContent("Knockback Settings"));
                if (m_displayKnockbackSettings)
                {
                    EditorGUILayout.PropertyField(m_knockbackWindowProp, new GUIContent("Knockback Window"));
                    if(m_knockbackWindowProp.floatValue <= 0)
                    {
                        Debug.LogWarning("Knockback window time invalid! Click disable knockback if you want to turn knockback off.");
                        m_knockbackWindowProp.floatValue = 0.001f;
                    }
                    EditorGUILayout.PropertyField(m_knockbackStrengthProp, new GUIContent("Knockback Strength"));
                    EditorGUILayout.PropertyField(m_knockbackRadiusProp, new GUIContent("Knockback Radius"));
                    if (m_knockbackRadiusProp.floatValue <= 0)
                    {
                        Debug.LogWarning("Knockback radius invalid! Click disable knockback if you want to turn knockback off.");
                        m_knockbackRadiusProp.floatValue = 0.001f;
                    }
                    EditorGUILayout.PropertyField(m_knockbackStunProp, new GUIContent("Knockback Stun Duration"));
                    if(m_knockbackStunProp.floatValue < 0)
                    {
                        m_knockbackStunProp.floatValue = 0;
                    }
                    if (GUILayout.Button("Disable Knockback"))
                        m_usingKnockback = false;
                }
            }
            else
            {
                if(m_knockbackWindowProp.floatValue > 0 || m_knockbackRadiusProp.floatValue > 0)
                {
                    m_knockbackRadiusProp.floatValue = m_knockbackWindowProp.floatValue = 0;
                }
                if(GUILayout.Button("Enable Knockback"))
                {
                    m_usingKnockback = true;
                }
            }

            //enemy
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
            }

            //colors
            GUILayout.Space(16);
            m_displayColorStyles = EditorGUILayout.Foldout(m_displayColorStyles, new GUIContent("Colors"));
            if (m_displayColorStyles)
            {
                EditorGUILayout.PropertyField(m_colorsProp, new GUIContent("Game Color"));
            }







            serializedObject.ApplyModifiedProperties();
            //base.OnInspectorGUI();
        }
    }
}
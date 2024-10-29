using System.Collections.Generic;
using ILOVEYOU.Cards;
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
        SerializedProperty maxTasksProp;
        SerializedProperty taskListProp;
        SerializedProperty cardCountProp;
        SerializedProperty cardTimeOutProp;
        SerializedProperty cardDataProp;
        SerializedProperty playerHealthProp;
        SerializedProperty iFramesProp;
        SerializedProperty playerSpeedProp;
        SerializedProperty playerShootingProp;
        SerializedProperty knockbackWindowProp;
        SerializedProperty knockbackStrengthProp;
        SerializedProperty knockbackRadiusProp;
        SerializedProperty enemyGroupsProp;
        SerializedProperty spawnRangeMinProp;
        SerializedProperty spawnRangeMaxProp;
        SerializedProperty spawnTimeProp;
        SerializedProperty spawnCapProp;
        SerializedProperty colorsProp;
        //Dictionary<string, object> SavedValues = new();
        //bool cardsEnabled = true;
        //bool cardsUpdated = true;
        private void OnEnable()
        {
            m_target = (GameSettings)target;
            diffCapProp = serializedObject.FindProperty("m_difficultyCap");
            maxTasksProp = serializedObject.FindProperty("m_maxTaskCount");
            taskListProp = serializedObject.FindProperty("m_taskList");
            cardCountProp = serializedObject.FindProperty("m_numberOfCardToGive");
            cardTimeOutProp = serializedObject.FindProperty("m_cardTimeOut");
            cardDataProp = serializedObject.FindProperty("m_cardData");
            playerHealthProp = serializedObject.FindProperty("m_playerHealth");
            iFramesProp = serializedObject.FindProperty("m_iframes");
            playerSpeedProp = serializedObject.FindProperty("m_playerSpeed");
            playerShootingProp = serializedObject.FindProperty("m_playerShootingPattern");
            knockbackWindowProp = serializedObject.FindProperty("m_knockbackWindow");
            knockbackStrengthProp = serializedObject.FindProperty("m_knockbackStrength");
            knockbackRadiusProp = serializedObject.FindProperty("m_knockbackRadius");
            enemyGroupsProp = serializedObject.FindProperty("m_enemyGroups");
            spawnRangeMinProp = serializedObject.FindProperty("m_spawnRangeMin");
            spawnRangeMaxProp = serializedObject.FindProperty("m_spawnRangeMax");
            spawnTimeProp = serializedObject.FindProperty("m_spawnTime");
            spawnCapProp = serializedObject.FindProperty("m_spawnCap");
            colorsProp = serializedObject.FindProperty("m_prefColors");
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

            EditorGUILayout.LabelField("Difficulty", EditorStyles.boldLabel);
            EditorGUILayout.Slider(diffCapProp, 0, 60, new GUIContent("Difficulty Cap"));
            EditorGUILayout.LabelField("Tasks", EditorStyles.boldLabel);
            if(taskListProp.arraySize == 0){
                if(EditorGUILayout.LinkButton("Create Task List"))
                {
                    taskListProp.InsertArrayElementAtIndex(0);
                    taskListProp.InsertArrayElementAtIndex(1);
                }
            }
            else{
                EditorGUILayout.PropertyField(maxTasksProp, new GUIContent("Max Task Count"));
                EditorGUILayout.PropertyField(taskListProp.GetArrayElementAtIndex(0), new GUIContent("Initial task"));
                EditorGUILayout.PropertyField(taskListProp, new GUIContent("Task List"));
            }

            //cardsEnabled = EditorGUILayout.Toggle("Use cards", cardsEnabled);
            //if(cardsEnabled){
            //    if(cardsUpdated != cardsEnabled){
            //        cardsUpdated = cardsEnabled;
            //        cardCountProp.intValue = (int)SavedValues[cardCountProp.name];
            //        cardTimeOutProp.floatValue = (float)SavedValues[cardTimeOutProp.name];
            //        //dunno how to to card data.
            //    }
            //}
            //else{
            //    if(cardsUpdated != cardsEnabled){
            //        cardsUpdated = cardsEnabled;
            //        SavedValues[cardCountProp.name] = cardCountProp.intValue;
            //        cardCountProp.intValue = 0;
            //        SavedValues[cardTimeOutProp.name] = cardTimeOutProp.floatValue;
            //        cardTimeOutProp.floatValue = 0;
            //    }
            //}

            EditorGUILayout.LabelField("Cards", EditorStyles.boldLabel);
            if(cardDataProp.arraySize == 0){
                if(EditorGUILayout.LinkButton("Create card data")){
                    cardDataProp.InsertArrayElementAtIndex(0);
                }
            }
            else{
                EditorGUILayout.IntSlider(cardCountProp, 0, 3, new GUIContent("Card Cap"));
                EditorGUILayout.PropertyField(cardTimeOutProp, new GUIContent("Card timeout"));
                EditorGUILayout.PropertyField(cardDataProp);
            }

            EditorGUILayout.LabelField("Player", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(playerHealthProp, new GUIContent("Player Health"));
            EditorGUILayout.PropertyField(iFramesProp, new GUIContent("iFrame Duration"));
            EditorGUILayout.PropertyField(playerSpeedProp, new GUIContent("Player Speed"));
            EditorGUILayout.PropertyField(playerShootingProp, new GUIContent("Player Shooting Pattern"));
            EditorGUILayout.LabelField("Knockback", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(knockbackWindowProp, new GUIContent("Knockback Window"));
            EditorGUILayout.PropertyField(knockbackStrengthProp, new GUIContent("Knockback Strength"));
            EditorGUILayout.PropertyField(knockbackRadiusProp, new GUIContent("Knockback Radius"));
            EditorGUILayout.LabelField("Enemies and Spawning", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(enemyGroupsProp, new GUIContent("Enemy Groups"));
            EditorGUILayout.PropertyField(spawnRangeMinProp, new GUIContent("Min Spawn Range"));
            EditorGUILayout.PropertyField(spawnRangeMaxProp, new GUIContent("Max Spawn Range"));
            EditorGUILayout.PropertyField(spawnTimeProp, new GUIContent("Enemy Spawn Rate"));
            EditorGUILayout.PropertyField(spawnCapProp, new GUIContent("Spawn Cap"));
            EditorGUILayout.LabelField("Color settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(colorsProp, new GUIContent("Game Color"));







            serializedObject.ApplyModifiedProperties();
            //base.OnInspectorGUI();
        }
    }
}
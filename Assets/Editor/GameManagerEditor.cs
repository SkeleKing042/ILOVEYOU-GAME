using ILOVEYOU.Management;
using UnityEditor;
using UnityEngine;

namespace ILOVEYOU
{
    namespace EditorScript
    {

        [CustomEditor(typeof(GameManager))]
        public class GameManagerEditor : Editor
        {
            GameManager m_target;
            
            private void OnEnable()
            {
                m_target = (GameManager)target;
            }
            public override void OnInspectorGUI()
            {
                if(GUILayout.Button("Start Game"))
                {
                    Debug.Log("Manual game start called");
                    m_target.BeginSetup();
                }
                base.OnInspectorGUI();
            }
        }
    }
}
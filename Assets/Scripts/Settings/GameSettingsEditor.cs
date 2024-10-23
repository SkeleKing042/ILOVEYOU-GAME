using ILOVEYOU.Management;
using UnityEditor;

namespace ILOVEYOU.EditorScript
{
    [CustomEditor(typeof(GameSettings))]
    public class GameSettingsEditor : Editor
    {
        GameSettings m_target;
        private void OnEnable()
        {
            m_target = (GameSettings)target;
        }
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("These are the settings that will be used in-game. To assign settings, find the \"settings\" variable in the GameManager, or click assign to change settings in realtime\n(Assigned settings will reset on scene start).", MessageType.Info);
            if (EditorGUILayout.LinkButton("Assign"))
            {
                m_target.Assign();
            }
            base.OnInspectorGUI();
        }
    }
}
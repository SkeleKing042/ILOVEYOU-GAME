using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ILOVEYOU
{
    namespace Management
    {

        public class MainMenuScene : MonoBehaviour
        {
            [SerializeField] private string m_sceneName;
            public void Awake()
            {
                GameManager.ResetScore();
            }
            public void TriggerSceneChange()
            {
                if (ControllerManager.Instance.ControllerCount == 2)
                    StartCoroutine(_changeScene());
                else
                {
                    Debug.LogWarning("Not enough controllers connected.");
                }
            }
            private IEnumerator _changeScene()
            {
                //unload the current scene - this won't affect this object
                //SceneManager.UnloadSceneAsync(gameObject.scene);
                //load the next scene
                SceneManager.LoadSceneAsync(m_sceneName);
                return null;
            }
            public void QuitApp()
            {
#if UNITY_EDITOR
                EditorApplication.ExitPlaymode();
#else
                Application.Quit();
#endif
            }
        }
    }
}
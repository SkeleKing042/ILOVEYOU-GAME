using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using AsyncOperation = UnityEngine.AsyncOperation;

namespace ILOVEYOU.MainMenu
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private int m_loadingScene;
        private string m_sceneName;
        private Slider m_loadingSlider;
        private TextMeshProUGUI[] m_text;

        public static SceneLoader Instance;

        //Create instance and destroy duplicates
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else { Destroy(gameObject); }
        }

        /// <summary>
        /// Loads scene by index of build layout
        /// </summary>
        public void LoadScene(int sceneNumber)
        {
            StartCoroutine(_LoadAsync(sceneNumber));

        }
        /// <summary>
        /// Loads scene by name of scene
        /// </summary>
        public void LoadScene(string sceneName)
        {
            StartCoroutine(_LoadAsync(sceneName));
        }
        /// <summary>
        /// Reloads currently active scene
        /// </summary>
        public void RestartScene()
        {
            //m_sceneName = SceneManager.GetActiveScene().name;

            StartCoroutine(_LoadAsync(SceneManager.GetActiveScene().name));
        }
        /// <summary>
        /// Loads new scene by going to the SceneLoader scene and then going to the scene
        /// </summary>
        IEnumerator _LoadAsync(int sceneNumber)
        {
            //First goes to the loading scene
            AsyncOperation operation = SceneManager.LoadSceneAsync(m_loadingScene);
            //makes sure the loading scene is loaded
            while (!operation.isDone)
            {
                yield return new WaitForEndOfFrame();
            }
            //starts loading next scene
            operation = SceneManager.LoadSceneAsync(sceneNumber);
            operation.allowSceneActivation = false;
            //gets the name of the next scene
            m_sceneName = SceneManager.GetSceneByBuildIndex(sceneNumber).name;
            //gets and sets values in the loading scene
            m_loadingSlider = FindObjectOfType<Slider>();
            m_text = FindObjectsOfType<TextMeshProUGUI>();
            m_text[0].text = "Loading " + m_sceneName + "...";

            yield return new WaitForEndOfFrame();
            //while loading update loading bar
            while (!operation.isDone)
            {
                m_loadingSlider.value = Mathf.RoundToInt(operation.progress * 75f); //multi by 75 as the loading bar is 0-75
                m_text[1].text = Mathf.RoundToInt(operation.progress * 100f) + "%";
                //once finished loading go to next scene after short delay
                if (operation.progress >= 0.9f) // <- Unity loading "Finishes" at .9, hence why it considers it done past .9
                {
                    m_loadingSlider.value = 75f;
                    m_text[1].text = "100%";
                    yield return new WaitForSecondsRealtime(.5f);
                    operation.allowSceneActivation = true; //enables the next scene
                }

                yield return new WaitForEndOfFrame();
            }

            m_loadingSlider = null;

            yield return null;
        }
        /// <summary>
        /// Loads new scene by going to the SceneLoader scene and then going to the scene
        /// </summary>
        IEnumerator _LoadAsync(string sceneName)
        {
            //First goes to the loading scene
            AsyncOperation operation = SceneManager.LoadSceneAsync(m_loadingScene);
            //makes sure the loading scene is loaded
            while (!operation.isDone)
            {
                yield return new WaitForEndOfFrame();
            }
            //starts loading next scene
            operation = SceneManager.LoadSceneAsync(sceneName);
            operation.allowSceneActivation = false;
            //gets the name of the next scene
            m_sceneName = SceneManager.GetSceneByName(sceneName).name;
            //gets and sets values in the loading scene
            m_loadingSlider = FindObjectOfType<Slider>();
            m_text = FindObjectsOfType<TextMeshProUGUI>();
            m_text[0].text = "Loading " + m_sceneName + "...";

            yield return new WaitForEndOfFrame();

            //while loading update loading bar
            while (!operation.isDone)
            {
                m_loadingSlider.value = Mathf.RoundToInt(operation.progress * 75f); //multi by 75 as the loading bar is 0-75
                m_text[1].text = Mathf.RoundToInt(operation.progress * 100f) + "%";
                //once finished loading go to next scene after short delay
                if (operation.progress >= 0.9f) // <- Unity loading "Finishes" at .9, hence why it considers it done past .9
                {
                    m_loadingSlider.value = 75f;
                    m_text[1].text = "100%";
                    yield return new WaitForSecondsRealtime(.5f);
                    operation.allowSceneActivation = true; //enables the next scene
                }

                yield return new WaitForEndOfFrame();
            }

            m_loadingSlider = null;

            yield return null;
        }
    }

}


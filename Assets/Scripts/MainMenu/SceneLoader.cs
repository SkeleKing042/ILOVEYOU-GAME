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

        //Create instance
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else { Destroy(gameObject); }
        }


        public void LoadScene(int sceneNumber)
        {
            m_sceneName = SceneManager.GetSceneByBuildIndex(sceneNumber).name;

            StartCoroutine(_LoadAsync(sceneNumber));

        }

        public void LoadScene(string sceneName)
        {
            m_sceneName = SceneManager.GetSceneByName(sceneName).name;

            StartCoroutine(_LoadAsync(sceneName));
        }

        public void RestartScene()
        {
            m_sceneName = SceneManager.GetActiveScene().name;

            StartCoroutine(_LoadAsync(SceneManager.GetActiveScene().name));
        }

        IEnumerator _LoadAsync(int sceneNumber)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(m_loadingScene);

            //Debug.Break();

            while (!operation.isDone)
            {
                yield return new WaitForEndOfFrame();
            }

            m_loadingSlider = FindObjectOfType<Slider>();
            m_text = FindObjectsOfType<TextMeshProUGUI>();
            m_text[0].text = "Loading " + m_sceneName + "...";

            operation = SceneManager.LoadSceneAsync(sceneNumber);
            operation.allowSceneActivation = false;

            yield return new WaitForEndOfFrame();

            while (!operation.isDone)
            {
                m_loadingSlider.value = Mathf.RoundToInt(operation.progress * 75f); //multi by 75 as the loading bar is 0-75
                m_text[1].text = Mathf.RoundToInt(operation.progress * 100f) + "%";


                if (operation.progress >= 0.9f)
                {
                    m_loadingSlider.value = 75f;
                    m_text[1].text = "100%";
                    yield return new WaitForSecondsRealtime(.5f);
                    operation.allowSceneActivation = true;
                }

                yield return new WaitForEndOfFrame();
            }

            m_loadingSlider = null;

            yield return null;
        }

        IEnumerator _LoadAsync(string sceneName)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(m_loadingScene);

            //Debug.Break();

            while (!operation.isDone)
            {
                yield return new WaitForEndOfFrame();
            }

            m_loadingSlider = FindObjectOfType<Slider>();
            m_text = FindObjectsOfType<TextMeshProUGUI>();
            m_text[0].text = "Loading " + m_sceneName + "...";

            operation = SceneManager.LoadSceneAsync(sceneName);
            operation.allowSceneActivation = false;

            yield return new WaitForEndOfFrame();

            while (!operation.isDone)
            {
                m_loadingSlider.value = Mathf.RoundToInt(operation.progress * 75f); //multi by 75 as the loading bar is 0-75
                m_text[1].text = Mathf.RoundToInt(operation.progress * 100f) + "%";


                if (operation.progress >= 0.9f)
                {
                    m_loadingSlider.value = 75f;
                    m_text[1].text = "100%";
                    yield return new WaitForSecondsRealtime(.5f);
                    operation.allowSceneActivation = true;
                }

                yield return new WaitForEndOfFrame();
            }

            m_loadingSlider = null;

            yield return null;
        }
    }

}


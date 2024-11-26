using ILOVEYOU.Audio;
using ILOVEYOU.MainMenu;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

namespace ILOVEYOU.Management 
{
    public class MenuPopUp : MonoBehaviour
    {
        [SerializeField] private GameObject m_firstSelected;
        private GameObject m_returnObject;

        // Start is called before the first frame update
        void Start()
        {
            //transform.GetChild(2).gameObject.SetActive(true);
            FindObjectOfType<MultiplayerEventSystem>().SetSelectedGameObject(m_firstSelected);
        }

        public void Return()
        {
            if (m_returnObject) FindObjectOfType<MultiplayerEventSystem>().SetSelectedGameObject(m_returnObject);
            FindObjectOfType<MenuManager>().CheckPlayerCounts();

            Destroy(gameObject);
        }

        public void PlaySound(string soundName)
        {
            SoundManager.UI.PlayRandomSound(soundName);
        }

        /// <summary>
        /// sets return object for when the popup is closed
        /// </summary>
        public void SetReturn(GameObject returnObj)
        {
            m_returnObject = returnObj;
        }

        public void ChangeScene(int scene)
        {
            GameManager.Instance.LoadScene(scene);
        }

        public void Restart()
        {
            GameManager.Instance.RestartScene();
        }

        public void Resume()
        {
            GameManager.Instance.ResumeGame();
        }
    }
}



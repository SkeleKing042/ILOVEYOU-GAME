using ILOVEYOU.Management;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ILOVEYOU.MainMenu
{
    public class MenuManager : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI m_inputField;
        private TypeWriterEffect m_effect;
        private EventSystem m_eventSystem;

        [SerializeField] private GameObject[] m_menuObjects;
        [SerializeField] private GameObject[] m_mainMenuButtons;
        [SerializeField] private GameObject[] m_optionSelect;

        private int m_lastPlayerCount = 0;
        //private bool[] m_connected = new bool[2];
        [SerializeField] private string m_stringToPassOnJoin;
        [SerializeField] private UnityEvent<string, float> m_onPlayerJoined;
        [SerializeField] private string m_stringToPassOnLeave;
        [SerializeField] private UnityEvent<string, float> m_onPlayerLeft;
        //[SerializeField] private Animator[] m_playerIndis;
        [SerializeField] private TextMeshProUGUI[] m_joinText;

        // Start is called before the first frame update
        void Awake()
        {
            Time.timeScale = 1f;

            m_effect = GetComponent<TypeWriterEffect>();
            m_eventSystem = GetComponent<EventSystem>();

            for (int i = 0; i < ControllerManager.Instance.ControllerCount && i < m_joinText.Length; i++)
           {
                m_joinText[i].text = "Player " + (i + 1) + " Has Joined";
                m_joinText[i].GetComponent<Animator>().SetTrigger("Change State");
            }

            m_lastPlayerCount = (int)ControllerManager.Instance.ControllerCount;
        }

        public void ButtonPressed(int selection)
        {
            if (selection == 0 && ControllerManager.Instance.ControllerCount < 2) return;

            foreach (GameObject obj in m_mainMenuButtons)
            {
                obj.GetComponent<Button>().interactable = false;
            }

            switch (selection)
            {
                //start game
                case 0:
                    m_effect.StartType(m_inputField, "run ILOVEYOU.exe", 12f, StartGame);
                    break;
                //options
                case 1:
                    m_effect.StartType(m_inputField, "open options.cfg", 12f, OptionsMenu);
                    break;
                //about
                case 2:
                    m_effect.StartType(m_inputField, "open README.txt", 12f, CreditsMenu);
                    break;
                //Quit
                case 3:
                    m_effect.StartType(m_inputField, "shutdown", 12f, Quit);
                    break;
                default:
                    m_effect.StartType(m_inputField, "Uhhh idk what I'm typing", 12f);
                    break;
            }

            m_eventSystem.enabled = false;

            //m_effect.StartType(m_inputField, "run program Goku.EXE", 12f, StartGame);
        }

        public void Update()
        {

            if (ControllerManager.Instance.ControllerCount > m_lastPlayerCount)
            {
                //controller added
                m_joinText[ControllerManager.Instance.MostRecentID].text = "Player " + (ControllerManager.Instance.MostRecentID + 1) + " Has Joined";
                m_joinText[ControllerManager.Instance.MostRecentID].GetComponent<Animator>().SetTrigger("Change State");
                m_onPlayerJoined.Invoke("", 0);
            }
            else if (ControllerManager.Instance.ControllerCount < m_lastPlayerCount)
            {
                //controller removed
                m_joinText[ControllerManager.Instance.MostRecentID].text = "Player " + (ControllerManager.Instance.MostRecentID + 1) + " Has Left";
                m_joinText[ControllerManager.Instance.MostRecentID].GetComponent<Animator>().SetTrigger("Change State");
                m_onPlayerLeft.Invoke("", 0);
            }
            m_lastPlayerCount = (int)ControllerManager.Instance.ControllerCount;

        }

        public void StartGame()
        {

            MainMenuAudio.Instance.Skip();

            SceneLoader.Instance.LoadScene(4);
        }

        public void MainMenu()
        {
            m_menuObjects[0].SetActive(true);
            m_menuObjects[1].SetActive(false);
            m_menuObjects[2].SetActive(false);

            foreach (GameObject obj in m_mainMenuButtons)
            {
                obj.GetComponent<Button>().interactable = true;
            }

            m_eventSystem.SetSelectedGameObject(m_optionSelect[0]);
        }

        public void OptionsMenu()
        {
            m_eventSystem.enabled = true;

            m_menuObjects[0].SetActive(false); //disable default menu
            m_menuObjects[1].SetActive(true); //enable options menu
            m_eventSystem.SetSelectedGameObject(m_optionSelect[1]);
        }

        public void CreditsMenu()
        {
            m_eventSystem.enabled = true;

            m_menuObjects[0].SetActive(false); //disable default menu
            m_menuObjects[2].SetActive(true); //enable credits menu
            m_eventSystem.SetSelectedGameObject(m_optionSelect[2]);
        }

        public void Quit()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
        Application.Quit();

#endif
        }

    }

}
using ILOVEYOU.Audio;
using ILOVEYOU.Management;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace ILOVEYOU.MainMenu
{
    public class MenuManager : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI m_inputField;
        private TypeWriterEffect m_effect;
        private MultiplayerEventSystem m_eventSystem;

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
        [SerializeField] private Button[] m_startButtons = new Button[0];

        // Start is called before the first frame update
        void Awake()
        {
            Time.timeScale = 1f;

            if(!SoundManager.Environment.IsPlaying(100)) SoundManager.Environment.PlaySoundLoop("ComputerStartUp", 1, 100);

            m_effect = GetComponent<TypeWriterEffect>();
            m_eventSystem = GetComponent<MultiplayerEventSystem>();

            for (int i = 0; i < ControllerManager.Instance.ControllerCount && i < m_joinText.Length; i++)
            {
                m_joinText[i].text = "Player " + (i + 1) + " Has Joined";
                m_joinText[i].GetComponent<Animator>().SetTrigger("Change State");
            }

            m_lastPlayerCount = (int)ControllerManager.Instance.ControllerCount;
        }

        public void ButtonPressed(int selection)
        {
            if (selection == 0 && ControllerManager.Instance.ControllerCount < 1) return;

            foreach (GameObject obj in m_mainMenuButtons)
            {
                obj.GetComponent<Button>().interactable = false;
            }

            switch (selection)
            {
                //Start game
                case 0:
                    m_effect.StartType(m_inputField, "run ILOVEYOU.exe", 12f, StartGame);
                    break;
                //Options
                case 1:
                    m_effect.StartType(m_inputField, "open options.cfg", 12f, OptionsMenu);
                    break;
                //Controls
                case 2:
                    m_effect.StartType(m_inputField, "open help.txt", 12f, ControlsMenu);
                    break;
                //Codex
                case 3:
                    m_effect.StartType(m_inputField, "open codex.txt", 12f, CodexMenu);
                    break;
                //About
                case 4:
                    m_effect.StartType(m_inputField, "open README.txt", 12f, CreditsMenu);
                    break;
                //Quit
                case 5:
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

            //MainMenuAudio.Instance.Skip();

            SoundManager.Environment.ClearAudio(true);

            SceneLoader.Instance.LoadScene(4);
        }

        public void MainMenu()
        {
            m_menuObjects[0].SetActive(true);
            m_menuObjects[1].SetActive(false);
            m_menuObjects[2].SetActive(false);
            m_menuObjects[3].SetActive(false);
            m_menuObjects[4].SetActive(false);

            foreach (GameObject obj in m_mainMenuButtons)
            {
                obj.GetComponent<Button>().interactable = true;
            }

            m_eventSystem.SetSelectedGameObject(m_optionSelect[0]);

            CheckPlayerCounts();
        }

        public void OptionsMenu()
        {
            m_eventSystem.enabled = true;

            m_menuObjects[0].SetActive(false); //disable default menu
            m_menuObjects[1].SetActive(true); //enable options menu
            m_eventSystem.SetSelectedGameObject(m_optionSelect[1]);
        }

        public void ControlsMenu()
        {
            m_eventSystem.enabled = true;

            m_menuObjects[0].SetActive(false); //disable default menu

            m_mainMenuButtons[3].GetComponent<CreatePopUpMenu>().CreatePopUp(transform);

            foreach (GameObject obj in m_mainMenuButtons)
            {
                obj.GetComponent<Button>().interactable = true;
            }
            CheckPlayerCounts();

            //m_menuObjects[2].SetActive(true); //enable options menu
            //m_eventSystem.SetSelectedGameObject(m_optionSelect[2]);
        }

        public void CodexMenu()
        {
            m_eventSystem.enabled = true;

            m_menuObjects[0].SetActive(false); //disable default menu
            m_menuObjects[4].SetActive(true); //enable options menu
            m_eventSystem.SetSelectedGameObject(m_optionSelect[3]);
        }

        public void CreditsMenu()
        {
            m_eventSystem.enabled = true;

            m_menuObjects[0].SetActive(false); //disable default menu
            m_menuObjects[5].SetActive(true); //enable credits menu
            m_eventSystem.SetSelectedGameObject(m_optionSelect[4]);
        }
        /// <summary>
        /// plays sound
        /// </summary>
        public void PlaySound(string soundName)
        {
            SoundManager.UI.PlayRandomSound(soundName);
        }

        public void CheckPlayerCounts()
        {
            m_startButtons[0].interactable = false;
            m_startButtons[1].interactable = false;
            if(ControllerManager.Instance.ControllerCount > 0)
            {
                m_startButtons[1].interactable = true;
                if(ControllerManager.Instance.ControllerCount > 1)
                {
                    m_startButtons[0].interactable = true;
                }
            }
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
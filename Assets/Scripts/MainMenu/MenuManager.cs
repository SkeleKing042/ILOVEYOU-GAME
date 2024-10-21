using TMPro;
using UnityEditor;
using UnityEngine;
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



        //[SerializeField] GameObject SHOWTHING;

        // Start is called before the first frame update
        void Start()
        {
            m_effect = GetComponent<TypeWriterEffect>();
            m_eventSystem = GetComponent<EventSystem>();
            //m_eventSystem.currentSelectedGameObject.GetComponent<ButtonSelect>().Enabled = true;
        }

        public void ButtonPressed(int selection)
        {
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

        public void StartGame()
        {
            //scene change here
            Debug.Log("Game started");

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
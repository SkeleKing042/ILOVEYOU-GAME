using ILOVEYOU.Management;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ILOVEYOU.UI
{
    public class CustomSettingsMenu : MonoBehaviour
    {
        private bool m_firstLoad {get { return CustomSettingsManager.SetList.Count == 0 ? true : false;} }
        [SerializeField] private Button m_optionTemplate;
        [SerializeField] private RectTransform m_contentBox;
        [SerializeField] private TMP_Text m_currentDisplay;
        //height of viewport is 666
        public void Awake()
        {
            DisplayCustomSettings();
        }
        public void ResetSettings(){
            GameSettings.Unassign();
        }
        public void DisplayCustomSettings(){
            if(m_firstLoad){
                CustomSettingsManager.GetSettings();
            }
    
            //Create buttons for each option
            foreach(var set in CustomSettingsManager.SetList){
                //Create new button instance
                Button butInst = Instantiate(m_optionTemplate);
                //Set the button text to the setting name
                string count = set.GetPlayerLimit == 0 ? "N/A" : set.GetPlayerLimit.ToString();
                butInst.GetComponentInChildren<TextMeshProUGUI>().text = $"{set.name}\nPlayer Limit = {count}";
                butInst.name = $"{set.name} setting button";
                if (set.IsOutdated) butInst.GetComponentInChildren<TextMeshProUGUI>().color = Color.magenta;
                //Have the button assign the setting on selection.
                butInst.onClick.AddListener(() => set.Assign(true));
                //Move the new instance to the context box
                butInst.transform.SetParent(m_contentBox);
            }
        }
        public void Update()
        {
            string s = $"Current settings: ";
            if (GameSettings.Current != null)
                s += GameSettings.Current.name;
            else
                s += "Default";

            if(s != m_currentDisplay.text)
            {
                m_currentDisplay.text = s;
                StartCoroutine(BlindText(m_currentDisplay.gameObject, 3, 0.1f));
            }
        }
        private IEnumerator BlindText(GameObject toBlink, float blinkAmount, float delay)
        {
            for(int i = 0; i < blinkAmount; i++)
            {
                toBlink.SetActive(false);
                yield return new WaitForSeconds(delay);
                toBlink.SetActive(true);
                yield return new WaitForSeconds(delay);
            }
        }
    }
}
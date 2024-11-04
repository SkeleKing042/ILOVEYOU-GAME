using System.Collections;
using TMPro;
using UnityEngine;
using ILOVEYOU.Audio;
using Random = UnityEngine.Random;

namespace ILOVEYOU.MainMenu
{
    public class TypeWriterEffect : MonoBehaviour
    {
        //[SerializeField] private TextMeshProUGUI Test;
        public delegate void FunctionToActivate();
        private FunctionToActivate m_activate;
        //public void AddFunction(FunctionToActivate function) { m_activate = function; }
        //public void RemoveFunction(FunctionToActivate function) { m_activate -= function; }
        //private SoundManager m_soundManager;

        // Start is called before the first frame update
        void Awake()
        {
            //m_soundManager = GetComponent<SoundManager>();
            //Application.targetFrameRate = 60;
            //helloooo
        }

        public void StartType(TextMeshProUGUI textObject, string textToType, float textSpeed)
        {
            StartCoroutine(Type(textObject, textToType, textSpeed));
        }

        public void StartType(TextMeshProUGUI textObject, string textToType, float textSpeed, FunctionToActivate function)
        {
            StartCoroutine(Type(textObject, textToType, textSpeed, function));
        }

        private IEnumerator Type(TextMeshProUGUI textObject, string textToType, float textSpeed)
        {
            float time = 0;
            //disables cursor if text object has one
            textObject.GetComponent<CursorEffect>().DisableCursor();
            //clears text and sets values
            textObject.text = "";
            string textBuffer = textToType;
            int currentChar = 0;

            float randomVariance = 0f;

            while (currentChar < textToType.Length)
            {
                time += (Time.deltaTime * textSpeed) + (randomVariance * Time.deltaTime);

                //keep it like this for now
                randomVariance = Random.Range(-textSpeed, textSpeed);

                if (time > currentChar)
                {
                    for (float i = currentChar; i < time; i++)
                    {

                        if (currentChar >= textToType.Length) break;

                        textObject.text += textBuffer.ToCharArray()[0];
                        //Remove that character from the original log
                        textBuffer = textBuffer.Remove(0, 1);
                        currentChar++;
                    }

                    int groupNumber = (textObject.text.ToCharArray()[textObject.text.Length - 1] == ' ') ? 2 : 0;

                    SoundManager.UI.PlayRandomSound(groupNumber);
                }

                yield return new WaitForEndOfFrame();
            }

            textObject.GetComponent<CursorEffect>().EnableCursor();

            yield return null;
        }

        private IEnumerator Type(TextMeshProUGUI textObject, string textToType, float textSpeed, FunctionToActivate function)
        {
            float time = 0;
            //disables cursor if text object has one
            textObject.GetComponent<CursorEffect>().DisableCursor();
            //sets function to activate to the one that has been inputted
            m_activate = function;
            //clears text and sets values
            textObject.text = "";
            string textBuffer = textToType;
            int currentChar = 0;

            float randomVariance = 0f;

            while (currentChar < textToType.Length)
            {
                time += (Time.deltaTime * textSpeed) + (randomVariance * Time.deltaTime);

                //keep it like this for now
                randomVariance = Random.Range(-textSpeed, textSpeed);

                if (time > currentChar)
                {
                    for (float i = currentChar; i < time; i++)
                    {

                        if (currentChar >= textToType.Length) break;

                        textObject.text += textBuffer.ToCharArray()[0];
                        //Remove that character from the original log
                        textBuffer = textBuffer.Remove(0, 1);
                        currentChar++;
                    }
                    //checks if next key is a space and chooeses sound accordingly
                    string group = (textObject.text.ToCharArray()[textObject.text.Length - 1] == ' ') ? "KeyboardSpace" : "KeyboardSelect";

                    SoundManager.UI.PlayRandomSound(group);
                }

                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(Random.Range(0.25f, 0.5f));

            SoundManager.UI.PlayRandomSound("KeyboardEnter");
            textObject.text = "";

            yield return new WaitForSeconds(Random.Range(0f, 0.1f));

            m_activate.Invoke();

            

            textObject.GetComponent<CursorEffect>().EnableCursor();

            yield return null;
        }
    }

}

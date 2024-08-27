using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace ILOVEYOU
{
    namespace Player
    {

        public class EventLogUI : MonoBehaviour
        {
            [SerializeField] private Transform m_logBox;
            [SerializeField] private TextMeshProUGUI m_textTemplate;
            [SerializeField] private float m_textDuration;
            [SerializeField] private bool m_durationIncludesTyping;
            [SerializeField] private float m_displayDelay;

            public void LogInput(string text)
            {
                //Create a new text box & assign it to the parent display box
                TextMeshProUGUI textBox = Instantiate(m_textTemplate);
                textBox.transform.SetParent(m_logBox, false);

                StartCoroutine(TypeBox(textBox, text));
            }

            /// <summary>
            /// Removes text character by character starting at the end
            /// </summary>
            /// <param name="box"></param>
            /// <returns></returns>
            private IEnumerator TypeBox(TextMeshProUGUI box, string text)
            {
                //while there is text to be typed
                while (text.Length > 0)
                {
                    //rich text check
                    if (text.ToCharArray()[0] == '<')
                    {
                        while (true)
                        {
                            //end of rich text
                            if (text.ToCharArray()[0] == '>')
                            {
                                break;
                            }

                            //add text as normal
                            box.text += text.ToCharArray()[0];
                            text = text.Remove(0, 1);
                        }
                    }

                    //add a character to the box
                    box.text += text.ToCharArray()[0];
                    //remove a character from the text
                    text = text.Remove(0, 1);
                    //wait to type the next one
                    yield return new WaitForSecondsRealtime(m_displayDelay);
                }

                //Wait before deleting the text
                float time = m_textDuration;
                if (m_durationIncludesTyping)
                {
                    time -= m_displayDelay * box.text.Length * 2;
                }
                yield return new WaitForSecondsRealtime(time);

                //While there is text in the box
                while (box.text.Length > 0)
                {
                    //rich text check
                    if (box.text[box.text.Length - 1] == '>')
                    {
                        while (true)
                        {
                            //end of rich text
                            if (box.text[box.text.Length - 1] == '<')
                            {
                                break;
                            }

                            //remove text as normal
                            box.text = box.text.Remove(box.text.Length - 1);
                        }
                    }
                    //Remove that character
                    box.text = box.text.Remove(box.text.Length - 1);
                    //Wait to remove the next one
                    yield return new WaitForSecondsRealtime(m_displayDelay);
                }

                //All characters have been removed, time is added to stop text dropping box down in layout groups.
                Destroy(box.gameObject, 1.0f);
            }
        }
    }
}
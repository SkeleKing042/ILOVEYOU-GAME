using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventLogUI : MonoBehaviour
{
    private List<string> m_buffer = new();
    [SerializeField] private Transform m_logBox;
    private List<TextMeshProUGUI> m_unfinishedBoxes = new();
    [SerializeField] private TextMeshProUGUI m_textTemplate;
    [SerializeField] private float m_textDuration;
    [SerializeField] private float m_displayDelay;
    [SerializeField] private float m_blinkerSpeed;
    private float m_timer;
    
    public void LogInput(string text)
    {
        //Create a new text box & assign it to the parent display box
        TextMeshProUGUI textBox = Instantiate(m_textTemplate);
        textBox.transform.SetParent(m_logBox, false);

        //The text needs to be added so the box is unfinished.
        m_unfinishedBoxes.Add(textBox);
        m_buffer.Add(text);
    }
    private void Update()
    {
        //If there is text to display and the timer up
        if (m_buffer.Count > 0 && m_timer <= 0)
        {
            //Iterate though each letter...
            for (int i = 0; i < m_buffer.Count; i++)
            {
                //Add the first character in the requested log to the text box
                m_unfinishedBoxes[i].text += m_buffer[i].ToCharArray()[0];
                //Remove that character from the original log
                m_buffer[i] = m_buffer[i].Remove(0 , 1);
                //If that log is empty...
                if (m_buffer[i].Length <= 0)
                {
                    //Get ready to remove the text
                    StartCoroutine(ClearText(m_unfinishedBoxes[i]));
                    //The box is finished and the log is empty, so remove them from their lists
                    m_unfinishedBoxes.RemoveAt(i);
                    m_buffer.RemoveAt(i);
                }
            }
            //Reset timer
            m_timer = m_displayDelay;
        }
        //Timer tick down
        if (m_timer > 0)
            m_timer -= Time.deltaTime;
    }
    /// <summary>
    /// Removes text character by character starting at the end
    /// </summary>
    /// <param name="box"></param>
    /// <returns></returns>
    private IEnumerator ClearText(TextMeshProUGUI box)
    {
        //Wait
        yield return new WaitForSecondsRealtime(m_textDuration);
        //Iterate though each character in the text box
        for(int i = box.text.Length - 1; i >= 0; i--)
        {
            //Remove that character
            box.text = box.text.Remove(i);
            //Wait
            yield return new WaitForSecondsRealtime(m_displayDelay);
        }
        //All characters have been removed, time is added to stop text dropping box down in layout groups.
        Destroy(box, 1.0f);
    }
}

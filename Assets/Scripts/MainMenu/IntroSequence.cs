using ILOVEYOU.Audio;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ILOVEYOU.MainMenu
{
    public class IntroSequence : MonoBehaviour
    {
        [SerializeField] private GameObject[] m_sequenceObjects;


        private float m_time;
        private float m_timeMulti;

        // Start is called before the first frame update
        void Start()
        {
            m_time = 0f;
            m_timeMulti = 1f;
            StartCoroutine(_Sequence());
            _Audio();
            //StartCoroutine(MainMenuAudio.Instance.AudioSequence());
        }

        private void Update()
        {
            m_time += Time.deltaTime * m_timeMulti;

            if (Input.anyKeyDown)
            {
                StopAllCoroutines();
                //MainMenuAudio.Instance.Skip();
                SoundManager.Environment.ClearAudio(true);
                SceneManager.LoadScene(3); //MAKE SURE TO CHANGE THIS IF EDITING THE SCENE BUILD LAYOUT!!!!!
            }

        }

        private void _Audio()
        {
            SoundManager.Environment.PlaySound("ComputerStartUp", 0);

            SoundManager.Environment.PlaySoundLoopDelay("ComputerStartUp", 1, 100, 15.15f); // plays the computer loop with an ID of 100
        }

        /// <summary>
        /// could I have done this in the animator? probably. Do I wanna? Nuh.
        /// </summary>
        private IEnumerator _Sequence()
        {
            //part one
            yield return new WaitForSeconds(Random.Range(2f, 3f));
            m_sequenceObjects[0].SetActive(true);
            yield return new WaitForSeconds(Random.Range(1f, 2f));
            m_sequenceObjects[1].SetActive(true);

            m_sequenceObjects[1].GetComponent<TextMeshProUGUI>().text = "Version JE44\r\n\r\nSUPER XTH-AWO CPU at 2MHz";
            yield return new WaitForSeconds(Random.Range(0f, 0.4f));

            m_time = 0f;
            m_timeMulti = 0.6f;

            while (m_time < 1f)
            {
                m_sequenceObjects[1].GetComponent<TextMeshProUGUI>().text = "Version JE44\r\n\r\nSUPER XTH-AWO CPU at 2MHz\r\nMemory Test: " + m_time.ToString("f4") + "K";

                yield return new WaitForSeconds(0.15f);
            }

            m_sequenceObjects[1].GetComponent<TextMeshProUGUI>().text = "Version JE44\r\n\r\nSUPER XTH-AWO CPU at 2MHz\r\nMemory Test: 1.0000K OK";

            yield return new WaitForSeconds(Random.Range(0f, 0.4f));

            m_sequenceObjects[2].SetActive(true);
            m_sequenceObjects[2].GetComponent<TextMeshProUGUI>().text = "Keyboard:";
            yield return new WaitForSeconds(Random.Range(0.3f, 0.5f));
            m_sequenceObjects[2].GetComponent<TextMeshProUGUI>().text = "Keyboard: OK\r\nMonitor:";
            yield return new WaitForSeconds(Random.Range(0.3f, 0.5f));
            m_sequenceObjects[2].GetComponent<TextMeshProUGUI>().text = "Keyboard: OK\r\nMonitor: OK\r\nCD Rom:";
            yield return new WaitForSeconds(Random.Range(0.3f, 0.5f));
            m_sequenceObjects[2].GetComponent<TextMeshProUGUI>().text = "Keyboard: OK\r\nMonitor: OK\r\nCD Rom: OK\r\nTechnomantic Gnomes: ";
            yield return new WaitForSeconds(Random.Range(0f, 0.3f));
            m_sequenceObjects[2].GetComponent<TextMeshProUGUI>().text = "Keyboard: OK\r\nMonitor: OK\r\nCD Rom: OK\r\nTechnomantic Gnomes: OK\r\nFlimple Drive:";
            yield return new WaitForSeconds(Random.Range(0f, 0.3f));
            m_sequenceObjects[2].GetComponent<TextMeshProUGUI>().text = "Keyboard: OK\r\nMonitor: OK\r\nCD Rom: OK\r\nTechnomantic Gnomes: OK\r\nFlimple Drive: OK\r\nGlognog: ";
            yield return new WaitForSeconds(Random.Range(0f, 0.3f));
            m_sequenceObjects[2].GetComponent<TextMeshProUGUI>().text = "Keyboard: OK\r\nMonitor: OK\r\nCD Rom: OK\r\nTechnomantic Gnomes: OK\r\nFlimple Drive: OK\r\nGlognog: OK\r\nCoconut JPEG:";
            yield return new WaitForSeconds(Random.Range(0f, 0.3f));
            m_sequenceObjects[2].GetComponent<TextMeshProUGUI>().text = "Keyboard: OK\r\nMonitor: OK\r\nCD Rom: OK\r\nTechnomantic Gnomes: OK\r\nFlimple Drive: OK\r\nGlognog: OK\r\nCoconut JPEG: OK";
            yield return new WaitForSeconds(0.2f);
            m_sequenceObjects[2].GetComponent<TextMeshProUGUI>().text = "Keyboard: OK\r\nMonitor: OK\r\nCD Rom: OK\r\nTechnomantic Gnomes: OK\r\nFlimple Drive: OK\r\nGlognog: OK\r\nCoconut JPEG: OK\r\n\r\n...All Good";
            SoundManager.Environment.PlaySound("ComputerStartUp",2); //Beep
            yield return new WaitForSeconds(0.3f);

            m_sequenceObjects[0].SetActive(false);
            m_sequenceObjects[1].SetActive(false);
            m_sequenceObjects[2].SetActive(false);

            yield return new WaitForSeconds(Random.Range(0.4f, 0.7f));

            m_sequenceObjects[3].SetActive(true);
            yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));
            m_sequenceObjects[4].SetActive(true);
            yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));
            m_sequenceObjects[5].SetActive(true);

            m_time = 0f;
            m_timeMulti = 12f;

            while (m_time < 75f)
            {
                yield return new WaitForSeconds(Random.Range(0, 0.5f));

                m_sequenceObjects[5].GetComponent<Slider>().value = Mathf.RoundToInt(m_time);
                m_sequenceObjects[6].GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt((m_time / 75f) * 100f).ToString() + "%";
            }

            m_sequenceObjects[5].GetComponent<Slider>().value = 75;
            m_sequenceObjects[6].GetComponent<TextMeshProUGUI>().text = "100%";

            yield return new WaitForSeconds(Random.Range(0, 0.3f));

            while (!SoundManager.Environment.IsPlaying(100))
            {
                yield return new WaitForEndOfFrame();
            }

            SceneManager.LoadSceneAsync(3); //MAKE SURE TO CHANGE THIS IF EDITING THE SCENE BUILD LAYOUT!!!!!
        }

    }

}


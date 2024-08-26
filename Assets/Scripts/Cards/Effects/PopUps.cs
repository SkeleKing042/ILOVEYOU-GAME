using ILOVEYOU.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILOVEYOU
{
    namespace Cards
    {
        public class PopUps : MonoBehaviour
        {
            [SerializeField] GameObject[] m_popUpPrefabs;
            private List<GameObject> m_currentPopups;
            private PlayerControls m_playerControls;
            private Vector2 m_canvasDimensions;

            private void Update()
            {
                //this is to ensure that the player can close the windows again just in case it gets overriden
                m_playerControls.SetContext(WindowClosed, 2, "Press A to close Popups!");
            }
            /// <summary>
            /// sets up variables in order for this script to work
            /// </summary>
            /// <param name="controls">player controls for the script to access</param>
            public void Initialize(PlayerControls controls)
            {
                m_playerControls = controls;
                m_currentPopups = new();
                m_canvasDimensions = new(GetComponent<RectTransform>().rect.width, GetComponent<RectTransform>().rect.height);
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="count"></param>
            public void StartPopUps(int count)
            {
                //creates popups with count
                for (int i = 0; i < count; i++)
                {
                    //creates instance from prefab list
                    GameObject popup = Instantiate(m_popUpPrefabs[Random.Range(0, m_popUpPrefabs.Length)], gameObject.transform);
                    //randomises position
                    popup.transform.localPosition = new(Random.Range(-m_canvasDimensions.x / 2, m_canvasDimensions.x / 2), Random.Range(-m_canvasDimensions.y / 2, m_canvasDimensions.y / 2));
                    //add gameobject to list for future reference
                    m_currentPopups.Add(popup);

                }
                //sets the context button to WindowClosed() to allow for closing of the window
                m_playerControls.SetContext(WindowClosed, 2, "Press A to close Popups!");
            }
            /// <summary>
            /// Destroys and resets required variables. Disables gameobject
            /// </summary>
            public void EndPopups()
            {
                //destroys all popups just in case
                for(int i = 0; i < m_currentPopups.Count; i++)
                {
                    GameObject objToRemove = m_currentPopups[^1];
                    m_currentPopups.Remove(objToRemove);
                    Destroy(objToRemove);
                }
                //removes context
                m_playerControls.RemoveSetContext(WindowClosed);
                //disables object
                gameObject.SetActive(false);
            }
            /// <summary>
            /// removes a window in the list
            /// </summary>
            public void WindowClosed()
            {
                GameObject objToRemove = m_currentPopups[^1];
                m_currentPopups.Remove(objToRemove);
                Destroy(objToRemove);
                //if all windows are destroyed end this script
                if (m_currentPopups.Count == 0) EndPopups();

            }
        }
    }
}

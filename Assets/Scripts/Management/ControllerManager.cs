using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UI.Image;

namespace ILOVEYOU
{
    namespace Management
    {

        public class ControllerManager : MonoBehaviour
        {
            bool m_ignoreJoin = false;
            [SerializeField] private uint m_maxControllers;
            private Controller[] m_controllers;
            public uint ControllerCount => (uint)transform.childCount;
            [SerializeField] private GameObject m_playerPrefab;
            public static ControllerManager Instance { get; private set; }
            private void Awake()
            {
                m_controllers = new Controller[m_maxControllers];
                DontDestroyOnLoad(this);
                Instance = this;
            }
            //called on controller input
            public void OnPlayerJoined(PlayerInput input)
            {
                if (!m_ignoreJoin)
                {
                    //made child so not destroyed on scene change
                    input.transform.SetParent(transform);

                    //save controller to array
                    for (uint i = 0; i < m_controllers.Length; i++)
                    {
                        if (m_controllers[i] == null)
                        {
                            m_controllers[i] = input.GetComponent<Controller>();
                            m_controllers[i].ID = i;
                            break;
                        }
                    }
                }
            }
            //instances player objects - for scene start
            public GameObject[] JoinPlayers()
            {
                m_ignoreJoin = true;
                GameObject[] players = new GameObject[transform.childCount];
                for (int i = 0; i < transform.childCount; i++)
                {
                    Controller current = transform.GetChild(i).GetComponent<Controller>();
                    if (!current.IsAssigned)
                    {
                        players[i] = PlayerInput.Instantiate(m_playerPrefab, default, pairWithDevices: current.GetDevice).gameObject;
                        current.AssignObject(players[i]);
                    }
                }
                m_ignoreJoin = false;
                return players;
            }
        }
    }
}
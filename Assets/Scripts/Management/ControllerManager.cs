using ILOVEYOU.Player;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            public bool doJoinLeave = true;
            [SerializeField] private uint m_maxControllers;
            [SerializeField] private List<Controller> m_controllers = new();
            [SerializeField] private Material[] m_playerMaterials;
            public List<Controller> GetControllers => m_controllers;
            public uint ControllerCount => (uint)transform.childCount;
            public uint NumberOfActivePlayers { get
                {
                    uint count = 0;
                    foreach(var controller in m_controllers)
                        if (controller.IsAssigned)
                            count++;
                    return count;
                } }
            private uint m_recentID;
            public uint MostRecentID => m_recentID;
            [SerializeField] private GameObject m_playerPrefab;
            public static ControllerManager Instance { get; private set; }
            private void Awake()
            {
                //m_controllers = new Controller[m_maxControllers];
                DontDestroyOnLoad(this);
                Instance = this;
            }
            //called on controller input
            public void OnPlayerJoined(PlayerInput input)
            {
                if (doJoinLeave)
                {
                    Debug.Log($"Player joined with {input.devices[0]}.");
                    //made child so not destroyed on scene change
                    input.transform.SetParent(transform);

                    if (m_controllers.Count == 0)
                    {
                        _setID(0, input);
                        return;
                    }
                    //go through each id
                    for (int i = 0; i < m_controllers.Count; i++)
                    {
                        //compare id
                        if (m_controllers[i].ID > i)
                        {
                            _setID(i, input);
                            return;
                        }
                    }
                    //this input is added to the end of the list
                    _setID(m_controllers.Count, input);
                    return;
                }
            }
            private void _setID(int index, PlayerInput input)
            {
                input.transform.SetSiblingIndex(index);
                m_controllers.Insert(index, input.GetComponent<Controller>());
                m_controllers[index].name = $"{input.currentControlScheme} - {index}";
                m_controllers[index].ID = (uint)index;
                m_recentID = (uint)index;

            }
            //instances player objects - for scene start
            public GameObject[] JoinPlayers(int playerCount = 0)
            {
                Debug.Log("Instancing players...");
                doJoinLeave = false;
                GameObject[] players = new GameObject[Mathf.Clamp(transform.childCount, 0, playerCount == 0 ? transform.childCount : playerCount)];
                for (int i = 0; i < Mathf.Clamp(transform.childCount, 0, playerCount == 0 ? transform.childCount : playerCount); i++)
                {
                    Controller current = transform.GetChild(i).GetComponent<Controller>();
                    if (!current.IsAssigned)
                    {
                        players[i] = PlayerInput.Instantiate(m_playerPrefab, default, pairWithDevices: current.GetDevice).gameObject;
                        current.AssignObject(players[i]);

                        players[i].GetComponent<PlayerControls>().SetMaterial(new Material[2] { m_playerMaterials[i * 2], m_playerMaterials[i * 2 + 1] });

                    }
                }
                doJoinLeave = true;
                Debug.Log("Players instanced...");
                return players;
            }
            //a player has left
            public void PlayerLeft(Controller caller)
            {
                int index = m_controllers.IndexOf(caller);
                m_recentID = caller.ID;
                Destroy(caller.gameObject);
                m_controllers.RemoveAt(index);
            }

            public static void ToggleJoinLeave(bool value)
            {
                Instance.doJoinLeave = value;
            }
        }
    }
}
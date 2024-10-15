using ILOVEYOU.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ILOVEYOU
{
    using Management;
    namespace Cards
    {
        public class DisruptCard : MonoBehaviour
        {
            [SerializeField] private Color m_color;
            [SerializeField] private bool m_effectSelf;
            public void Trigger(GameManager manager, PlayerManager player)
            {
                //This function is called by a button on click event, a script with this function should be attached to the same gameobject as this script
                SendMessage("ExecuteEvents", new object[] { manager, player });
                switch (m_effectSelf)
                {
                    case true:
                        player.GetUI.GetCardVin.FlashIn(m_color);
                        break;
                    case false:
                        foreach(PlayerManager others in GameManager.Instance.GetOtherPlayers(player))
                        {
                            others.GetUI.GetCardVin.FlashIn(m_color);
                        }
                        break;
                }
            }
        }
    }
}
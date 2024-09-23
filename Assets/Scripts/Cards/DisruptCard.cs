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
            public void Trigger(GameManager manager, PlayerManager player)
            {
                //This function is called by a button on click event, a script with this function should be attached to the same gameobject as this script
                SendMessage("ExecuteEvents", new object[] { manager, player });
            }
        }
    }
}
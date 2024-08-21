using ILOVEYOU.Hazards;
using ILOVEYOU.Management;
using ILOVEYOU.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILOVEYOU
{
    namespace Cards
    {

        public class HazardCard : MonoBehaviour
        {
            [Tooltip("How long the effect will last for. 0 seconds will enable the hazard without it turning off.")] [SerializeField] private float m_time;
            [Tooltip("What hazards will be enabled when this card is selected. If none is inputted, activate all hazards instead.")][SerializeField] private HazardTypes[] m_hazardType;
            public void ExecuteEvents(object[] data)
            {
                PlayerManager player = (PlayerManager)data[1];
                GameManager gm = (GameManager)data[0];


                //if there aren't any types assigned to the card, activate all hazards
                if (m_hazardType.Length == 0)
                {
                    //if time is inputted, activate hazards for set amount of time
                    if(m_time > 0) gm.GetOtherPlayer(player).GetLevelManager.GetComponent<HazardManager>().EnableAllHazards(m_time);
                    //else toggle all hazards on
                    else gm.GetOtherPlayer(player).GetLevelManager.GetComponent<HazardManager>().EnableAllHazards();
                }
                else
                {
                    //if time is inputted, activate hazards for set amount of time
                    if (m_time > 0)
                    {
                        foreach (HazardTypes type in m_hazardType)
                        {
                            gm.GetOtherPlayer(player).GetLevelManager.GetComponent<HazardManager>().EnableTypeHazards(type, m_time);
                        }
                    }
                    //else toggle all hazards on
                    else
                    {
                        foreach (HazardTypes type in m_hazardType)
                        {
                            gm.GetOtherPlayer(player).GetLevelManager.GetComponent<HazardManager>().EnableTypeHazards(type);
                        }
                    }
                }
                        
            }
        }
    }
}
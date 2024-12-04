using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILOVEYOU.Management
{
    public class CreatePopUpMenu : MonoBehaviour
    {
        [SerializeField] private GameObject m_popUp;
        /// <summary>
        /// Creates popup, by default it goes to the first canvas it finds
        /// </summary>
        public void CreatePopUp()
        {
            if (GetComponentInParent<Canvas>())
            {
                Instantiate(m_popUp, GetComponentInParent<Canvas>().gameObject.transform).GetComponent<MenuPopUp>().SetReturn(gameObject);
            }
        }

        /// <summary>
        /// Creates popup
        /// </summary>
        public MenuPopUp CreatePopUp(Transform parent)
        {
            MenuPopUp popup = Instantiate(m_popUp, parent).GetComponent<MenuPopUp>();
            popup.SetReturn(gameObject);
            return popup;
        }
    }


}


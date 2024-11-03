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
            Instantiate(m_popUp, FindObjectOfType<Canvas>().gameObject.transform).GetComponent<MenuPopUp>().SetReturn(gameObject);
        }

        /// <summary>
        /// Creates popup
        /// </summary>
        public void CreatePopUp(Transform parent)
        {
            Instantiate(m_popUp, parent).GetComponent<MenuPopUp>().SetReturn(gameObject);
        }
    }


}


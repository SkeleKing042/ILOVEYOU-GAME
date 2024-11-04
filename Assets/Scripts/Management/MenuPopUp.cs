using UnityEngine;
using UnityEngine.EventSystems;

namespace ILOVEYOU.Management 
{
    public class MenuPopUp : MonoBehaviour
    {
        [SerializeField] private GameObject m_firstSelected;
        private GameObject m_returnObject;

        // Start is called before the first frame update
        void Start()
        {
            transform.GetChild(2).gameObject.SetActive(true);
            FindObjectOfType<EventSystem>().SetSelectedGameObject(m_firstSelected);
        }

        public void Return()
        {
            if (m_returnObject) FindObjectOfType<EventSystem>().SetSelectedGameObject(m_returnObject);

            Destroy(gameObject);
        }

        /// <summary>
        /// sets return object for when the popup is closed
        /// </summary>
        public void SetReturn(GameObject returnObj)
        {
            m_returnObject = returnObj;
        }
    }
}


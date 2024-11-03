using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ILOVEYOU.MainMenu
{
    public class SliderSelect : MonoBehaviour, ISubmitHandler
    {
        [SerializeField] private UnityEvent m_onSubmit;

        public void OnSubmit(BaseEventData eventData)
        {
            m_onSubmit.Invoke();
        }
    }
}



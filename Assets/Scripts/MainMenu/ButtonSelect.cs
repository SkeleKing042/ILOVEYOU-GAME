using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ILOVEYOU.MainMenu
{
    public class ButtonSelect : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [SerializeField] private UnityEvent m_onSelect;
        [SerializeField] private UnityEvent m_onDeselect;

        [SerializeField] private bool m_makeSound = true;
        public bool MakeSound { get { return m_makeSound; } set { m_makeSound = value; } }

        public void OnDeselect(BaseEventData eventData)
        {
            if (MakeSound) m_onDeselect.Invoke();
        }

        public void OnSelect(BaseEventData eventData)
        {
            if (MakeSound) m_onSelect.Invoke();
            else MakeSound = true;
        }

    }
}



using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SliderSelect : MonoBehaviour, ISubmitHandler
{
    [SerializeField] private UnityEvent m_onSubmit;

    public void OnSubmit(BaseEventData eventData)
    {
        m_onSubmit.Invoke();
    }
}

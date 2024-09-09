using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuffSystem : MonoBehaviour
{
    [Serializable]
    public class BuffData
    {
        [SerializeField] private int m_buffType = 0;
        [SerializeField] private bool m_isPermanent = false;
        [SerializeField] private float m_time = 1f;
        [SerializeField] private UnityEvent m_OnActivate;

        public int GetBuffType { get { return m_buffType; } }
        public float GetTime { get {return m_time; } }


        public void SetBuffType(int type) => m_buffType = type;
        public void SetTime(float time) => m_time = time;
    }

    [SerializeField] private BuffData[] m_buffData = new BuffData[2];

    public BuffData[] GetData { get { return m_buffData; } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

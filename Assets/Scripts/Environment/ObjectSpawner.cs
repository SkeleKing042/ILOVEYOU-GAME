using ILOVEYOU.Hazards;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ILOVEYOU.Environment
{
    public class ObjectSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] m_objectList;
        private int m_index;
        public enum SpawnStyle
        {
            Single,
            RoundRobin,
            Random,
            RandomCount,
            All,
        }
        [SerializeField] private SpawnStyle m_spawnMethod;
        [SerializeField] private float m_timeBetweenSpawns;
        private float m_countdown;
        [SerializeField] private float m_objectLifetime = 1;

        public bool Initialize(GameObject[] objs, SpawnStyle style, float time, float lifetime, int index = 0)
        {
            m_objectList = objs;
            m_spawnMethod = style;
            m_timeBetweenSpawns = time;
            m_countdown = m_timeBetweenSpawns;
            m_objectLifetime = lifetime;
            m_index = index;
            return true;
        }

        // Update is called once per frame
        void Update()
        {
            if(m_countdown <= 0)
            {
                m_countdown = m_timeBetweenSpawns;
                _spawnObjects();
            }
            else
            {
                m_countdown -= Time.deltaTime;
            }
        }
        private void _spawnObjects()
        {
            switch (m_spawnMethod)
            {
                case SpawnStyle.Single:
                    {
                        GameObject newObject = Instantiate(m_objectList[m_index]);
                        newObject.transform.position = transform.position;
                        _hazardCheck(newObject);
                        Destroy(newObject, m_objectLifetime);
                    }
                    break;
                case SpawnStyle.RoundRobin:
                    {
                        GameObject newObject = Instantiate(m_objectList[m_index]);
                        newObject.transform.position = transform.position;
                        _hazardCheck(newObject);
                        Destroy(newObject, m_objectLifetime);

                        m_index++;
                        if(m_index >= m_objectList.Length)
                            m_index = 0;
                    }
                    break;
                case SpawnStyle.Random:
                    {
                        int rnd = Random.Range(0, m_objectList.Length);
                        GameObject newObject = Instantiate(m_objectList[rnd]);
                        newObject.transform.position = transform.position;
                        _hazardCheck(newObject);
                        Destroy(newObject, m_objectLifetime);
                    }
                    break;
                case SpawnStyle.RandomCount:
                    for(int i = 0; i < m_index; i++)
                    {
                        int rnd = Random.Range(0, m_objectList.Length);
                        GameObject newObject = Instantiate(m_objectList[rnd]);
                        newObject.transform.position = transform.position;
                        _hazardCheck(newObject);
                        Destroy(newObject, m_objectLifetime);
                    }
                    break;
                case SpawnStyle.All:
                    foreach(var obj in m_objectList)
                    {
                        GameObject newObject = Instantiate(obj);
                        newObject.transform.position = transform.position;
                        _hazardCheck(newObject);
                        Destroy(newObject, m_objectLifetime);
                    }
                    break;
            }
        }
        private void _hazardCheck(GameObject obj)
        {
            HazardObject hazard = obj.GetComponent<HazardObject>();
            if (hazard)
            {
                hazard.EnableHazard(m_objectLifetime);
            }
        }
    }
}
using ILOVEYOU.EnemySystem;
using ILOVEYOU.Management;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ILOVEYOU.Player
{
    [RequireComponent(typeof(Collider))]
    public class BoomBox : MonoBehaviour
    {
        [SerializeField] private float m_radius;
        private bool m_triggerCheck;
        private void OnTriggerEnter(Collider other)
        {
            Enemy e = other.GetComponent<Enemy>();
            if (e)
            {
                e.GetStunned();
                Vector3 direction = (other.transform.position - transform.position).normalized;
                other.GetComponent<Rigidbody>().AddForceAtPosition(new Vector3(GameSettings.Current.GetKnockbackStrength.x * direction.x, GameSettings.Current.GetKnockbackStrength.y, GameSettings.Current.GetKnockbackStrength.x * direction.z), transform.position, ForceMode.Impulse);
            }
        }
        private void Awake()
        {
            GetComponent<SphereCollider>().radius = GameSettings.Current.GetKnockbackRadius;
        }
        private void Update()
        {
            if (gameObject.activeSelf && !m_triggerCheck)
            {
                m_triggerCheck = true;
                Invoke("DisableBB", 0.1f);
            }
        }
        private void DisableBB()
        {
            gameObject.SetActive(false);
            m_triggerCheck = false;
        }
    }
}
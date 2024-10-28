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
                other.GetComponent<Rigidbody>().AddExplosionForce(GameSettings.Current.GetKnockbackStrength.x, transform.position, m_radius);
                other.GetComponent<Rigidbody>().AddForce(new Vector3(0, GameSettings.Current.GetKnockbackStrength.y, 0), ForceMode.Impulse);
            }
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
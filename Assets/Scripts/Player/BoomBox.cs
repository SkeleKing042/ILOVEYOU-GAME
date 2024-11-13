using ILOVEYOU.EnemySystem;
using ILOVEYOU.Management;
using ILOVEYOU.ProjectileSystem;
using UnityEngine;
using UnityEngine.Events;

namespace ILOVEYOU.Player
{
    [RequireComponent(typeof(Collider))]
    public class BoomBox : MonoBehaviour
    {
        [SerializeField] private UnityEvent m_onActivate = new UnityEvent();
        private bool m_triggerCheck;
        private float m_stunDuration;
        private void OnTriggerEnter(Collider other)
        {
            Enemy e = other.GetComponent<Enemy>();
            if (e)
            {
                e.GetStunned(m_stunDuration);
                Vector3 direction = (other.transform.position - transform.position).normalized;
                other.GetComponent<Rigidbody>().AddForceAtPosition(new Vector3(GameSettings.Current.GetKnockbackStrength.x * direction.x, GameSettings.Current.GetKnockbackStrength.y, GameSettings.Current.GetKnockbackStrength.x * direction.z), transform.position, ForceMode.Impulse);
            }
            Projectile p = other.GetComponent<Projectile>();
            if (p)
            {
                Destroy(p.gameObject);
            }
        }
        private void Awake()
        {
            GetComponent<SphereCollider>().radius = GameSettings.Current.GetKnockbackRadius;
            m_stunDuration = GameSettings.Current.GetKnockbackStunDuration;
        }
        private void Update()
        {
            if (gameObject.activeSelf && !m_triggerCheck)
            {
                m_onActivate.Invoke();
                m_triggerCheck = true;
                Invoke("DisableBB", GameSettings.Current.GetKnockbackWindow);
            }
        }
        private void DisableBB()
        {
            gameObject.SetActive(false);
            m_triggerCheck = false;
        }
    }
}
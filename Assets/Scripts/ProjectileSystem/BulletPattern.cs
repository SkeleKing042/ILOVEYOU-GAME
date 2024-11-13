using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static ILOVEYOU.ProjectileSystem.BulletPatternObject;

namespace ILOVEYOU
{
    namespace ProjectileSystem
    {
        public class BulletPattern : MonoBehaviour
        {

            [SerializeField] private BulletPatternObject m_patternObj;
            public BulletPatternObject GetBulletPatternObject { get { return m_patternObj; } }
            [SerializeField] private bool m_isPlayer;

            private BulletPatternArray _getBulletPattern(int i) { return m_patternObj.GetBulletArray(i); }

            private float[] m_cooldown;
            private float[] m_spinFactor;

            [SerializeField] private float m_fireSpeedMulti = 1f; //changes how fast things shoot
            [SerializeField] private float m_damageMulti = 1f; //changes how much damage things do

            private Transform m_target;

            [Header("Audio & Visuals")]
            [SerializeField] private GameObject m_muzzleFlash;
            [SerializeField] private float m_flashTime;
            [SerializeField] private UnityEvent m_onBulletFire;
            /// <summary>
            /// Goes through each array and adjusts cooldowns for each. If a cooldown for an array has reached 0, the specified array will fire.
            /// </summary>
            public void PatternUpdate()
            {
                for (int i = 0; i < m_patternObj.GetSize(); i++)
                {
                    try
                    {
                        m_cooldown[i] -= Time.deltaTime * m_fireSpeedMulti;
                    
                        if (m_cooldown[i] <= 0)
                        {
                            ShootArray(i);
                            m_cooldown[i] = _getBulletPattern(i).FireSpeed;
                        }

                        m_spinFactor[i] += _getBulletPattern(i).SpinFactor * Time.deltaTime;
                    }
                    catch (Exception)
                    {
                        Debug.LogWarning("Bullet object updated in play mode! Please restart in order for it to update properly.");
                    }
                }
            }

            private void Start()
            {
                if (m_patternObj) PatternInitialize();
            }

            /*private void Update()
            {
                //PatternUpdate();
            }*/

            /// <summary>
            /// adds target for the bullets to inherit can be used for homing bullets
            /// </summary>
            /// <param name="target">transform of the target</param>
            public void AddTarget(Transform target)
            {
                m_target = target;
            }
            /// <summary>
            /// Replaces the current bullet pattern with a new one
            /// </summary>
            /// <param name="bulletPatternObject">Pattern object to replace the current one.</param>
            public void ChangePattern( BulletPatternObject bulletPatternObject)
            {
                m_patternObj = bulletPatternObject;
                PatternInitialize();
            }

            /// <summary>
            /// resets variables if for when you want to add a new bullet array
            /// </summary>
            public void PatternInitialize()
            {
                m_cooldown = new float[m_patternObj.GetSize()];
                m_spinFactor = new float[m_patternObj.GetSize()];
                for (int i = 0; i < m_patternObj.GetSize(); i++)
                {
                    m_cooldown[i] = _getBulletPattern(i).FireSpeedOffset;
                }
            }

            /// <summary>
            /// creates all bullets in an array and rotates them approprietely
            /// </summary>
            /// <param name="selectedPattern"></param>
            public void ShootArray(int selectedPattern)
            {
                float rotOffset = 0f;
                //did this for readability
                BulletPatternArray pattern = _getBulletPattern(selectedPattern);

                for (int i = 0; i < pattern.BulletsPerArray; i++)
                {
                    //create object
                    GameObject newBullet = Instantiate(pattern.BulletPrefab);

                    //set position and rotation (depending on how the inital rotation is selected)
                    switch (pattern.InitialAngle)
                    {
                        case BulletPatternArray.AngleMode.Worldspace:
                            newBullet.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(new Vector3(0, pattern.AngleOffset + m_spinFactor[selectedPattern], 0)));
                            break;
                        case BulletPatternArray.AngleMode.ObjectTransform:
                            newBullet.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y + pattern.AngleOffset + m_spinFactor[selectedPattern], 0)));
                            break;
                        case BulletPatternArray.AngleMode.TargetObject:
                            newBullet.transform.position = transform.position; //reason this is seperated is to allow the look at to be influenced by the object's position
                            newBullet.transform.LookAt(m_target, transform.up);
                            newBullet.transform.rotation = Quaternion.Euler(new Vector3(0, newBullet.transform.rotation.eulerAngles.y + pattern.AngleOffset + m_spinFactor[selectedPattern], 0));
                            break;
                    }
                    //initialize bullet script
                    newBullet.GetComponent<Projectile>().InitializeProjectile(pattern.BulletSpeed, pattern.BulletAcceleration, pattern.SidewaysBulletAcceleration,
                        m_target, pattern.BulletDamage * m_damageMulti, pattern.BulletPierce, pattern.BulletLifetime, m_isPlayer);
                    
                    //add position offset to bullet
                    Vector3 posOffset = newBullet.transform.right * pattern.Offset.x + newBullet.transform.forward * pattern.Offset.y;
                    newBullet.transform.SetPositionAndRotation(newBullet.transform.position + posOffset, Quaternion.Euler(0, newBullet.transform.rotation.eulerAngles.y + rotOffset, 0));

                    //increase rotation offset for future loops
                    rotOffset += pattern.SpreadWithinArrays;
                }

                if (m_muzzleFlash)
                {
                    m_muzzleFlash.SetActive(true);
                    Invoke(nameof(_disableMuzzleFlash), m_flashTime);
                    m_onBulletFire.Invoke();
                }
            }
            private void _disableMuzzleFlash()
            {
                m_muzzleFlash.SetActive(false);
            }
            //voids for changing or setting fire speed multiplier
            public void SetFireSpeed(float speed)
            {
                m_fireSpeedMulti = speed;
            }
            public void AddFireSpeed(float speed)
            {
                m_fireSpeedMulti += speed;
            }
            //voids for changing or setting dmage multiplier
            public void SetDamage(float damage)
            {
                m_damageMulti = damage;
            }
            public void AddDamage(float damage)
            {
                m_damageMulti += damage;
            }
        }
    }
}
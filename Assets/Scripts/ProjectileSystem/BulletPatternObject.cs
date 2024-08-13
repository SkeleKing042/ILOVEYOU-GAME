using System;
using UnityEngine;

namespace ILOVEYOU 
{ 
    namespace ProjectileSystem 
    {
        [CreateAssetMenu(fileName = "BulletPattern", menuName = "Bullets/BulletPattern", order = 1)]
        public class BulletPatternObject : ScriptableObject
        {
            [Serializable]
            public class BulletPatternArray
            {
                public enum AngleMode
                {
                    Worldspace,
                    ObjectTransform,
                    TargetObject
                }


                public string Name; //here just for organisation

                public GameObject BulletPrefab;

                [Header("Transform variables")]
                [Tooltip("How many Bullets spawn in the array")] public int BulletsPerArray;
                [Tooltip("The spread inbetween bullets")] public float SpreadWithinArrays;
                [Tooltip("What influences the initial angle. Worldspace causes the projectiles to be influenced by nothing. " +
                    "Object transform causes the projectiles to spawn facing the same direction as the object that created it. " +
                    "Target Transform will cause the projectiles to face an assigned object")] 
                    public AngleMode InitialAngle;
                [Tooltip("Offset applied to the initial angle")] public float AngleOffset;
                [Tooltip("The speed at which the bullet array will spin (if any)")] public float SpinFactor;

                [Header("Bullet variables")]
                [Tooltip("Initial damage of the projectile")] public float BulletDamage;
                [Tooltip("How many times a bullet can go through enemies")] public int BulletPierce;
                [Tooltip("Initial speed of the projectile")] public float BulletSpeed;
                [Tooltip("How fast the bullet accelerates (if it accelerates at all)")] public float BulletAcceleration;
                [Tooltip("How fast the bullet accelerates to the side (if it accelerates at all)")] public float SidewaysBulletAcceleration;
                [Tooltip("Cooldown in seconds")] public float FireSpeed;
                [Tooltip("Extra seconds applied at start to offset the shooting")] public float FireSpeedOffset;
                [Tooltip("Lifetime of bullet in seconds")] public float BulletLifetime;
                [Tooltip("Position offset of the bullet (this is applied after all other ")] public Vector2 Offset;


            }
            [SerializeField] private BulletPatternArray[] m_bulletArrays;

            public BulletPatternArray GetBulletArray(int i) { return m_bulletArrays[i]; }
            public int GetSize() { return m_bulletArrays.Length; }


        }
    }

}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ILOVEYOU.Management
{
    public class ControllerVibrationHandler : MonoBehaviour
    {
        //singleton stuff
        public static ControllerVibrationHandler Instance {get; private set;}
        private void Awake()
        {
            DontDestroyOnLoad(this);
            if(Instance == null)
                Instance = this;
            else
                Destroy(this);
        }
        //----------------------------------------------------------------------//
        public struct VibeInfo
        {
            //The low frequency to use
            public float LowFrequency {get; private set;}
            //The high frequency to use
            public float HighFrequency {get; private set;}
            //How long the controller should vibrate for
            public float Duration {get; private set;}
            //How long until the next vibration in a burst should wait
            public float Cooldown {get; private set;}

            /// <summary>
            /// The infomation for a unqiue vibration sequence.
            /// </summary>
            /// <param name="newDuration">How long the controller should vibrate for</param>
            /// <param name="newCooldown">How long until the next vibration in a burst should wait</param>
            /// <param name="newLowFrequency">The low frequency to use</param>
            /// <param name="newHighFrequency">The high frequency to use.<br></br>If left unaltered, it will use the low frequency value.</param>
            public VibeInfo(float newDuration, float newCooldown = 0f, float newLowFrequency = 1, float newHighFrequency = -1)
            {
                Duration = newDuration;
                Cooldown = newCooldown;
                if(newHighFrequency == -1)
                    newHighFrequency = newLowFrequency;
                LowFrequency = newLowFrequency;
                HighFrequency = newHighFrequency;
            }
        }
        /// <summary>
        /// The controller vibration itself
        /// </summary>
        private class vibeInstance : MonoBehaviour
        {
            private Gamepad m_device;

            public void StartVibes(Gamepad newDevice, VibeInfo vibe)
            {
                m_device = newDevice;

                m_device.SetMotorSpeeds(vibe.LowFrequency, vibe.HighFrequency);
                Invoke("EndVibes", vibe.Duration);
            }
            public void EndVibes()
            {
                m_device.SetMotorSpeeds(0, 0);
                Destroy(gameObject);
            }
        }
        /// <summary>
        /// Basic function for getting the controller to vibrate
        /// </summary>
        /// <param name="device">The gamepad to target</param>
        /// <param name="vibe"></param>
        public void SetMotors(Gamepad device, VibeInfo vibe)
        {
            if (device != null)
            {
                GameObject newGO = new GameObject();
                DontDestroyOnLoad(newGO);
                newGO.name = $"vibing for {vibe.Duration} seconds.";
                newGO.AddComponent<vibeInstance>().StartVibes(device, vibe);
            }
        }
        /// <summary>
        /// Able to set multiple vibrations in sequence.
        /// </summary>
        /// <param name="device">The device to target</param>
        /// <param name="vibes"></param>
        public void SetMotors(Gamepad device, VibeInfo[] vibes)
        {
            if(device != null)
                StartCoroutine(_startMotorBurst(device, vibes));
        }
        private IEnumerator _startMotorBurst(Gamepad device, VibeInfo[] vibes)
        {
            foreach(var vibe in vibes)
            {
                SetMotors(device, vibe);
                yield return new WaitForSeconds(vibe.Duration + vibe.Cooldown);
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILOVEYOU
{
    namespace Player
    {

        public class PointerArrow : MonoBehaviour
        {
            public Transform Target;
            private bool m_isRenderering;

            // Update is called once per frame
            void Update()
            {
                if (Target.gameObject.activeSelf)
                {
                    if (!m_isRenderering)
                    {
                        ToggleRenderer(true);
                    }
                    transform.LookAt(new Vector3(Target.position.x, transform.position.y, Target.position.z));
                }
                else
                {
                    ToggleRenderer(false);
                }
            }
            private void ToggleRenderer(bool set)
            {
                m_isRenderering = set;
                GetComponentInChildren<MeshRenderer>().enabled = m_isRenderering;
            }
        }

    }
}
using ILOVEYOU.Audio;
using UnityEngine;

namespace ILOVEYOU.Player
{
    public class PlayerAnimatorEvents : MonoBehaviour
    {
        private Animator m_animator;

        private void Start()
        {
            m_animator = GetComponent<Animator>();
        }

        public void FootStep(AnimationEvent evt)
        {
            if (_IsHeaviestAnimClip(evt.animatorClipInfo.clip))
            {
                //GetComponent<SoundManager>().PlayRandomSound(0);
            }
        }

        private bool _IsHeaviestAnimClip(AnimationClip currentClip)
        {
            var currentAnimatorClipInfo = m_animator.GetCurrentAnimatorClipInfo(0);
            float highestWeight = 0f;
            AnimationClip highestWeightClip = null;

            // Find the clip with the highest weight
            foreach (var clipInfo in currentAnimatorClipInfo)
            {
                if (clipInfo.weight > highestWeight)
                {
                    highestWeight = clipInfo.weight;
                    highestWeightClip = clipInfo.clip;
                }
            }

            return highestWeightClip != null && currentClip == highestWeightClip;
        }
    }

}


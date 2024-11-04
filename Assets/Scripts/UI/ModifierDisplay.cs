using UnityEngine;
using UnityEngine.UI;

namespace ILOVEYOU.UI
{
    public class ModifierDisplay : MonoBehaviour
    {
        private Transform m_targetCam;
        public Transform GetCamera { get { return m_targetCam; } set { m_targetCam = value; } }
        public void AddModifierToDisplay(Image modImage)
        {
            //create a new image instance as a child
            Image newImage = Instantiate(modImage);
            newImage.transform.SetParent(transform, false);
            newImage.transform.localScale = Vector3.one;
        }

        // Update is called once per frame
        void Update()
        {
            transform.LookAt(m_targetCam.transform);
        }
    }
}

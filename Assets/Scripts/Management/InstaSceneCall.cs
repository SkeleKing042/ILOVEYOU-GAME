using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ILOVEYOU
{
    namespace Management
    {
        public class InstaSceneCall : MonoBehaviour
        {
            public string TargetScene;
            // Start is called before the first frame update
            void Start()
            {
                SceneManager.LoadScene(TargetScene);
            }
        }
    }
}

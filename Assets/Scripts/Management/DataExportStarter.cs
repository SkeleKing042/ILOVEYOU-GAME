using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DataExporter
{
    public class DataExportStarter : MonoBehaviour
    {
        [SerializeField] private string m_targetScene;
        // Start is called before the first frame update
        void Start()
        {
            DataExport.Init();
        }
        public void ChangeScene()
        {
            SceneManager.LoadSceneAsync(m_targetScene);
        }
    }
}
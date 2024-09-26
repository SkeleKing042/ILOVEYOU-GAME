using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataExporter
{
    public class DataExportStarter : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            DataExport.Init();
        }
    }
}
using System.IO;
using UnityEngine;
using ILOVEYOU.Tools;
using System.Collections.Generic;

namespace ILOVEYOU.Management
{
    public class GetLocalSettings : MonoBehaviour
    {
        private string m_filePath = "CustomSettings";

        public static List<GameSettings> List { get; private set; }

        // Start is called before the first frame update
        void Start()
        {
            //Check the folder paths
            DirectoryUtilities.CheckForFolderPath(DirectoryUtilities.GameDataPath);
            DirectoryUtilities.CheckForFolderPath($"{DirectoryUtilities.GameDataPath}{m_filePath}/");

            //Get the folder with the custom settings
            var paths = Directory.GetFiles($"{DirectoryUtilities.GameDataPath}{m_filePath}");
            List = new();

            //Add each custom settings to the list
            foreach (var path in paths)
            {
                GameSettings tmplt = (GameSettings)ScriptableObject.CreateInstance("GameSettings");
                List.Add(JsonHandler.ReadData(path, tmplt));
            }
        }
    }
}
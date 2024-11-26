using System.IO;
using UnityEngine;

namespace ILOVEYOU.Tools
{
    public static class DirectoryUtilities
    {
        public static string GameDataPath => Application.persistentDataPath + "/GameData/";

        static public bool CheckForFolderPath(string path, bool doCreate = true)
        {
            if (!Directory.Exists(path))
            {
                Debug.Log($"Folder @ {path} not found!");
                if (doCreate)
                {
                    Debug.Log($"Creating folder @ {path}");
                    Directory.CreateDirectory(path);
                    return true;
                }
                return false;
            }
            Debug.Log($"Folder @ {path} found.");
            return true;
        }
        static public bool CheckForFilePath(string path)
        {
            if (!File.Exists(path))
            {
                Debug.Log($"File @ {path} not found!");
                return false;
            }
            Debug.Log($"File @ {path} found.");
            return true;
        }
    }
}
using ILOVEYOU.Management;
using System.IO;
using UnityEngine;

namespace ILOVEYOU.Tools
{
    public static class JsonHandler
    {

        /// <summary>
        /// Write a item to a text file with json formating.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="fileName">The name of the file</param>
        /// <param name="folderpath"></param>
        public static void WriteData<T>(T item, string fileName, string folderpath = "./GameData/ExportedJsons/")
        {
            DirectoryUtilities.CheckForFolderPath(folderpath);
            string json = JsonUtility.ToJson(item, true);
            StreamWriter sw = new($"{folderpath}{fileName}.txt");
            if (!File.Exists($"{folderpath}{fileName}.txt"))
            {
                File.Create($"{folderpath}{fileName}.txt");
            }
            sw.WriteLine(json);
            sw.Close();
            Debug.Log($"Exported \"{fileName}.txt\" to \"{folderpath}\"\nTo use a custom setting, place it in \"\\AppData\\locallow\\Ransomware Games\\ILOVEYOU\\GameData\\CustomSettings\\\"");
            //File.WriteAllText($"{folderpath}{fileName}.txt", json);
        }
        /// <summary>
        /// Reads a json formated text file for the requested item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns>The file at the path</returns>
        public static T ReadData<T>(string filePath, T overrideItem = default)
        {
            if(!DirectoryUtilities.CheckForFilePath(filePath)) return default;
            string json = File.ReadAllText($"{filePath}");
            if (overrideItem != null)
            {
                JsonUtility.FromJsonOverwrite(json, overrideItem);
                return overrideItem;
            }
            else
            {
                return JsonUtility.FromJson<T>(json);
            }
        }
    }
}
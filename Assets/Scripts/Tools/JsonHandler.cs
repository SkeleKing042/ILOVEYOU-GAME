using ILOVEYOU.Management;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
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
        
        public class AsyncMethod<T>{
            public delegate T CreateNew(string name);
        }
        public static async Task<T[]> ReadAllFromFolder<T>(string folderPath, AsyncMethod<T>.CreateNew c)
        {
            //Get all the file paths from the given directory
            string[] filePaths = Directory.GetFiles(folderPath);
            //Find the number of items in the directory
            int fileCount = filePaths.Length;
            //Reset the array
            T[] files = new T[fileCount];


            for(int i = 0; i < fileCount; i++){
                //Create a temporery item
                string fileName = Path.GetFileName($"{filePaths[i]}");
                fileName = fileName.Remove(fileName.Length - ".txt".Length, ".txt".Length);
                files[i] = c(fileName);
                //Read the file at the current index
                string json = File.ReadAllText($"{filePaths[i]}");
                //Overwrite the temporery item with the one in the json file
                JsonUtility.FromJsonOverwrite(json, files[i]);
            }
            //All the files have now been loaded into the array.
            return files;
        }
    }
}
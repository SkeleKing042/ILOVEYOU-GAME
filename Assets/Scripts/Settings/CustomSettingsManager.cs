using UnityEngine;
using ILOVEYOU.Tools;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ILOVEYOU.Management
{
    public class CustomSettingsManager : MonoBehaviour
    {
        private static string m_filePath = "CustomSettings";

        public static List<GameSettings> SetList { get; private set; } = new();

        // Start is called before the first frame update
        public static async Task<GameSettings[]> GetSettings()
        {
            //Check the folder paths
            DirectoryUtilities.CheckForFolderPath($"{DirectoryUtilities.GameDataPath}{m_filePath}/");

            GameSettings[] settings = await JsonHandler.ReadAllFromFolder<GameSettings>($"{DirectoryUtilities.GameDataPath}{m_filePath}/", _createNewTemplate);
            SetList.AddRange(settings);
            return settings;
        }
        private static GameSettings _createNewTemplate(string name){
            GameSettings set = (GameSettings)ScriptableObject.CreateInstance("GameSettings");
            set.name = name;
            return set;
        }
    }
}
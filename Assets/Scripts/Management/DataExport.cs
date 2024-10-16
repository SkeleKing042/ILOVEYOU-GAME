using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using System.IO;
using System.Globalization;
using System;

namespace DataExporter
{
    public static class DataExport
    {
        //The data that will be saved then exported
        private static Dictionary<string, object[]> m_dataSet = new();

        //the max length of data - good for seeing values over time
        private static uint m_arraySize = 16;
        public static int ReadArraySize => (int)m_arraySize;

        private static string m_fileName = $"Untitled";
        private static string m_fileDirectory = $".\\";
        private static string m_configPath = $".\\config.txt";

        static DataExport()
        {
            //check for an existing config
            if (!File.Exists(m_configPath))
            {
                //create default config
                TextWriter tw = new StreamWriter(m_configPath, false);
                tw.WriteLine(m_fileName);
                tw.WriteLine(m_fileDirectory);
                tw.WriteLine(m_arraySize);
                tw.Close();
            }
            else
            {
                //read the config file
                string[] lines = File.ReadAllLines(m_configPath);
                m_fileName = $"{lines[0]}";
                m_fileDirectory = $"{lines[1]}";
                m_arraySize = Convert.ToUInt32(lines[2]);
            }


            //check if the output path exists
            FileInfo fi = new FileInfo(m_fileDirectory);
            if (!fi.Directory.Exists)
            {
                //otherwise create it
                System.IO.Directory.CreateDirectory(fi.DirectoryName);
            }
        }
        /// <summary>
        /// Checks for the dictionary <paramref name="key"/>, and can create it if needed.
        /// </summary>
        /// <param name="key">Dictionary Key</param>
        /// <param name="createIfMissing">If the key should be added to the dictionary if its not found.</param>
        private static void _validateTarget(string key, bool createIfMissing = false)
        {
            if (!m_dataSet.ContainsKey(key))
            {
                if (createIfMissing)
                {
                    Debug.LogWarning($"{key} doesn't exist in the dictionary, creating key!");
                    m_dataSet.TryAdd(key, new object[m_arraySize]);
                }
                else
                {
                    Debug.LogError($"Data export dictionary doesn't contain the key \"{key}\".");
                }
            }
        }
        /// <summary>
        /// Returns a value reference at the given <paramref name="index"/> and <paramref name="key"/>.
        /// </summary>
        /// <param name="key">Dictionary Key</param>
        /// <param name="index">Index in dictionary.</param>
        /// <param name="createIfMissing">If the key should be added to the dictionary if its not found.</param>
        public static ref object GetValue(string key, int index = 0, bool createIfMissing = false)
        {
            key = key.ToUpper();
            _validateTarget(key, createIfMissing);
            return ref m_dataSet[key][index];
        }
        /// <summary>
        /// Returns all the values from a <paramref name="key"/>
        /// </summary>
        /// <param name="key">Dictionary key</param>
        /// <returns></returns>
        public static object[] ReadValues(string key)
        {
            return m_dataSet[key];
        }
        /// <summary>
        /// Exports the data out as an CSV file
        /// </summary>
        public static void ExportCSV()
        {
            //set the path and unique name
            var csvPath = m_fileDirectory + m_fileName + $" - {DateTime.Now.ToFileTime()}.csv";
            Debug.Log($"Exporting csv file to {csvPath}.");

            //create and clear the file
            TextWriter tw = new StreamWriter(csvPath, false);

            //set the keys as headers
            string titles = "";
            foreach (var key in m_dataSet.Keys)
            {
                titles += $"{key},";
            }
            tw.WriteLine(titles);
            tw.Close();

            //begin writing in the data
            tw = new StreamWriter(csvPath, true);

            //go through each value - the y
            for (int i = 0; i < m_arraySize; i++)
            {
                string output = "";
                //go through each key - the x
                foreach (var key in m_dataSet.Keys)
                {
                    output += $"{GetValue(key, i)},";
                }
                tw.WriteLine($"{output}");
            }
            tw.Close();

            Debug.Log("CSV exported!!");

            //Clear
            m_dataSet = new();
        }
    }
}
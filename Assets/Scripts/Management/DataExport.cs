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
        static Dictionary<string, int[]> m_dataSet = new Dictionary<string, int[]>();
        public enum Operation
        {
            Add,
            Multiply,
            Set,
            Reset
        };
        //the max length of data - good for seeing values over time
        private static uint m_arraySize = 256;
        public static int ReadArraySize => (int)m_arraySize;

        private static string m_fileName = $"Untitled - {DateTime.Now.ToFileTime()}.csv";
        private static string m_fileDirectory = $".\\";
        private static string m_configPath = $".\\config.txt";

        public static void Init()
        {
            //check for the config
            if (!File.Exists(m_configPath))
            {
                //create default config
                TextWriter tw = new StreamWriter(m_configPath, false);
                tw.WriteLine("Untitled");
                tw.WriteLine(".\\");
                tw.WriteLine("256");
                tw.Close();
            }

            //read the config file
            string[] lines = File.ReadAllLines(m_configPath);
            m_fileName = $"{lines[0]} - {DateTime.Now.ToFileTime()}.csv";
            m_fileDirectory = $"{lines[1]}";
            m_arraySize = Convert.ToUInt32(lines[2]);

            //check if the output path exists
            FileInfo fi = new FileInfo(m_fileDirectory);
            if (!fi.Directory.Exists)
            {
                System.IO.Directory.CreateDirectory(fi.DirectoryName);
            }
        }
        /// <summary>
        /// Modifies the value at the given index and key.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        /// <param name="index"></param>
        /// <param name="oper"></param>
        public static void UpdateValue(string target, int value, int index, Operation oper)
        {
            //Debug.Log($"Adding {value} to {target}.");
            if (!m_dataSet.ContainsKey(target))
            {
                m_dataSet.TryAdd(target, new int[m_arraySize]);
            }

            switch (oper)
            {
                case Operation.Add:
                    m_dataSet[target][index] += value;
                    break;
                case Operation.Multiply:
                    m_dataSet[target][index] *= value;
                    break;
                case Operation.Set:
                    m_dataSet[target][index] = value;
                    break;
                case Operation.Reset:
                    m_dataSet[target][index] = 0;
                    break;
            }
        }
        /// <summary>
        /// Returns the value at the given index from a key
        /// </summary>
        /// <param name="target"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static int ReadValue(string target, int index)
        {
            int[] outval = new int[m_arraySize];
            if (m_dataSet.TryGetValue(target, out outval))
            {
                Debug.Log($"{outval[index]} read from {target}.");
                return outval[index];
            }
            else
            {
                Debug.Log($"{target} has not yet been made.");
                return 0;
            }
        }
        /// <summary>
        /// Returns all the values from a key
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int[] ReadValues(string target)
        {
            int[] outval = new int[m_arraySize];
            m_dataSet.TryGetValue(target, out outval);
            Debug.Log($"Read from {target}.");
            return outval;
        }
        /// <summary>
        /// Exports the data out as an CSV file
        /// </summary>
        /// <param name="path"></param>
        public static void ExportCSV()
        {
            //set the path
            var csvPath = m_fileDirectory + m_fileName;
            Debug.Log($"Exporting csv file to {csvPath}.");

            //create and clear the file
            TextWriter tw = new StreamWriter(csvPath, false);

            //set the keys as headers
            string titles = "";
            foreach (var key in m_dataSet.Keys)
            {
                titles += $"{key}, ";
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
                    output += $"{ReadValue(key, i)}, ";
                }
                tw.WriteLine($"{output}");
            }
            tw.Close();

            Debug.Log("CSV exported!!");
        }
    }
}
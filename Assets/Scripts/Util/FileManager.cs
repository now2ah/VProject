using UnityEngine;
using System;
using System.IO;

namespace VProject.Utils
{
    public static class FileManager
    {
        private static readonly string _filePath = Application.persistentDataPath;

        public static void SaveFile(string data, string fileName)
        {
            string filePath = Path.Combine(_filePath, fileName);
            File.WriteAllText(filePath, data);
        }

        public static string LoadFile(string fileName)
        {
            string filePath = Path.Combine(_filePath, fileName);
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            else
            {
                return null;
            }
        }
    }
}
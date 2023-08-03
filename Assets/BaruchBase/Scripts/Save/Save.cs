using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

namespace Baruch
{
    public static class Save
    {
        private static readonly string _savePath = Application.persistentDataPath + "/save.dat";

        public static bool Exists()
        {
            return File.Exists(_savePath);
        }

        public static void SaveGame()
        {
            SaveData[] save = SaveAttributeFinder.FindSaveFields();
            FileStream fileStream = File.Create(_savePath);
            BinaryFormatter binaryFormatter = new();
            binaryFormatter.Serialize(fileStream, save);
            fileStream.Close();
        }



        public static void Load()
        {
            if (Exists())
            {
                Debug.Log("LOADED");

                FileStream fileStream = File.Open(_savePath, FileMode.Open);
                BinaryFormatter binaryFormatter = new();
                var saveDataArray = (SaveData[])binaryFormatter.Deserialize(fileStream);

                foreach (var item in saveDataArray)
                {
                    var saveData = item;

                    saveData.FieldInfo.SetValue(null, saveData.Value);

                }
                fileStream.Close();

            }


        }

        public static void DeleteSave()
        {
            PlayerPrefs.DeleteAll();
            if (!File.Exists(_savePath))
            {
                Debug.Log("There is no save data!");
                return;
            }
            File.Delete(_savePath);
            Debug.Log("Save deleted!");
        }



    }
}
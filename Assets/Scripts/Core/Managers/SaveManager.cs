using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Core.Managers
{
    public class SaveManager
    {
        // private DatabaseReference _firebaseDB;

        public SaveManager()
        {
            
        }

        public void Save(ISaveData saveData)
        {
            var saveID = saveData.GetType().FullName;
            var saveJson = JsonConvert.SerializeObject(saveData);
            var path = $"{Application.persistentDataPath}/{saveID}.Save";

            File.WriteAllText(path, saveJson);
        }

        public void Load<T>(Action<T> onComplete) where T : ISaveData
        {
            if (!HasData<T>())
            {
                onComplete.Invoke(default);
                return;
            }

            var saveID = typeof(T).FullName;
            var path = $"{Application.persistentDataPath}/{saveID}.Save";

            var saveJson = File.ReadAllText(path);
            var saveData = JsonConvert.DeserializeObject<T>(saveJson);
            onComplete.Invoke(saveData);
        }

        public bool HasData<T>() where T : ISaveData
        {
            var saveID = typeof(T).FullName;
            var path = $"{Application.persistentDataPath}/{saveID}.Save";
            return File.Exists(path);
        }

        public void ClearAllData()
        {
            var path = Application.persistentDataPath;

            var files = Directory.GetFiles(path);

            foreach (var file in files)
            {
                if (file.Contains("Save"))
                {
                    File.Delete(file);
                }
            }

            PlayerPrefs.DeleteAll();
        }

    }

    public interface ISaveData
    {
        
    }
}
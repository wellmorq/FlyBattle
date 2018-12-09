using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace FlyBattle.Utils
{
    public class SaveLoad
    {
        public static T Load<T>(string path) where T : class
        {
            if (!File.Exists(path))
            {
                Debug.LogException(new UnityException($"Can`t find file in path: {path}"));
                return null;
            }

            BinaryFormatter bf = new BinaryFormatter();
            T obj = null;
            
            using (FileStream file = File.Open(path, FileMode.Open))
            {
                obj = (T) bf.Deserialize(file);
            }

            return obj;
        }

        public static bool Save<T>(T obj, string path) where T : class
        {
            bool flag = File.Exists(path);

            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, obj);
            }

            return flag;
        }

        public static bool DeleteFile(string path)
        {
            if (!File.Exists(path)) return false;
            
            File.Delete(path);
            return true;
        }
    }
}
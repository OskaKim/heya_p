using UnityEngine;
using System.IO;
using System.Text;

namespace Utility
{
    public class FileIOUtility
    {
        static public void CreateJsonFile(string createPath, string fileName, object obj)
        {
            var jsonData = JsonUtility.ToJson(obj);
            FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", createPath, fileName), FileMode.CreateNew);
            byte[] data = Encoding.UTF8.GetBytes(jsonData);
            fileStream.Write(data, 0, data.Length);
            fileStream.Close();
        }
        static public T LoadJsonFile<T>(string loadPath, string fileName)
        {
            FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", loadPath, fileName), FileMode.Open);
            byte[] data = new byte[fileStream.Length];
            fileStream.Read(data, 0, data.Length);
            fileStream.Close();
            string jsonData = Encoding.UTF8.GetString(data);
            return JsonUtility.FromJson<T>(jsonData);
        }
    }
}

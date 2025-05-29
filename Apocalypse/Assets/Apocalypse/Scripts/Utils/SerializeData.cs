using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Apocalypse.Utils
{
    public class SerializeData : MonoBehaviour
    {
        private readonly string path = Application.persistentDataPath + "/data.json";

        public void Serialize(object @object)
        {
            string json = JsonConvert.SerializeObject(@object, Formatting.Indented);
            File.WriteAllText(path, json);
        }

        public string Deserialize()
        {
            string json = File.ReadAllText(path);
            Debug.Log($"You're deserialize turret data: {json}");
            return json;
        }
    }
}
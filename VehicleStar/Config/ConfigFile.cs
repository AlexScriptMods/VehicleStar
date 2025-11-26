using System;
using System.IO;
using System.Xml.Serialization;

namespace VehicleStar
{
    [Serializable]
    public class ConfigData
    {
        public string OutputDir { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "VehicleStar-EXPORTS");
    }

    public class ConfigFile
    {
        private string filePath;
        public ConfigData data { get; private set; }

        public ConfigFile(string fileName)
        {
            filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

            if (File.Exists(filePath))
            {
                Load();
            }
            else
            {
                data = new ConfigData();
                Save();
            }
        }

        public void Load()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ConfigData));
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                data = (ConfigData)serializer.Deserialize(fs);
            }
        }

        public void Save()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ConfigData));
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                serializer.Serialize(fs, data);
            }
        }
    }
}

using GTA;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VehicleStar
{
    [Serializable]
    public class VehStar
    {
        public int VehicleHash { get; set; }

        public VehStar() { }

        public VehStar(string pathToVehStar, int vehicleHash)
        {
            VehicleHash = vehicleHash;
            SaveToFile(pathToVehStar);
        }

        private void SaveToFile(string path)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));

                XmlSerializer serializer = new XmlSerializer(typeof(VehStar));
                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    serializer.Serialize(fs, this);
                }
            }

            catch (Exception ex)
            {
                GTA.UI.Screen.ShowSubtitle($"~r~Failed to save VehStar Config File: {ex.Message}");
            }
        }

        public static VehStar LoadFromFile(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    GTA.UI.Screen.ShowSubtitle("~y~VehStar Config file not found!");
                    return null;
                }

                XmlSerializer serializer = new XmlSerializer(typeof(VehStar));
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    VehStar loaded = (VehStar)serializer.Deserialize(fs);
                    return loaded;
                }
            }

            catch (Exception ex)
            {
                GTA.UI.Screen.ShowSubtitle($"~r~Failed to load VehStar Config File: {ex.Message}");
                return null;
            }
        }
    }
}

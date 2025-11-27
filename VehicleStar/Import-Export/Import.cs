using GTA.Math;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;

namespace VehicleStar
{
    //load an xml to a list of DataRecord
    public static class Import
    {
        public static List<RecordData> LoadFromXML(string xmlPath)
        {
            if (!File.Exists(xmlPath))
            {
                GTA.UI.Screen.ShowSubtitle("~r~XML file not found!");
                return null;
            }

            List<RecordData> recordings = new List<RecordData>();

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlPath);

                XmlNodeList items = doc.SelectNodes("//VehicleRecordList/Item");
                if (items == null || items.Count == 0)
                {
                    GTA.UI.Screen.ShowSubtitle("~r~No recordings found in XML!");
                    return null;
                }

                foreach (XmlNode item in items)
                {
                    RecordData rec = new RecordData();

                    // Time
                    XmlNode timeNode = item.SelectSingleNode("Time");
                    rec.Time = timeNode != null && timeNode.Attributes["value"] != null
                        ? int.Parse(timeNode.Attributes["value"].Value)
                        : 0;

                    // Position
                    XmlNode posNode = item.SelectSingleNode("Position");
                    rec.Position = ParseVector3(posNode);

                    // Rotation. 
                    XmlNode rotNode = item.SelectSingleNode("Rotation");
                    rec.Rotation = ParseVector3(rotNode);

                    // Velocity
                    XmlNode velNode = item.SelectSingleNode("Velocity");
                    rec.Velocity = ParseVector3(velNode);

                    // Forward
                    XmlNode forwardNode = item.SelectSingleNode("Forward");
                    rec.Forward = ParseVector3(forwardNode);

                    // Right
                    XmlNode rightNode = item.SelectSingleNode("Right");
                    rec.Right = ParseVector3(rightNode);

                    // Steering
                    XmlNode steerNode = item.SelectSingleNode("Steering");
                    rec.SteeringAngle = steerNode != null && steerNode.Attributes["value"] != null
                        ? float.Parse(steerNode.Attributes["value"].Value, CultureInfo.InvariantCulture)
                        : 0f;

                    // Gas
                    XmlNode gasNode = item.SelectSingleNode("GasPedal");
                    rec.Gas = gasNode != null && gasNode.Attributes["value"] != null
                        ? float.Parse(gasNode.Attributes["value"].Value, CultureInfo.InvariantCulture)
                        : 0f;


                    XmlNode brakeNode = item.SelectSingleNode("BrakePedal");
                    rec.Brake = brakeNode != null && brakeNode.Attributes["value"] != null
                        ? float.Parse(brakeNode.Attributes["value"].Value, CultureInfo.InvariantCulture)
                        : 0f;

                    XmlNode handNode = item.SelectSingleNode("Handbrake");
                    rec.UseHandbrake = handNode != null && handNode.Attributes["value"] != null
                        ? bool.Parse(handNode.Attributes["value"].Value)
                        : false;

                    rec.VehicleHash = 0;
                    if (recordings.Count == 0)
                    {
                        XmlNode vehNode = item.SelectSingleNode("VehicleHash");
                        if (vehNode != null && vehNode.Attributes["value"] != null)
                            rec.VehicleHash = int.Parse(vehNode.Attributes["value"].Value);
                    }

                    recordings.Add(rec);
                }

                GTA.UI.Screen.ShowSubtitle($"~g~Loaded {recordings.Count} recordings from XML");
                return recordings;
            }
            catch (Exception ex)
            {
                GTA.UI.Screen.ShowSubtitle($"~r~Failed to load XML: {ex.Message}");
                return null;
            }
        }

        private static Vector3 ParseVector3(XmlNode node)
        {
            if (node == null) return Vector3.Zero;

            float x = node.Attributes["x"] != null ? float.Parse(node.Attributes["x"].Value, CultureInfo.InvariantCulture) : 0f;
            float y = node.Attributes["y"] != null ? float.Parse(node.Attributes["y"].Value, CultureInfo.InvariantCulture) : 0f;
            float z = node.Attributes["z"] != null ? float.Parse(node.Attributes["z"].Value, CultureInfo.InvariantCulture) : 0f;

            return new Vector3(x, y, z);
        }
    }
}

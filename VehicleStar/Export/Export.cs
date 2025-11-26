using CodeWalker.GameFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace VehicleStar
{
    public class Export
    {
        List<RecordData> currentRecordings;
     

        public Export(List<RecordData> recordingsSingleVehicle, string outputPathXML, string outputPathYVR)
        {
            currentRecordings = recordingsSingleVehicle;
            SaveXMLAndYVR(outputPathXML, outputPathYVR);
        }

        #region Saving/Exporting
        public void SaveXMLAndYVR(string outputPathXML, string outputPathYVR)
        {
            var folderXML = Path.GetDirectoryName(outputPathXML);

            if (!Directory.Exists(folderXML))
            {
                Directory.CreateDirectory(folderXML);
            }

            var folderYVR = Path.GetDirectoryName(outputPathYVR);

            if (!Directory.Exists(folderYVR))
            {
                Directory.CreateDirectory(folderYVR);
            }

            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("VehicleRecordList");
            doc.AppendChild(root);

            foreach (var rec in currentRecordings)
            {
                XmlElement item = doc.CreateElement("Item");

                XmlElement time = doc.CreateElement("Time");
                time.SetAttribute("value", rec.Time.ToString());
                item.AppendChild(time);

                XmlElement pos = doc.CreateElement("Position");
                pos.SetAttribute("x", rec.Position.X.ToString("G"));
                pos.SetAttribute("y", rec.Position.Y.ToString("G"));
                pos.SetAttribute("z", rec.Position.Z.ToString("G"));
                item.AppendChild(pos);

                XmlElement vel = doc.CreateElement("Velocity");
                vel.SetAttribute("x", rec.Velocity.X.ToString("G"));
                vel.SetAttribute("y", rec.Velocity.Y.ToString("G"));
                vel.SetAttribute("z", rec.Velocity.Z.ToString("G"));
                item.AppendChild(vel);

                XmlElement forward = doc.CreateElement("Forward");
                forward.SetAttribute("x", rec.Forward.X.ToString("G"));
                forward.SetAttribute("y", rec.Forward.Y.ToString("G"));
                forward.SetAttribute("z", rec.Forward.Z.ToString("G"));
                item.AppendChild(forward);

                XmlElement right = doc.CreateElement("Right");
                right.SetAttribute("x", rec.Right.X.ToString("G"));
                right.SetAttribute("y", rec.Right.Y.ToString("G"));
                right.SetAttribute("z", rec.Right.Z.ToString("G"));
                item.AppendChild(right);

                XmlElement steer = doc.CreateElement("Steering");
                steer.SetAttribute("value", rec.SteeringAngle.ToString("G"));
                item.AppendChild(steer);

                XmlElement gas = doc.CreateElement("GasPedal");
                gas.SetAttribute("value", rec.Gas.ToString("G"));
                item.AppendChild(gas);

                XmlElement brake = doc.CreateElement("BrakePedal");
                brake.SetAttribute("value", rec.Brake.ToString("G"));
                item.AppendChild(brake);

                XmlElement handbrake = doc.CreateElement("Handbrake");
                handbrake.SetAttribute("value", rec.UseHandbrake.ToString());
                item.AppendChild(handbrake);

                root.AppendChild(item);
            }

            doc.Save(outputPathXML);

            //Convert to YVR
            var yvr = XmlYvr.GetYvr(doc);
            var yvrData = yvr.Save();

            File.WriteAllBytes(outputPathYVR, yvrData);
        }
        #endregion
    }
}

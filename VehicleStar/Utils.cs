using GTA;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VehicleStar
{
    public class Utils
    {

        public static string SelectVehStarFile()
        {
            GTA.UI.Screen.ShowSubtitle("~y~Select the .vehstar config file for the recording you wish to play~w~");
            Script.Wait(10);

            string selectedPath = null;

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                OpenFileDialog ofd = new OpenFileDialog
                {
                    Filter = "VehStar Config Files (*.vehstar)|*.vehstar",
                    Title = "Select your VehStar Config File",
                    InitialDirectory = Main.config.data.OutputDir,
                };

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    selectedPath = ofd.FileName;
                }
            });

            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join(); //wait for dialog to close

            return selectedPath;
        }

        static public string GetNewIndex()
        {
            string dir = Main.config.data.OutputDir;
            Directory.CreateDirectory(dir);

            int lastIndex = 0;

            //get all xml files
            var xmlFiles = Directory.GetFiles(dir, "*.xml", SearchOption.AllDirectories);

            if (xmlFiles.Length > 0)
            {
                lastIndex = xmlFiles
                    .Select(f => Path.GetFileNameWithoutExtension(f))

                    .Select(name =>
                    {
                        var match = Regex.Match(name, @"\d+");
                        return match.Success ? int.Parse(match.Value) : 0;
                    })
                    .Max();
            }

            int newIndex = lastIndex + 1;

            return newIndex.ToString("000");
        }

    }
}

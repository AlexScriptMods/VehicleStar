using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleStar
{
    public class Utils
    {
        static public int GetNewIndex()
        {
            string dir = Main.config.data.OutputDir;

            Directory.CreateDirectory(dir);

            int lastIndex = 0;
            var xmlFiles = Directory.GetFiles(dir, "record*.xml");
            if (xmlFiles.Length > 0)
            {
                lastIndex = xmlFiles
                    .Select(f => Path.GetFileNameWithoutExtension(f))
                    .Select(f => f.Substring("record".Length))
                    .Select(s => int.TryParse(s, out int n) ? n : 0)
                    .Max();
            }

            return lastIndex + 1;
        }
    }
}

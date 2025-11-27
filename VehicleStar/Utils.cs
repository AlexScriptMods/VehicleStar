using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VehicleStar
{
    public class Utils
    {

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

using LemonUI.Menus;
using System.Collections.Generic;
using System.IO;
using VehicleStar;

public static class MenuMain
{
    public static NativeMenu Create(UI ui)
    {
        var menu = new NativeMenu("VehicleStar", "Options");

        var recordItem = new NativeItem("Start");
        var stopItem = new NativeItem("Stop");
        var exportItem = new NativeItem("Export");
        var debugItem = new NativeItem("DEBUG Playback");
        var yvrItem = new NativeItem("YVR Playback");

        recordItem.Activated += (m, i) =>
        {
            Main.recorder.StartRecording();
        };

        stopItem.Activated += (m, i) =>
        {
            Main.recorder.StopRecording();
        };

        exportItem.Activated += (m, i) =>
        {
            string strIndex = Utils.GetNewIndex();

            string outputSubDir = Path.Combine(Main.config.data.OutputDir, strIndex);

            if (Directory.Exists(outputSubDir))
            {
                Directory.Delete(outputSubDir, true);
            }

            Directory.CreateDirectory(outputSubDir);

            string outputPathXML = Path.Combine(outputSubDir, $"record{strIndex}.xml");
            string outputPathYVR = Path.Combine(outputSubDir, $"record{strIndex}.yvr");
            string outputPathVEHSTAR = Path.Combine(outputSubDir, $"record{strIndex}.vehstar");

            Main.recorder.Export(outputPathXML, outputPathYVR, outputPathVEHSTAR);

            Export export = new Export();
            export.SaveXMLInternal(Path.Combine(outputSubDir, "internal.xml"), Main.recorder.GetRecordingsData());
        };

        debugItem.Activated += (m, i) =>
        {
            string vehStarPath = Utils.SelectVehStarFile();
            string directory = null;
            string fileNameWithoutExt = null;

            if (!string.IsNullOrEmpty(vehStarPath))
            {
                directory = Path.GetDirectoryName(vehStarPath);
                fileNameWithoutExt = Path.GetFileNameWithoutExtension(vehStarPath);
            }

            else
            {
                GTA.UI.Screen.ShowSubtitle("~r~No file selected~w~");
                return;
            }

            List<RecordData> recordings = new List<RecordData>();
            recordings = Import.LoadFromXML(Path.Combine(directory, "internal.xml"));

            Main.debugPlayback.PlaybackStartDebug(vehStarPath, recordings);
        };
        
        yvrItem.Activated += (m, i) =>
        {
            ui.OpenMenu(ui.yvrMenu); // push yvrMenu onto stack
        };

        menu.Add(recordItem);
        menu.Add(stopItem);
        menu.Add(debugItem);
        menu.Add(exportItem);
        menu.Add(yvrItem);

        return menu;
    }
}

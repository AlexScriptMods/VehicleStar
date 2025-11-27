using GTA;
using GTA.Native;
using LemonUI.Menus;
using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;

namespace VehicleStar
{
    public class UI
    {
        private NativeMenu mainMenu;

        public UI()
        {
            mainMenu = new NativeMenu("VehicleStar", "Options");

            var recordItem = new NativeItem("Start");
            var stopItem = new NativeItem("Stop");
            var exportItem = new NativeItem("Export");
            var debugItem = new NativeItem("DEBUG Playback (Export before this)");
            var yvrItem = new NativeItem("YVR Playback");

            //Callbacks
            recordItem.Activated += (menu, item) =>
            {
                Main.recorder.StartRecording();
            };

            stopItem.Activated += (menu, item) =>
            {
                Main.recorder.StopRecording();
            };

            debugItem.Activated += (menu, item) =>
            {
                string vehStarPath = SelectVehStarFile();
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

            exportItem.Activated += (menu, item) =>
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

            yvrItem.Activated += (menu, item) =>
            {
                Form_PlayYVR form = new Form_PlayYVR();
                form.ShowDialog();

                if (!form.isCancelled)
                {
                    string fileName = form.selectedFileName;
                    string fileIndex = form.selectedFileIndex;

                    Main.yvrPlayback.PlayRecording(fileIndex, fileName, form.shouldWarpIntoVehicle);
                }

                else
                {
                    GTA.UI.Screen.ShowSubtitle("~y~YVR Playback cancelled~w~");
                }
            };

            mainMenu.Add(recordItem);
            mainMenu.Add(stopItem);
            mainMenu.Add(debugItem);
            mainMenu.Add(exportItem);
            mainMenu.Add(yvrItem);

            mainMenu.Visible = false;
        }

        public void Update()
        {
            mainMenu.Process();

            HandleMouseScroll();
            Function.Call(Hash.SET_MOUSE_CURSOR_VISIBLE, false);
        }

        public void ToggleMenu()
        {
            mainMenu.Visible = !mainMenu.Visible;
        }

        private void HandleMouseScroll()
        {
            if (!mainMenu.Visible) return;

            int selected = mainMenu.SelectedIndex;

            int scroll = Game.GetControlValueNormalized(GTA.Control.CursorScrollUp) > 0 ? -1 :
                         Game.GetControlValueNormalized(GTA.Control.CursorScrollDown) > 0 ? 1 : 0;

            if (scroll != 0)
            {
                selected += scroll;

                if (selected < 0)
                {
                    selected = mainMenu.Items.Count - 1;
                }

                else if (selected >= mainMenu.Items.Count)
                {
                    selected = 0;
                }

                mainMenu.SelectedIndex = selected;
            }
        }

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

    }
}
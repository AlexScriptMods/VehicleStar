using GTA;
using GTA.Native;
using LemonUI.Menus;
using System;
using System.IO;
using System.Windows.Forms;

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
            var debugItem = new NativeItem("DEBUG Playback");
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
                Main.debugPlayback.PlaybackStartDebug(VehicleHash.Cypher, Main.recorder.GetRecordingsData());
            };

            exportItem.Activated += (menu, item) =>
            {
                int index = Utils.GetNewIndex();
                string outputPathXML = Path.Combine(Main.config.data.OutputDir, $"record{index}.xml");
                string outputPathYVR = Path.Combine(Main.config.data.OutputDir, $"record{index}.yvr");
                Main.recorder.Export(outputPathXML, outputPathYVR);
            };

            yvrItem.Activated += (menu, item) =>
            {
                Form_PlayYVR form = new Form_PlayYVR();
                form.ShowDialog();

                if (!form.isCancelled)
                {
                    string fileName = form.selectedFileName;
                    int fileIndex = form.selectedFileIndex;

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
    }
}
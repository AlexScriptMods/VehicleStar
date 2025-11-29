using GTA;
using System;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace VehicleStar
{
    public class Main : Script
    {
        static public AppMode mode = AppMode.IDLE;
        static public ConfigFile config;
        private UI ui;

        static public Recorder recorder;
        static public DebugPlayback debugPlayback;
        static public YVRPlayback yvrPlayback;

        public Main()
        {
            Tick += OnTick;
            KeyDown += OnKeyDown;

            ui = new UI();
            config = new ConfigFile("config.xml");

            recorder = new Recorder();
            debugPlayback = new DebugPlayback();
            yvrPlayback = new YVRPlayback();
        }

        public void OnTick(object sender, EventArgs e)
        {
            if (ui != null)
            {
                ui.Update();
            }

            if (mode == AppMode.DEBUG_PLAYBACK)
            {
                debugPlayback.PlaybackTickDebug();
            }

            if (mode == AppMode.RECORDING)
            {
                recorder.CaptureVehicleData();
            }
        }

        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Z)
            {
                if (ui != null)
                {
                    ui.ToggleMenu();
                }
            }

            if(e.KeyCode == Keys.Back)
            {
                ui.CloseCurrentMenu();
            }
        }

    }
}

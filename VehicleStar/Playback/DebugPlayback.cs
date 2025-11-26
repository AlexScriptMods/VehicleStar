using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleStar
{
    public class DebugPlayback
    {
        private int frame = 0;
        private List<RecordData> currentRecordings = new List<RecordData>();
        private Vehicle playbackVehicle;

        public void PlaybackStartDebug(VehicleHash vehicleModel, List<RecordData> recordings)
        {
            currentRecordings = recordings;

            if (currentRecordings.Count == 0)
            {
                GTA.UI.Screen.ShowSubtitle("~r~Error: No available recordings~w~");
                return;
            }

            Vector3 spawnPos = currentRecordings[0].Position + new Vector3(0, 0, 0.0f);
            playbackVehicle = World.CreateVehicle(vehicleModel, spawnPos);

            if (!playbackVehicle.Exists())
            {
                GTA.UI.Screen.ShowSubtitle("~r~Failed to spawn playback vehicle~w~");
                return;
            }

            //Warp player into vehicle
            Game.Player.Character.SetIntoVehicle(playbackVehicle, VehicleSeat.Driver);

            frame = 0; // start at first frame
            Main.mode = AppMode.DEBUG_PLAYBACK;

            GTA.UI.Screen.ShowSubtitle("~g~Debug playback started");
        }


        public void PlaybackTickDebug()
        {
            if (Main.mode != AppMode.DEBUG_PLAYBACK || playbackVehicle == null) return;

            if (frame >= currentRecordings.Count)
            {
                EndPlaybackDebug();
                return;
            }

            //get data for this frame
            RecordData recordDataThisFrame = currentRecordings[frame];

            playbackVehicle.Position = recordDataThisFrame.Position;
            playbackVehicle.Rotation = recordDataThisFrame.Rotation;

            frame++;
        }

        public void EndPlaybackDebug()
        {
            Main.mode = AppMode.IDLE;
            GTA.UI.Screen.ShowSubtitle("~r~Playback ended~w~");
        }
    }
}

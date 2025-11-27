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
    public class YVRPlayback
    {
        private Vehicle playbackVehicle;

        public void PlayRecording(string recordingIndex, string recordingName, bool shouldWarpIntoVehicle)
        {
            Main.mode = AppMode.YVR_PLAYBACK;

            string paddedIndex;

            //validate index
            if (!int.TryParse(recordingIndex, out int idx))
            {
                GTA.UI.Screen.ShowSubtitle("~r~Invalid recording index.~w~");
                return;
            }

            paddedIndex = idx.ToString("000");
            int index = int.Parse(paddedIndex);

            Function.Call(Hash.REQUEST_VEHICLE_RECORDING, index, recordingName);

            int timeout = Game.GameTime + 5000; //5sec timeout

            while (!Function.Call<bool>(Hash.HAS_VEHICLE_RECORDING_BEEN_LOADED, index, recordingName))
            {
                GTA.UI.Screen.ShowSubtitle($"~y~Loading recording ~w~{recordingName + paddedIndex + ".yvr"}~y~...~w~");
                Script.Wait(0);

                if(Game.GameTime > timeout)
                {
                    GTA.UI.Screen.ShowSubtitle($"~r~Failed to load:~w~ {recordingName + paddedIndex}.yvr~w~");
                    return;
                }
            }

            Vector3 startPos = Function.Call<Vector3>(Hash.GET_POSITION_OF_VEHICLE_RECORDING_AT_TIME, index, 0.0f, recordingName);
            Vector3 startRot = Function.Call<Vector3>(Hash.GET_ROTATION_OF_VEHICLE_RECORDING_AT_TIME, index, 0.0f, recordingName);

            playbackVehicle = World.CreateVehicle(VehicleHash.Cypher, startPos);

            Function.Call(Hash.SET_ENTITY_COLLISION, playbackVehicle, true, true);
            playbackVehicle.Rotation = startRot;

            //Warp into vehicle
            if(shouldWarpIntoVehicle)
            {
                Game.Player.Character.SetIntoVehicle(playbackVehicle, VehicleSeat.Passenger);
            }

            //Start playback
            Function.Call(Hash.START_PLAYBACK_RECORDED_VEHICLE, playbackVehicle, index, recordingName, true);

            GTA.UI.Screen.ShowSubtitle("~g~Playing recording...~w~");
        }

        public void EndPlayback()
        {
            if (playbackVehicle == null || Main.mode != AppMode.YVR_PLAYBACK)
            {
                return;
            }

            // Stop playback
            Function.Call(Hash.STOP_PLAYBACK_RECORDED_VEHICLE, playbackVehicle);

            Main.mode = AppMode.IDLE;

            GTA.UI.Screen.ShowSubtitle("~r~Playback ended~w~");
            //playbackVehicle.Delete();
        }
    }
}

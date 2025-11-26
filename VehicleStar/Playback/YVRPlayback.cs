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

        public void PlayRecording(int recordingIndex, string recordingName, bool shouldWarpIntoVehicle)
        {   
            Main.mode = AppMode.YVR_PLAYBACK;

            Function.Call(Hash.REQUEST_VEHICLE_RECORDING, recordingIndex, recordingName);

            while (!Function.Call<bool>(Hash.HAS_VEHICLE_RECORDING_BEEN_LOADED, recordingIndex, recordingName))
            {
                GTA.UI.Screen.ShowSubtitle($"~y~Loading recording ~w~{recordingName+recordingIndex+".yvr"}~y~...~w~");
                Script.Wait(0);
            }

            Vector3 startPos = Function.Call<Vector3>(Hash.GET_POSITION_OF_VEHICLE_RECORDING_AT_TIME, recordingIndex, 0.0f, recordingName);
            Vector3 startRot = Function.Call<Vector3>(Hash.GET_ROTATION_OF_VEHICLE_RECORDING_AT_TIME, recordingIndex, 0.0f, recordingName);

            Function.Call(Hash.SET_ENTITY_COLLISION, playbackVehicle, true, true);

            playbackVehicle = World.CreateVehicle(VehicleHash.Cypher, startPos);
            playbackVehicle.Rotation = startRot;

           

            //Warp into vehicle
            if(shouldWarpIntoVehicle)
            {
                Game.Player.Character.SetIntoVehicle(playbackVehicle, VehicleSeat.Passenger);
            }

            //Start playback
            Function.Call(Hash.START_PLAYBACK_RECORDED_VEHICLE, playbackVehicle, recordingIndex, recordingName, true);

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

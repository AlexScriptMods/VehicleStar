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
    public class Recorder
    {
        private List<RecordData> currentRecordings = new List<RecordData>();
        private int recordingStartTime = 0;

        #region Recording
        public void CaptureVehicleData()
        {
            Vehicle vehicle = Game.Player.Character.CurrentVehicle;
            if (vehicle == null)
            {
                return;
            }

            float steeringDeg = vehicle.SteeringAngle;
            float steeringRad = (float)(steeringDeg * (Math.PI / 180.0));

            RecordFrame(
        vehicle.Position,
        vehicle.Rotation,
        vehicle.Velocity,
        vehicle.ForwardVector,
        vehicle.RightVector,
        steeringRad,
        Function.Call<float>(GTA.Native.Hash.GET_CONTROL_NORMAL, 0, (int)GTA.Control.VehicleAccelerate),
        Function.Call<float>(GTA.Native.Hash.GET_CONTROL_NORMAL, 0, (int)GTA.Control.VehicleBrake),
        Function.Call<float>(GTA.Native.Hash.GET_CONTROL_NORMAL, 0, (int)GTA.Control.VehicleHandbrake) > 0.5f
    );
        }

        public void StartRecording()
        {
            Main.mode = AppMode.RECORDING;

            currentRecordings.Clear();
            recordingStartTime = Environment.TickCount;

            GTA.UI.Screen.ShowSubtitle("~g~ Recording Started~w~");
        }

        public void StopRecording()
        {
            Main.mode = AppMode.IDLE;
            GTA.UI.Screen.ShowSubtitle("~r~ Recording Stopped~w~");
        }

        public void Export(string XmlPath, string YvrPath)
        {
            Export export = new Export(currentRecordings, XmlPath, YvrPath);
            export.SaveXMLAndYVR(XmlPath, YvrPath);
        }

        public List<RecordData> GetRecordingsData()
        {
            return currentRecordings;
        }

        public void RecordFrame(
            Vector3 position,
            Vector3 rotation,
            Vector3 velocity,
            Vector3 forward,
            Vector3 right,
            float steering,
            float gas,
            float brake,
            bool handbrake)
        {
            if (Main.mode != AppMode.RECORDING)
            {
                return;
            }

            currentRecordings.Add(new RecordData
            {
                Time = Environment.TickCount - recordingStartTime,
                Position = position,
                Rotation = rotation,
                Velocity = velocity,
                Forward = forward,
                Right = right,
                SteeringAngle = steering,
                Gas = gas,
                Brake = brake,
                UseHandbrake = handbrake
            });
        }
        #endregion
    };
}

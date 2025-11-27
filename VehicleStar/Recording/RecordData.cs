using GTA;
using GTA.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleStar
{
    public class RecordData
    {
        public int Time { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Right { get; set; }
        public Vector3 Forward { get; set; }
        public Vector3 Velocity { get; set; }
        public float SteeringAngle { get; set; }
        public float Gas { get; set; }
        public float Brake { get; set; }
        public bool UseHandbrake { get; set; }

        public int VehicleHash { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAcceleration
{
    public class VectorD
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
    }

    public class LocationHistory
    {
        private readonly List<(double, VectorD)> locationHistory = new List<(double, VectorD)>(3);

        /// <summary>
        /// The velocity per axis of the latest location update.
        /// </summary>
        public VectorD Velocity { get; private set; }

        /// <summary>
        /// The acceleration per axis of the latest location update.
        /// </summary>
        public VectorD Acceleration { get; private set; }

        /// <summary>
        /// Update the location information and time interval to be used to velocity and acceleration calculation. 
        /// </summary>
        /// <param name="location">tuple containing the timestamp in CxStopwatch ticks(this would be converted to ms)
        /// and location VectorD.</param>
        public void UpdateLocationHistory((double, VectorD) location)
        {
            // Remove first item added and add new item
            if (locationHistory.Count == 3)
            {
                locationHistory[0] = locationHistory[1];
                locationHistory[1] = locationHistory[2];
                locationHistory.RemoveAt(2);
            }

            locationHistory.Add(location);
            var i = locationHistory.Count - 1;
            Velocity = getVelocity(i);
            Acceleration = getAcceleration(i);
        }

        private VectorD getVelocity(int i)
        {
            if (i == 0)
            {
                return new VectorD()
                {
                    X = 0,
                    Y = 0,
                    Z = 0
                };
            }
            var timeInterval = locationHistory[i].Item1 - locationHistory[i - 1].Item1;

            //Console.WriteLine("CURR X: " + locationHistory[i].Item2.X + " PREV X: " + locationHistory[i - 1].Item2.X + " INTERVAL: " + timeInterval);
            //Console.WriteLine("CURR Y: " + locationHistory[i].Item2.Y + " PREV Y: " + locationHistory[i - 1].Item2.Y + " INTERVAL: " + timeInterval);
            //Console.WriteLine("CURR Z: " + locationHistory[i].Item2.Z + " PREV Z: " + locationHistory[i - 1].Item2.Z + " INTERVAL: " + timeInterval);

            return new VectorD()
            {
                X = Math.Abs(locationHistory[i].Item2.X - locationHistory[i - 1].Item2.X) / timeInterval,
                Y = Math.Abs(locationHistory[i].Item2.Y - locationHistory[i - 1].Item2.Y) / timeInterval,
                Z = Math.Abs(locationHistory[i].Item2.Z - locationHistory[i - 1].Item2.Z) / timeInterval
            };
        }

        private VectorD getAcceleration(int i)
        {
            if (i == 0)
            {
                return new VectorD()
                {
                    X = 0,
                    Y = 0,
                    Z = 0
                };
            }

            var prevV = new VectorD();
            var timeInterval = locationHistory[i].Item1 - locationHistory[i - 1].Item1;
            switch (i)
            {
                case 1:
                    prevV = new VectorD()
                    {
                        X = locationHistory[0].Item2.X / timeInterval,
                        Y = locationHistory[0].Item2.Y / timeInterval,
                        Z = locationHistory[0].Item2.Z / timeInterval
                    };
                    break;
                case 2:
                    prevV = new VectorD()
                    {
                        X = Math.Abs(locationHistory[1].Item2.X - locationHistory[0].Item2.X) / timeInterval,
                        Y = Math.Abs(locationHistory[1].Item2.Y - locationHistory[0].Item2.Y) / timeInterval,
                        Z = Math.Abs(locationHistory[1].Item2.Z - locationHistory[0].Item2.Z) / timeInterval
                    };
                    break;
            }

            return new VectorD()
            {
                X = (Velocity.X - prevV.X) / timeInterval,
                Y = (Velocity.Y - prevV.Y) / timeInterval,
                Z = (Velocity.Z - prevV.Z) / timeInterval
            };
        }
    }
}

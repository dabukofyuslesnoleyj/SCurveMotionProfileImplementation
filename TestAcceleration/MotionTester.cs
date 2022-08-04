using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TestAcceleration
{
    public enum MotionPhase
    {
        Full,
        Start,
        Mid,
        End
    }
    /// <summary>
    /// A sample implementation of the MotionProfile class on an XYZ coordinate system
    /// </summary>
    public class MotionTester
    {
        MotionProfile mpX;
        MotionProfile mpY;
        MotionProfile mpZ;

        bool isCP;

        double maxVelocity;
        double maxAcceleration;

        List<string> csvLines;
        public LocationHistory locationHistory = new LocationHistory();

        public MotionTester(double maxVelocity, double maxAcceleration, bool isCP)
        {
            this.maxVelocity = maxVelocity;
            this.maxAcceleration = maxAcceleration;
            this.isCP = isCP;

            csvLines = new List<string>();
            csvLines.Add("x, y, z, Vx, Vy, Vz, Ax, Ay, Az, Vxyz");

        }

        public void InitializeMotionProfiles(MotionPhase motionPhaseX = MotionPhase.Full,
            MotionPhase motionPhaseY = MotionPhase.Full,
            MotionPhase motionPhaseZ = MotionPhase.Full)
        {
            switch (motionPhaseX)
            {
                case MotionPhase.Start:
                    mpX = new CpFirstPointMotionProfile(this.maxVelocity, 0, this.maxAcceleration, 0);
                    break;
                case MotionPhase.Mid:
                    mpX = new CpMidPointMotionProfile(this.maxVelocity, 0, this.maxAcceleration, 0);
                    break;
                case MotionPhase.End:
                    mpX = new CpLastPointMotionProfile(this.maxVelocity, 0, this.maxAcceleration, 0);
                    break;
                default:
                    mpX = new MotionProfile(this.maxVelocity, 0, this.maxAcceleration, 0);
                    break;
            }

            switch (motionPhaseY)
            {
                case MotionPhase.Start:
                    mpY = new CpFirstPointMotionProfile(this.maxVelocity, 0, this.maxAcceleration, 0);
                    break;
                case MotionPhase.Mid:
                    mpY = new CpMidPointMotionProfile(this.maxVelocity, 0, this.maxAcceleration, 0);
                    break;
                case MotionPhase.End:
                    mpY = new CpLastPointMotionProfile(this.maxVelocity, 0, this.maxAcceleration, 0);
                    break;
                default:
                    mpY = new MotionProfile(this.maxVelocity, 0, this.maxAcceleration, 0);
                    break;
            }

            switch (motionPhaseZ)
            {
                case MotionPhase.Start:
                    mpZ = new CpFirstPointMotionProfile(this.maxVelocity, 0, this.maxAcceleration, 0);
                    break;
                case MotionPhase.Mid:
                    mpZ = new CpMidPointMotionProfile(this.maxVelocity, 0, this.maxAcceleration, 0);
                    break;
                case MotionPhase.End:
                    mpZ = new CpLastPointMotionProfile(this.maxVelocity, 0, this.maxAcceleration, 0);
                    break;
                default:
                    mpZ = new MotionProfile(this.maxVelocity, 0, this.maxAcceleration, 0);
                    break;
            }
        }

        public void PerformMotion(double initX, double initY, double initZ,
            double targetX, double targetY, double targetZ, double targetSpeed, double timeInterval = 0.1, double initV = 0)
        {
            List<double> posX = new List<double>();
            List<double> posY = new List<double>();
            List<double> posZ = new List<double>();

            if (isCP)
            {
                double distance = Math.Sqrt(Math.Pow(Math.Abs(targetX - initX), 2) +
                    Math.Pow(Math.Abs(targetY - initY), 2) +
                    Math.Pow(Math.Abs(targetZ - initZ), 2));
                double time = distance / targetSpeed;

                double speedX = Math.Abs(targetX - initX) / time;
                double speedY = Math.Abs(targetY - initY) / time;
                double speedZ = Math.Abs(targetZ - initZ) / time;

                mpX.InitializeMotionProfileParameters(speedX, initX, targetX, initV);
                mpY.InitializeMotionProfileParameters(speedY, initY, targetY, initV);
                mpZ.InitializeMotionProfileParameters(speedZ, initZ, targetZ, initV);
            }
            else
            {
                mpX.InitializeMotionProfileParameters(targetSpeed, initX, targetX, initV);
                mpY.InitializeMotionProfileParameters(targetSpeed, initY, targetY, initV);
                mpZ.InitializeMotionProfileParameters(targetSpeed, initZ, targetZ, initV);
            }

            double xyzTotalTime = Math.Max(mpX.TotalTime, Math.Max(mpY.TotalTime, mpZ.TotalTime));
            for (double i = 0; i < xyzTotalTime; i += timeInterval)
            {
                double pX = mpX.GetPointAtGivenTime(i);
                posX.Add(pX);
                double pY = mpY.GetPointAtGivenTime(i);
                posY.Add(pY);
                double pZ = mpZ.GetPointAtGivenTime(i);
                posZ.Add(pZ);

                locationHistory.UpdateLocationHistory(
                    (i * timeInterval,
                    new VectorD
                    {
                        X = pX,
                        Y = pY,
                        Z = pZ
                    })
                    );

                double Vx = locationHistory.Velocity.X;
                double Vy = locationHistory.Velocity.Y;
                double Vz = locationHistory.Velocity.Z;

                double Ax = locationHistory.Acceleration.X;
                double Ay = locationHistory.Acceleration.Y;
                double Az = locationHistory.Acceleration.Z;

                Console.WriteLine(Vx + ", " + Vy + ", " + Vz + ", " + Ax + ", " + Ay + ", " + Az);
            }

            int xyzPosCount = Math.Max(posX.Count, Math.Max(posY.Count, posZ.Count));
            for (int i = 0; i < xyzPosCount; i++)
            {
                double Vx;
                double Vy;
                double Vz;
                double Ax;
                double Ay;
                double Az;

                if (i == 0)
                {
                    Vx = initV;
                    Vy = initV;
                    Vz = initV;

                    Ax = 0;
                    Ay = 0;
                    Az = 0;
                }
                else if (i == 1)
                {
                    Vx = ((posX[i] - posX[i - 1]) / timeInterval) + initV;
                    Vy = ((posY[i] - posY[i - 1]) / timeInterval) + initV;
                    Vz = ((posZ[i] - posZ[i - 1]) / timeInterval) + initV;

                    double prevVx = ((posX[i - 1] - initV) / timeInterval) + initV;
                    double prevVy = ((posY[i - 1] - initV) / timeInterval) + initV;
                    double prevVz = ((posZ[i - 1] - initV) / timeInterval) + initV;

                    Ax = (Vx - prevVx) / timeInterval;
                    Ay = (Vy - prevVy) / timeInterval;
                    Az = (Vz - prevVz) / timeInterval;
                }
                else
                {
                    Vx = (posX[i] - posX[i - 1]) / timeInterval;
                    Vy = (posY[i] - posY[i - 1]) / timeInterval;
                    Vz = (posZ[i] - posZ[i - 1]) / timeInterval;

                    double prevVx = (posX[i - 1] - posX[i - 2]) / timeInterval;
                    double prevVy = (posY[i - 1] - posY[i - 2]) / timeInterval;
                    double prevVz = (posZ[i - 1] - posZ[i - 2]) / timeInterval;

                    Ax = (Vx - prevVx) / timeInterval;
                    Ay = (Vy - prevVy) / timeInterval;
                    Az = (Vz - prevVz) / timeInterval;

                    if (Vx - prevVx > 0 && Vx < targetSpeed - 0.00001)
                        Vx += initV;
                    if (Vy - prevVy > 0 && Vy < targetSpeed - 0.00001)
                        Vy += initV;
                    if (Vz - prevVz > 0 && Vz < targetSpeed - 0.00001)
                        Vz += initV;
                }

                double pX = posX[i] + initX;
                double pY = posY[i] + initY;
                double pZ = posZ[i] + initZ;

                //Vx = locationHistory.Velocity.X;
                //Vy = locationHistory.Velocity.Y;
                //Vz = locationHistory.Velocity.Z;
                //
                //Ax = locationHistory.Acceleration.X;
                //Ay = locationHistory.Acceleration.Y;
                //Az = locationHistory.Acceleration.Z;

                if (initX == targetX)
                {
                    pX = initX;
                    Vx = initV;
                    Ax = 0.0;
                }
                if (initY == targetY)
                {
                    pY = initY;
                    Vy = initV;
                    Ay = 0.0;
                }
                if (initZ == targetZ)
                {
                    pZ = initZ;
                    Vz = initV;
                    Az = 0.0;
                }

                csvLines.Add(pX + ", " + pY + ", " + pZ + ", " + Vx + ", " + Vy + ", " + Vz + ", " + Ax + ", " + Ay + ", " + Az);

            }
        }
        public void WriteCsvFile(string fileName)
        {
            StreamWriter file = new StreamWriter(fileName + ".csv");

            foreach (string line in csvLines)
            {
                file.WriteLine(line);
            }
            file.Close();
        }

        public static double GetDistance(double initX, double initY, double initZ, double targetX, double targetY, double targetZ)
        {
            return Math.Sqrt(Math.Pow(targetX - initX, 2) + Math.Pow(targetY - initY, 2) + Math.Pow(targetZ - initZ, 2));
        }
    }
}

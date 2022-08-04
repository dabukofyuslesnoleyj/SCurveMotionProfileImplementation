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

    //public class MotionProfile
    //{
    //    double maxVelocity;
    //    double minVelocity;
    //    double maxAcceleration;
    //    double minAcceleration;
    //
    //    double programmedVelocity;
    //    double averageAcceleration;
    //    double programmedAcceleration;
    //
    //    double jerk;
    //
    //    double transition1;
    //    double transition2;
    //    double transition3;
    //    double transition4;
    //    double transition5;
    //    double transition6;
    //    double transition7;
    //    public double totalTime { get; private set; }
    //
    //    public double distanceTravelled { get; private set; }
    //
    //    double targetVelocity1;
    //    double targetVelocity2;
    //
    //    double initialVelocity;
    //    double currentVelocity;
    //    public double currentPoint { get; private set; }
    //    double initialPoint;
    //    double finalPoint;
    //
    //    public MotionProfile(double maxVelocity, double minVelocity, double maxAcceleration, double minAcceleration)
    //    {
    //        this.maxAcceleration = maxAcceleration;
    //        this.minAcceleration = minAcceleration;
    //        this.maxVelocity = maxVelocity;
    //        this.minVelocity = minVelocity;
    //    }
    //
    //    public void initializeProgrammedParameters(double programmedVelocity, double initialPosition, double finalPosition, double initialVelocity = 0)
    //    {
    //        initialPoint = initialPosition;
    //        currentPoint = initialPosition;
    //        finalPoint = finalPosition;
    //        distanceTravelled = Math.Abs(finalPoint - initialPoint);
    //
    //        initializeMotionProfileParameters(programmedVelocity, initialVelocity);
    //    }
    //
    //    public void initializeMotionProfileParameters(double programmedVelocity, double initialVelocity)
    //    {
    //        this.initialVelocity = initialVelocity;
    //
    //        double accelerationSmoothness = 1.8;
    //        if (distanceTravelled < programmedVelocity * 3)
    //            accelerationSmoothness = 1.5;
    //        if (distanceTravelled < programmedVelocity * 2)
    //            accelerationSmoothness = 1.1;
    //
    //        if (distanceTravelled < programmedVelocity)
    //            programmedVelocity = distanceTravelled;
    //
    //        this.programmedVelocity = programmedVelocity;
    //        if (this.programmedVelocity > maxVelocity)
    //            this.programmedVelocity = maxVelocity;
    //
    //        this.programmedAcceleration = this.programmedVelocity * 0.9;
    //
    //        if (programmedAcceleration > maxAcceleration)
    //            this.programmedAcceleration = maxAcceleration;
    //
    //        averageAcceleration = this.programmedAcceleration / accelerationSmoothness;
    //        currentVelocity = initialVelocity;
    //
    //
    //        if (initialVelocity == 0)
    //            jerk = (Math.Pow(this.programmedAcceleration, 2) * averageAcceleration) /
    //                (this.programmedVelocity * (this.programmedAcceleration - averageAcceleration));
    //        else
    //            jerk = (Math.Pow(this.programmedAcceleration, 2) * averageAcceleration) /
    //                ((this.programmedVelocity - initialVelocity) * (this.programmedAcceleration - averageAcceleration));
    //
    //        transition1 = this.programmedAcceleration / jerk;
    //        transition2 = (this.programmedVelocity / averageAcceleration) - (this.programmedAcceleration / jerk);
    //        transition3 = this.programmedVelocity / averageAcceleration;
    //
    //        double accelerationDistance = this.programmedVelocity / 2 * transition3;
    //        double constVelocityDistance = distanceTravelled - (accelerationDistance * 2);
    //
    //        transition4 = (constVelocityDistance / this.programmedVelocity) + transition3;
    //        transition5 = transition4 + transition1;
    //        transition6 = transition4 + transition2;
    //        transition7 = transition4 + transition3;
    //        totalTime = transition4 + transition3;
    //
    //        if (double.IsNaN(totalTime))
    //            totalTime = 0;
    //
    //        targetVelocity1 = Math.Pow(programmedAcceleration, 2) / (2 * jerk);
    //        targetVelocity2 = this.programmedVelocity - targetVelocity1;
    //    }
    //
    //    public double AccelerationAtGivenTime(double time)
    //    {
    //        if (time >= 0 && transition1 > time)
    //        {
    //            return jerk * time;
    //        }
    //        else if (time >= transition1 && transition2 >= time)
    //        {
    //            return programmedAcceleration;
    //        }
    //        else if (time >= transition2 && transition3 >= time)
    //        {
    //            return programmedAcceleration - (jerk * (time - transition2));
    //        }
    //
    //        return 0;
    //    }
    //
    //
    //    public double VelocityAtGivenTime(double time)
    //    {
    //        if (time >= 0 && transition1 >= time)
    //        {
    //            return (jerk * Math.Pow(time, 2)) / 2;
    //        }
    //        else if (time >= transition1 && transition2 >= time)
    //        {
    //            return (Math.Pow(programmedAcceleration, 2) / (2 * jerk)) + (programmedAcceleration * (time - transition1));
    //        }
    //        else if (time >= transition2 && transition3 >= time)
    //        {
    //            return programmedVelocity - (jerk * Math.Pow(transition3 - time, 2)) / 2;
    //        }
    //        else if (time >= transition3 && transition4 >= time)
    //        {
    //            return programmedVelocity;
    //        }
    //        return 0;
    //    }
    //
    //    public double AccelerationDisplacementAtGivenTime(double time)
    //    {
    //        if (time >= 0 && transition1 >= time)
    //        {
    //            //Console.WriteLine("jerk: " + jerk);
    //            return (jerk * Math.Pow(time, 3)) / 6;
    //        }
    //        else if (time >= transition1 && transition2 >= time)
    //        {
    //            return (jerk * Math.Pow(transition1, 3)) / 6 +
    //                (programmedAcceleration * Math.Pow(time - transition1, 2) / 2) +
    //                (targetVelocity1 * (time - transition1));
    //        }
    //        else if (time >= transition2 && transition3 >= time)
    //        {
    //            //Console.WriteLine("jerk: " + (-1 * jerk));
    //            return (Math.Pow(programmedVelocity, 2) / (2 * averageAcceleration)) +
    //                ((jerk * Math.Pow(transition3 - time, 3)) / 6) -
    //                (programmedVelocity * (transition3 - time));
    //        }
    //        return 0;
    //    }
    //
    //    public double DecelerationDisplacementAtGivenTime(double time)
    //    {
    //        double altTime = time - transition4;
    //        double altTransition3 = transition7 - transition4;
    //        double altTransition2 = transition6 - transition4;
    //        double altTransition1 = transition5 - transition4;
    //
    //        double phase1 = (programmedVelocity * (altTransition1)) - ((jerk * Math.Pow(altTransition1, 3)) / 6);
    //        double phase2 = //(jerk * Math.Pow(altTransition1, 3)) / 6 +
    //                (programmedAcceleration * Math.Pow(altTransition2 - altTransition1, 2) / 2) +
    //                (targetVelocity1 * (altTransition2 - altTransition1));
    //        double phase3 = ((jerk * Math.Pow(transition7 - transition6, 3)) / 6);
    //
    //        if (time > transition4 && transition5 >= time)
    //        {
    //           return  (programmedVelocity * (altTime)) - ((jerk * Math.Pow(altTime, 3)) / 6);
    //        }
    //        else if (time >= transition5 && transition6 > time)
    //        {
    //            double currentStep = //(jerk * Math.Pow(altTransition1, 3)) / 6 +
    //                (programmedAcceleration * Math.Pow(altTransition2 - altTime, 2) / 2) +
    //                (targetVelocity1 * (altTransition2 - altTime));
    //            return phase1 + (phase2 - currentStep);
    //        }
    //        else if (time >= transition6 && transition7 >= time)
    //        {
    //
    //            //Console.WriteLine("jerk: " + (-decelerationJerk));
    //            double currentStep = ((jerk * Math.Pow(altTransition3 - altTime, 3)) / 6);
    //            //Console.WriteLine("PHASE 3: " + (transition7 - transition6));
    //            return phase1 + phase2 + (phase3 - currentStep);
    //        }
    //        return 0;
    //    }
    //
    //    public double GetDisplacementAtGivenTime(double time)
    //    {
    //        double multiplier = (finalPoint - initialPoint) / distanceTravelled;
    //        if (time <= transition3)
    //        {
    //            double accelOutput = AccelerationDisplacementAtGivenTime(time);
    //            return multiplier * accelOutput;
    //        }
    //        else if (time < transition4)
    //        {
    //            double constOutput = AccelerationDisplacementAtGivenTime(transition3) +
    //                programmedVelocity * (time - transition3);
    //            return multiplier * constOutput;
    //        }
    //        else if (time <= transition7)
    //        {
    //            double decelOutput = AccelerationDisplacementAtGivenTime(transition3) +
    //                programmedVelocity * (transition4 - transition3) +
    //                DecelerationDisplacementAtGivenTime(time);
    //            return multiplier * decelOutput;
    //        }
    //        else
    //        {
    //            double decelOutput = AccelerationDisplacementAtGivenTime(transition3) +
    //                programmedVelocity * (transition4 - transition3) +
    //                DecelerationDisplacementAtGivenTime(transition7);
    //            return multiplier * decelOutput;
    //        }
    //    }
    //}

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
                    new VectorD { 
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
    class Program
    {
        static void Main(string[] args)
        {
            #region no class test
            //MotionProfile mpX = new MotionProfile(100, 0, 100, 0);
            //MotionProfile mpY = new MotionProfile(100, 0, 100, 0);
            //MotionProfile mpZ = new MotionProfile(100, 0, 100, 0);
            //
            //double timeInterval = 0.1;
            //
            //List<string> lines = new List<string>();
            //
            //List<string> csvLines = new List<string>();
            //csvLines.Add("x, y, z, Vx, Vy, Vz, Ax, Ay, Az");
            //
            //double targetSpeed = 100;
            //
            //lines.Add("target speed: " + targetSpeed);
            //
            //double initX = 0;
            //double initY = 0;
            //double initZ = 0;
            //
            //lines.Add("initial position x: " + initX + " y: " + +initY + " z: " + initZ);
            //
            //double targetX = 300;
            //double targetY = 150;
            //double targetZ = 250;
            //
            //lines.Add("target position x: " + targetX + " y: " + +targetY + " z: " + targetZ);
            //
            //double distance = Math.Sqrt(Math.Pow(targetX, 2) + Math.Pow(targetY, 2) + Math.Pow(targetZ, 2));
            //double time = distance / targetSpeed;
            //
            //double speedX = Math.Abs(targetX - initX) / time;
            //double speedY = Math.Abs(targetY - initY) / time;
            //double speedZ = Math.Abs(targetZ - initZ) / time;
            //
            //List<double> posX = new List<double>();
            //List<double> posY = new List<double>();
            //List<double> posZ = new List<double>();
            //
            //mpX.initializeProgrammedParameters(speedX, 0, targetX, 0);
            //mpY.initializeProgrammedParameters(speedY, 0, targetY, 0);
            //mpZ.initializeProgrammedParameters(speedZ, 0, targetZ, 0);
            //
            //bool isTotalTimeEqual = mpX.totalTime == mpY.totalTime && mpY.totalTime == mpZ.totalTime;
            //
            //lines.Add("do all axis get to the target at the same time: " + isTotalTimeEqual);
            //
            //for (double i = 0; i < mpX.totalTime; i += timeInterval)
            //{
            //    posX.Add(mpX.GetDisplacementAtGivenTime(i));
            //    posY.Add(mpY.GetDisplacementAtGivenTime(i));
            //    posZ.Add(mpZ.GetDisplacementAtGivenTime(i));
            //}
            //
            //double initToTarget = GetDistance(initX, initY, initZ, targetX, targetY, targetZ);
            //lines.Add("Distance from init to target: " + initToTarget);
            //
            //for (int i = 0; i < posX.Count; i ++)
            //{
            //    double initToPointToTarget = GetDistance(initX, initY, initZ, posX[i], posY[i], posZ[i]) + GetDistance(targetX, targetY, targetZ, posX[i], posY[i], posZ[i]);
            //
            //    bool isInLine = (initToPointToTarget < (initToTarget + 0.000001)) && (initToPointToTarget > (initToTarget - 0.000001));
            //
            //    lines.Add("current position x: " + posX[i] + " y: " + +posY[i] + " z: " + posZ[i]);
            //
            //    double Vx;
            //    double Vy;
            //    double Vz;
            //    double Ax;
            //    double Ay;
            //    double Az;
            //
            //    if(i == 0)
            //    {
            //        Vx = 0;
            //        Vy = 0;
            //        Vz = 0;
            //
            //        Ax = 0;
            //        Ay = 0;
            //        Az = 0;
            //    }
            //    else if (i == 1)
            //    {
            //        Vx = (posX[i] - posX[i - 1]) / timeInterval;
            //        Vy = (posY[i] - posY[i - 1]) / timeInterval;
            //        Vz = (posZ[i] - posZ[i - 1]) / timeInterval;
            //
            //        double pVx = posX[i - 1] / timeInterval;
            //        double pVy = posY[i - 1] / timeInterval;
            //        double pVz = posZ[i - 1] / timeInterval;
            //
            //        Ax = (Vx - pVx) / timeInterval;
            //        Ay = (Vy - pVy) / timeInterval;
            //        Az = (Vz - pVz) / timeInterval;
            //    }
            //    else
            //    {
            //        Vx = (posX[i] - posX[i - 1]) / timeInterval;
            //        Vy = (posY[i] - posY[i - 1]) / timeInterval;
            //        Vz = (posZ[i] - posZ[i - 1]) / timeInterval;
            //
            //        double pVx = (posX[i - 1] - posX[i - 2]) / timeInterval;
            //        double pVy = (posY[i - 1] - posY[i - 2]) / timeInterval;
            //        double pVz = (posZ[i - 1] - posZ[i - 2]) / timeInterval;
            //
            //        Ax = (Vx - pVx) / timeInterval;
            //        Ay = (Vy - pVy) / timeInterval;
            //        Az = (Vz - pVz) / timeInterval;
            //    }
            //
            //    csvLines.Add(posX[i] + ", " + posY[i] + ", " + posZ[i] + ", " + Vx + ", " + Vy + ", " + Vz + ", " + Ax + ", " + Ay + ", " + Az);
            //
            //    lines.Add("Distance from init to point: " + GetDistance(initX, initY, initZ, posX[i], posY[i], posZ[i]));
            //    lines.Add("Distance from target to point: " + GetDistance(targetX, targetY, targetZ, posX[i], posY[i], posZ[i]));
            //    lines.Add("Distance from init to point + point to target: " + initToPointToTarget);
            //
            //    lines.Add("is point in straight line: " + isInLine);
            //    
            //}
            //
            //StreamWriter file = new StreamWriter("cpTest.txt");
            //
            //foreach (string line in lines)
            //{
            //    file.WriteLine(line);
            //}
            //file.Close();
            //
            //file = new StreamWriter("ptpMaxAccelerationPositions.csv");
            //
            //foreach (string line in csvLines)
            //{
            //    file.WriteLine(line);
            //}
            //file.Close();

            //Random rand = new Random();

            //for (double i = 0; i < mp.totalTime; i += 0.1)
            //{
            //    Console.WriteLine("time: " + i  + ", pos: " + mp.GetDisplacementAtGivenTime(i));
            //}
            #endregion no class test

            #region class testrue

            //NOTE: test different acceleration smoothness for the 
            //NOTE: Implement constant velocity mid points for CP
            //PROBLEM: moving straight from
            MotionTester ptpMotionTester;

            //ptpMotionTester = new MotionTester(100, 100, false);
            //ptpMotionTester.InitializeMotionProfiles();
            //ptpMotionTester.PerformMotion(0, 0, 0, 50, 40, 30, 100, 0.1);
            //ptpMotionTester.WriteCsvFile("ptpSingleShort100Accel100Velocity");

            ptpMotionTester = new MotionTester(100, 100, false);
            ptpMotionTester.InitializeMotionProfiles();
            ptpMotionTester.PerformMotion(0, 0, 0, 500, 400, 300, 100, 0.1, 70);
            ptpMotionTester.WriteCsvFile("ptpSingleDiagonal100Accel100Velocity70InitV");

            //ptpMotionTester = new MotionTester(100, 100, false);
            //ptpMotionTester.InitializeMotionProfiles();
            //ptpMotionTester.PerformMotion(0, 0, 0, 500, 0, 0, 100, 0.1);
            //ptpMotionTester.WriteCsvFile("ptpSingleStraightX100Accel100Velocity");
            //
            //ptpMotionTester = new MotionTester(100, 100, false);
            //ptpMotionTester.InitializeMotionProfiles();
            //ptpMotionTester.PerformMotion(0, 0, 0, 0, 500, 0, 100, 0.1);
            //ptpMotionTester.WriteCsvFile("ptpSingleStraightY100Accel100Velocity");
            //
            //ptpMotionTester = new MotionTester(100, 100, false);
            //ptpMotionTester.InitializeMotionProfiles();
            //ptpMotionTester.PerformMotion(0, 0, 0, 0, 0, 500, 100, 0.1);
            //ptpMotionTester.WriteCsvFile("ptpSingleStraightZ100Accel100Velocity");
            //
            //ptpMotionTester = new MotionTester(100, 100, false);
            //ptpMotionTester.InitializeMotionProfiles();
            //ptpMotionTester.PerformMotion(0, 0, 0, 400, 200, 100, 100, 0.1);
            //ptpMotionTester.PerformMotion(400, 200, 100, 500, 300, 200, 100, 0.1);
            //ptpMotionTester.PerformMotion(500, 300, 200, 500, 500, 500, 100, 0.1);
            //ptpMotionTester.WriteCsvFile("ptpConsecutive100Accel100Velocity");
            //
            //ptpMotionTester = new MotionTester(100, 100, true);
            //ptpMotionTester.InitializeMotionProfiles();
            //ptpMotionTester.PerformMotion(0, 0, 0, 300, 0, 0, 100, 0.1);
            //ptpMotionTester.WriteCsvFile("cpSinglePoint100Accel100Velocity");
            //
            //ptpMotionTester = new MotionTester(100, 100, true);
            //ptpMotionTester.InitializeMotionProfiles(MotionPhase.Start);
            //ptpMotionTester.PerformMotion(0, 0, 0, 400, 0, 0, 100, 0.1);
            //ptpMotionTester.InitializeMotionProfiles(MotionPhase.End);
            //ptpMotionTester.PerformMotion(400, 0, 0, 500, 0, 0, 100, 0.1);
            //ptpMotionTester.WriteCsvFile("cp2PointStraight100Accel100Velocity");
            //
            //ptpMotionTester = new MotionTester(100, 100, true);
            //ptpMotionTester.InitializeMotionProfiles(MotionPhase.Start);
            //ptpMotionTester.PerformMotion(0, 0, 0, 300, 0, 0, 100, 0.1);
            //ptpMotionTester.InitializeMotionProfiles(MotionPhase.Mid);
            //ptpMotionTester.PerformMotion(300, 0, 0, 400, 0, 0, 100, 0.1);
            //ptpMotionTester.InitializeMotionProfiles(MotionPhase.End);
            //ptpMotionTester.PerformMotion(400, 0, 0, 500, 0, 0, 100, 0.1);
            //ptpMotionTester.WriteCsvFile("cpMultiPointStraight100Accel100Velocity");
            //
            //ptpMotionTester = new MotionTester(100, 100, true);
            //ptpMotionTester.InitializeMotionProfiles();
            //ptpMotionTester.PerformMotion(0, 0, 0, 500, 0, 0, 100, 0.1);
            //ptpMotionTester.PerformMotion(500, 0, 0, 500, 500, 0, 100, 0.1);
            //ptpMotionTester.WriteCsvFile("cp2Point90DegreeTurn100Accel100Velocity");
            //
            //ptpMotionTester = new MotionTester(100, 100, true);
            //ptpMotionTester.InitializeMotionProfiles();
            //ptpMotionTester.PerformMotion(0, 0, 0, 500, 0, 0, 100, 0.1);
            //ptpMotionTester.PerformMotion(500, 0, 0, 500, 500, 0, 100, 0.1);
            //ptpMotionTester.PerformMotion(500, 500, 0, 0, 500, 0, 100, 0.1);
            //ptpMotionTester.WriteCsvFile("cpMultiPoint90DegreeTurn100Accel100Velocity");
            //
            //ptpMotionTester = new MotionTester(100, 100, true);
            //ptpMotionTester.InitializeMotionProfiles(MotionPhase.Start);
            //ptpMotionTester.PerformMotion(0, 0, 0, 300, 300, 0, 100, 0.1);
            //ptpMotionTester.InitializeMotionProfiles(MotionPhase.End);
            //ptpMotionTester.PerformMotion(300, 300, 0, 500, 300, 0, 100, 0.1);
            //ptpMotionTester.WriteCsvFile("cp2Point45DegreeTurn100Accel100Velocity");
            //
            //ptpMotionTester = new MotionTester(100, 100, true);
            //ptpMotionTester.InitializeMotionProfiles(MotionPhase.Start, MotionPhase.Start);
            //ptpMotionTester.PerformMotion(0, 0, 0, 300, 300, 0, 100, 0.1);
            //ptpMotionTester.InitializeMotionProfiles(MotionPhase.Mid, MotionPhase.Mid);
            //ptpMotionTester.PerformMotion(300, 300, 0, 500, 300, 0, 100, 0.1);
            //ptpMotionTester.InitializeMotionProfiles(MotionPhase.End, MotionPhase.End);
            //ptpMotionTester.PerformMotion(500, 300, 0, 300, 500, 0, 100, 0.1);
            //ptpMotionTester.WriteCsvFile("cp2Point45DegreeTurn100Accel100Velocity");

            #endregion class test
            Console.WriteLine("DONE!");
            Console.ReadLine();
        }
    }
}

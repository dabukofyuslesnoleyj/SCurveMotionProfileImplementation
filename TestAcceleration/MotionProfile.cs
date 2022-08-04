using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAcceleration
{
    /// <summary>
    /// A class that implements the S-Curve motion profile
    /// </summary>
    public class MotionProfile
    {
        protected double maxVelocity;
        protected double minVelocity;
        protected double maxAcceleration;
        protected double minAcceleration;

        protected double programmedVelocity;
        protected double averageAcceleration;
        protected double programmedAcceleration;

        protected double jerk;

        protected double transition1;
        protected double transition2;
        protected double transition3;
        protected double transition4;
        protected double transition5;
        protected double transition6;
        protected double transition7;

        private double targetVelocity1;
        private double targetVelocity2;

        public double TotalTime { get; protected set; }
        public double DistanceTraveled { get; protected set; }
        public double CurrentPoint { get; private set; }

        double currentVelocity;
        protected double initialPoint;
        protected double finalPoint;

        private double initV;
        private double accelJerk;
        private double accelProgrammedVelocity;
        private double accelTargetVelocity1;
        private double accelTargetVelocity2;
        private double accelProgrammedAcceleration;
        private double accelAverageAcceleration;


        /// <summary>
        /// The constructor of the MotionProfile class that requires the input of min and max velocity and acceleration
        /// </summary>
        /// <param name="maxVelocity">the maximum target velocity that can be inputted</param>
        /// <param name="minVelocity">the minimum target velocity that can be inputted</param>
        /// <param name="maxAcceleration">the maximum acceleration that can be inputted</param>
        /// <param name="minAcceleration">the minimum acceleration that can be inputted</param>
        public MotionProfile(double maxVelocity, double minVelocity, double maxAcceleration, double minAcceleration)
        {
            this.maxAcceleration = maxAcceleration;
            this.minAcceleration = minAcceleration;
            this.maxVelocity = maxVelocity;
            this.minVelocity = minVelocity;
        }

        /// <summary>
        /// a method that initializes the variables needed for computing the S-curve motion profile
        /// </summary>
        /// <param name="inputVelocity">the target velocity you are trying to get to</param>
        /// <param name="initialPosition">the position you start at</param>
        /// <param name="finalPosition">the position you are trying to get to</param>
        /// <param name="initialVelocity">in the case that you start at a non-zero velocity</param>
        /// <param name="accelerationSmoothness">decides how smooth the acceleration is or in other words if there will be points where acceleration is constant.
        /// NOTE: minimum value = 1.0 (Trapezoid: acceleration is always constant) maximum value = 2.0 (Perfect S-Curve: acceleration is never cosntant)</param>
        public virtual void InitializeMotionProfileParameters(double inputVelocity, double initialPosition, double finalPosition, double initialVelocity = 0, double accelerationSmoothness = 2.0)
        {
            initialPoint = initialPosition;
            CurrentPoint = initialPosition;
            finalPoint = finalPosition;
            DistanceTraveled = Math.Abs(finalPosition - initialPosition);

            //double accelerationSmoothness = 1.8;
            //if (DistanceTraveled < inputVelocity * 3)
            //    accelerationSmoothness = 1.5;
            //if (DistanceTraveled < inputVelocity * 2)
            //    accelerationSmoothness = 1.1;

            if (accelerationSmoothness > 2.0)
                accelerationSmoothness = 2.0;
            if (accelerationSmoothness < 1.0)
                accelerationSmoothness = 1.0;

            if (DistanceTraveled < inputVelocity)
                inputVelocity = DistanceTraveled;

            this.programmedVelocity = inputVelocity;
            if (this.programmedVelocity > maxVelocity)
                this.programmedVelocity = maxVelocity;

            this.programmedAcceleration = this.programmedVelocity;
            if (programmedAcceleration > maxAcceleration)
                programmedAcceleration = maxAcceleration;

            this.averageAcceleration = programmedAcceleration / accelerationSmoothness;
            currentVelocity = initialVelocity;

            //if (initialVelocity == 0)
                jerk = (Math.Pow(programmedAcceleration, 2) * averageAcceleration) /
                    (this.programmedVelocity * (programmedAcceleration - averageAcceleration));
            //else
            //    jerk = (Math.Pow(programmedAcceleration, 2) * averageAcceleration) /
            //        ((this.programmedVelocity - initialVelocity) * (programmedAcceleration - averageAcceleration));

            transition1 = programmedAcceleration / jerk;
            transition2 = (this.programmedVelocity / averageAcceleration) - (programmedAcceleration / jerk);
            transition3 = this.programmedVelocity / averageAcceleration;

            double accelerationDistance = (this.programmedVelocity / 2) * transition3;
            double constVelocityDistance = DistanceTraveled - (accelerationDistance * 2);

            transition4 = (constVelocityDistance / this.programmedVelocity) + transition3;
            transition5 = transition4 + transition1;
            transition6 = transition4 + transition2;
            transition7 = transition4 + transition3;

            TotalTime = transition4 + transition3;

            targetVelocity1 = Math.Pow(programmedAcceleration, 2) / (2 * jerk);
            targetVelocity2 = this.programmedVelocity - targetVelocity1;

            initV = initialVelocity;

            accelProgrammedVelocity = programmedVelocity - initialVelocity;
            accelProgrammedAcceleration = accelProgrammedVelocity;
            accelAverageAcceleration = accelProgrammedAcceleration / accelerationSmoothness;
            accelJerk = (Math.Pow(accelProgrammedAcceleration, 2) * accelAverageAcceleration) /
                (accelProgrammedVelocity * (accelProgrammedAcceleration - accelAverageAcceleration));
            accelTargetVelocity1 = Math.Pow(accelProgrammedAcceleration, 2) / (2 * accelJerk);
            accelTargetVelocity2 = accelProgrammedVelocity - accelTargetVelocity1;
        }

        /// <summary>
        /// The method outputs the current displacement based on the parameters given upon calling 
        /// InitializeMotionProfileParameters and the current time elapsed inputted
        /// </summary>
        /// <param name="time">The current time that elapsed since start of the motion.</param>
        /// <param name="point">the point at the given time</param>
        /// <returns>returns whether or not the action was successful</returns>
        public bool TryGetPointAtGivenTime(double time, out double point)
        {
            if (time <= TotalTime)
            {
                point = GetPointAtGivenTime(time);
                return true;
            }

            point = 0;
            return false;
        }

        /// <summary>
        /// The method outputs the current displacement based on the parameters given upon calling 
        /// InitializeMotionProfileParameters and the current time elapsed inputted
        /// </summary>
        /// <param name="time">The current time that elapsed since start of the motion.</param>
        /// <returns>the point at the given time</returns>
        public virtual double GetPointAtGivenTime(double time)
        {
            double multiplier = (finalPoint - initialPoint) / DistanceTraveled;
            if (time <= transition3)
            {
                double accelOutput = AccelerationDisplacementAtGivenTime(time);
                return multiplier * accelOutput;
            }
            else if (time < transition4)
            {
                double constOutput = AccelerationDisplacementAtGivenTime(transition3) +
                    programmedVelocity * (time - transition3);
                return multiplier * constOutput;
            }
            else if (time <= transition7)
            {
                double decelOutput = AccelerationDisplacementAtGivenTime(transition3) +
                    programmedVelocity * (transition4 - transition3) +
                    DecelerationDisplacementAtGivenTime(time);
                return multiplier * decelOutput;
            }
            else
            {
                double finalOutput = AccelerationDisplacementAtGivenTime(transition3) +
                    programmedVelocity * (transition4 - transition3) +
                    DecelerationDisplacementAtGivenTime(transition7);
                return multiplier * finalOutput;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        protected double AccelerationDisplacementAtGivenTime(double time)
        {
            if (time >= 0 && transition1 >= time)
            {
                //Console.WriteLine("jerk: " + jerk);
                return ((accelJerk * Math.Pow(time, 3)) / 6) + initV;
            }
            else if (time >= transition1 && transition2 >= time)
            {
                return ((accelJerk * Math.Pow(transition1, 3)) / 6 +
                    (accelProgrammedAcceleration * Math.Pow(time - transition1, 2) / 2) +
                    (accelTargetVelocity1 * (time - transition1))) + initV;
            }
            else if (time >= transition2 && transition3 >= time)
            {
                //Console.WriteLine("jerk: " + (-1 * jerk));
                return ((Math.Pow(accelProgrammedVelocity, 2) / (2 * accelAverageAcceleration)) +
                    ((accelJerk * Math.Pow(transition3 - time, 3)) / 6) -
                    (accelProgrammedVelocity * (transition3 - time))) + initV;
            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        protected double DecelerationDisplacementAtGivenTime(double time)
        {
            double altTime = time - transition4;
            double altTransition3 = transition7 - transition4;
            double altTransition2 = transition6 - transition4;
            double altTransition1 = transition5 - transition4;

            double phase1 = (programmedVelocity * altTransition1) - ((jerk * Math.Pow(altTransition1, 3)) / 6);
            double phase2 = (programmedAcceleration * Math.Pow(altTransition2 - altTransition1, 2) / 2) +
                    (targetVelocity1 * (altTransition2 - altTransition1));
            double phase3 = (jerk * Math.Pow(transition7 - transition6, 3)) / 6;

            if (time > transition4 && transition5 >= time)
            {
                return (programmedVelocity * (altTime)) - ((jerk * Math.Pow(altTime, 3)) / 6);
            }
            else if (time >= transition5 && transition6 > time)
            {
                double currentStep = (programmedAcceleration * Math.Pow(altTransition2 - altTime, 2) / 2) +
                    (targetVelocity1 * (altTransition2 - altTime));
                return phase1 + (phase2 - currentStep);
            }
            else if (time >= transition6 && transition7 >= time)
            {
                double currentStep = ((jerk * Math.Pow(altTransition3 - altTime, 3)) / 6);
                return phase1 + phase2 + (phase3 - currentStep);
            }

            return 0;
        }
    }
}

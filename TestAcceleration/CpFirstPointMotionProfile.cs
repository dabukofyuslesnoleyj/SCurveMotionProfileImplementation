using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAcceleration
{
    public class CpFirstPointMotionProfile : MotionProfile
    {
        public CpFirstPointMotionProfile(double maxVelocity, double minVelocity, double maxAcceleration, double minAcceleration) : 
            base(maxVelocity, minVelocity, maxAcceleration, minAcceleration)
        {
        }

        public override void InitializeMotionProfileParameters(double inputVelocity, double initialPosition, double finalPosition, double initialVelocity = 0)
        {
            base.InitializeMotionProfileParameters(inputVelocity, initialPosition, finalPosition, initialVelocity);
            
            if (inputVelocity > maxVelocity)
                inputVelocity = maxVelocity;

            programmedVelocity = inputVelocity;

            this.programmedAcceleration = this.programmedVelocity;
            if (programmedAcceleration > maxAcceleration)
                programmedAcceleration = maxAcceleration;

            this.averageAcceleration = programmedAcceleration / 1.1;

            if (initialVelocity == 0)
                jerk = (Math.Pow(programmedAcceleration, 2) * averageAcceleration) /
                    (this.programmedVelocity * (programmedAcceleration - averageAcceleration));
            else
                jerk = (Math.Pow(programmedAcceleration, 2) * averageAcceleration) /
                    ((this.programmedVelocity - initialVelocity) * (programmedAcceleration - averageAcceleration));

            double accelerationDistance = (this.programmedVelocity / 2) * transition3;
            double constVelocityDistance = DistanceTraveled - accelerationDistance;

            transition1 = programmedAcceleration / jerk;
            transition2 = (this.programmedVelocity / averageAcceleration) - (programmedAcceleration / jerk);
            transition3 = this.programmedVelocity / averageAcceleration;

            transition7 = (constVelocityDistance / this.programmedVelocity) + transition3;

            TotalTime = transition7;
        }

        public override double GetPointAtGivenTime(double time)
        {
            double multiplier = (finalPoint - initialPoint) / DistanceTraveled;
            if (time <= transition3)
            {
                double accelOutput = AccelerationDisplacementAtGivenTime(time);
                return multiplier * accelOutput;
            }
            else if (time < transition7)
            {
                double constOutput = AccelerationDisplacementAtGivenTime(transition3) +
                    programmedVelocity * (time - transition3);
                return multiplier * constOutput;
            }
            else
            {
                double constOutput = AccelerationDisplacementAtGivenTime(transition3) +
                    programmedVelocity * (transition7);
                return multiplier * constOutput;
            }
        }
    }
}

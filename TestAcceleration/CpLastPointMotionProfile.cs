using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAcceleration
{
    public class CpLastPointMotionProfile : MotionProfile
    {
        public CpLastPointMotionProfile(double maxVelocity, double minVelocity, double maxAcceleration, double minAcceleration) :
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

            double accelerationDistance = (programmedVelocity / 2) * transition3;
            double constVelocityDistance = DistanceTraveled - accelerationDistance;

            transition4 = constVelocityDistance / this.programmedVelocity;

            transition5 = transition4 + transition1;
            transition6 = transition4 + transition2;
            transition7 = transition4 + transition3;

            TotalTime = transition7;
        }

        public override double GetPointAtGivenTime(double time)
        {
            double multiplier = (finalPoint - initialPoint) / DistanceTraveled;
            if (time < transition4)
            {
                double constOutput = programmedVelocity * time;
                return multiplier * constOutput;
            }
            else if (time <= transition7)
            {
                double decelOutput = (programmedVelocity * transition4) +
                    DecelerationDisplacementAtGivenTime(time);
                return multiplier * decelOutput;
            }
            else
            {
                double finalOutput = (programmedVelocity * transition4) +
                    DecelerationDisplacementAtGivenTime(transition7);
                return multiplier * finalOutput;
            }
        }
    }
}

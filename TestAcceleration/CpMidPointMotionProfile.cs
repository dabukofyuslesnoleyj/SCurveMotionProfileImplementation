using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAcceleration
{
    public class CpMidPointMotionProfile : MotionProfile
    {
        public CpMidPointMotionProfile(double maxVelocity, double minVelocity, double maxAcceleration, double minAcceleration) :
            base(maxVelocity, minVelocity, maxAcceleration, minAcceleration)
        {
        }

        public override void InitializeMotionProfileParameters(double inputVelocity, double initialPosition, double finalPosition, double initialVelocity = 0)
        {
            base.InitializeMotionProfileParameters(inputVelocity, initialPosition, finalPosition, initialVelocity);
            
            if (inputVelocity > maxVelocity)
                inputVelocity = maxVelocity;

            programmedVelocity = inputVelocity;
            transition7 = DistanceTraveled / programmedVelocity;
            TotalTime = transition7;
        }

        public override double GetPointAtGivenTime(double time)
        {
            double multiplier = (finalPoint - initialPoint) / DistanceTraveled;
            if (time < transition7)
            {
                double constOutput = programmedVelocity * time;
                return multiplier * constOutput;
            }
            else
            {
                double constOutput = programmedVelocity * transition7;
                return multiplier * constOutput;
            }
        }
    }
}

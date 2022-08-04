# SCurveMotionProfileImplementation
An Implementation of the S-Curve MotionProfile on C#

I made this repository out of frustration that I coudn't find easily accessible implementations of the S-Curve Motion Profile.
This implementation is relatively simple to use and you only have to really take note of ```MotionProfile``` class. 
The class can calculate the <b>output positions</b> in an S-Curve Motion given the <b>input time</b>

A basic implementation on an XYZ coordinate system can be found in the ```MotionTester``` class if you want an example.
Here is an example code snippet of how to use ```MotionProfile```

```C#
double maxVelocity = 100;
double maxAcceleration = 50;

double targetVelocity = 50;
double initialPosition = 0;
double finalPosition = 500;

MotionProfile mp = new MotionProfile(maxVelocity, 0, maxAcceleration, 0);

mp.InitializeMotionProfileParameters(targetVelocity, initialPosition, finalPosition);

for(double timeElapsed = 0.0; timeElapsed < mp.TotalTime; i += 0.1)
  Console.Writeline(mp.GetPointAtGivenTime(i));
```

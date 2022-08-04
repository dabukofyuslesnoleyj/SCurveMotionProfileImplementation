using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TestAcceleration
{

    class Program
    {
        static void Main(string[] args)
        {
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brierley.TestAutomation.Core.Utilities;

namespace Hertz.API.Controllers
{
    public class RANUMController
    {
        private const int section1Max = 9999;
        private const int section2Max = 26;
        private const int section2MaxSize = 3;
        private const int section3Max = 99;

        private static RANUMController current = null;
        private int section1;
        private int[] section2 = new int[section2MaxSize];
        private int section3;


        public RANUMController()
        {
            section1 = StrongRandom.Next(0, 9000);
            for (int i = section2MaxSize - 1; i >= 0; i--)
                section2[i] = StrongRandom.Next(0, 25);
            section3 = StrongRandom.Next(0, 99);
        }



        public static string Generate()
        {
            current = RANUMController.current ?? new RANUMController();
            string output = String.Format("{0:D4}{1}{2:D2}", current.section1, section2String(), current.section3);
            RANUMController.incrementCurrent();
            return output;
        }
        private static void incrementCurrent()
        {
            if (current.section3 < 99) current.section3 += 1;
            else
            {
                current.section3 = 0;
                rollSection2();
            }
        }
        private static void rollSection2()
        {
            if (current.section2[section2MaxSize - 1] < section2Max) current.section2[2] += 1;
            else
            {
                current.section2[2] = 0;
                if (current.section2[1] < section2Max) current.section2[1] += 1;
                else
                {
                    if (current.section2[0] < section2Max) current.section2[0] += 1;
                    else rollSection1();
                }
            }
        }
        private static void rollSection1()
        {
            if (current.section1 < section1Max) current.section1 += 1;
            else throw new Exception("RANUM overflow");
        }
        private static string section2String()
        {
            return String.Format("{0}{1}{2}", Convert.ToChar(65 + current.section2[0]), Convert.ToChar(65 + current.section2[1]), Convert.ToChar(65 + current.section2[2]));
        }
    }
}

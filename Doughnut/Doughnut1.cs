using System;
using System.Text;
using System.Threading;
using System.Collections;
namespace Doughnut
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var output = new StringBuilder();
            int screenHight = 24;
            int screenWidth = 80;
            int R1 = 1; //The radius of the doughnuts tube
            int R2 = 2;//radius of the centre circle
            int K2 = 5;//how far away we are from the doughnut
            double a = 0; //The X angle increment
            double b = 0;//The Y angle increment
            double[] Depthsearch = new double[screenHight * screenWidth];
            string chars = " .,-~:;=!*#$@"; //The chars for lighting
            char[] Terminal = new char[screenHight * screenWidth];
            while (true)
            {
                var startTime = DateTime.Now;
                for(int  i = 0;i < Terminal.Length; i++) { Depthsearch[i] = 0; Terminal[i] = ' '; }

                for (double phi = 0; phi < Math.PI * 2; phi += 0.07)
                {
                    for (double theta = 0; theta < Math.PI * 2; theta += 0.02)
                    {
                        double sinA = Math.Sin(a), cosA = Math.Cos(a);
                        double sinB = Math.Sin(b), cosB = Math.Cos(b);
                        double sinTheta = Math.Sin(theta), cosTheta = Math.Cos(theta);
                        double sinPhi = Math.Sin(phi), cosPhi = Math.Cos(phi);

                        double circleX = R2 + R1 * cosTheta;//The whole circle
                        double circleY = R1 * sinTheta;//The circle in the centre
                                         //Rotations
                        double x = circleX * (cosB * cosPhi + sinA * sinB * sinPhi) - circleY * cosA * sinB;
                        double y = circleX * (sinB * cosPhi - cosB * sinA * sinPhi) + circleY * cosA * cosB;
                        double z = cosA * circleX * sinPhi + circleY * sinA + K2;

                        double zInverse = 1 / z;//Inverse of z used for depth and xp/yp calculations

                        int xp = (int)(screenWidth / 2 + 30 * x * zInverse);
                        int yp = (int)(screenHight / 2 - 15 * y * zInverse);
                        int pixlepoint = xp + screenWidth * yp;//The point where the pixle will be placed

                        double N = cosPhi * cosTheta * sinB - cosA * sinPhi * cosTheta - sinA * sinTheta + cosB * (cosA * sinTheta - sinPhi * cosTheta * sinA);
                        if (pixlepoint >= 0 && pixlepoint < screenWidth * screenHight && zInverse > Depthsearch[pixlepoint] && N > 0)
                        {
                            Depthsearch[pixlepoint] = zInverse;
                            int LuminanceIndex = (int)(N * 9);
                            Terminal[pixlepoint] = chars[Math.Max(0, Math.Min(LuminanceIndex, chars.Length - 1))];
                        }
                    }
                }
                output.Append("\u001b[38;2;225;200;0m");//Color
                for (int i = 0; i < Terminal.Length; i++)
                {
                    if (i % screenWidth == 0) output.Append("\n");
                    output.Append(Terminal[i]);
                }
               
                var elapsedTime = DateTime.Now - startTime;
                double fps = 1000.0 / elapsedTime.TotalMilliseconds;
                output.Append($"\n\u001b[38;2;0;225;0mFPS: {fps:F2}\u001b[0m");
                Console.Write(output);
                a += 0.07;
                b += 0.02;
                Thread.Sleep(40);
                output.Clear();
                Console.Write("\u001b[H");//Refresh
            }
        }
    }
}



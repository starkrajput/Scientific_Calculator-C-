using System;

namespace Trignometry
{
    public class Trigonometry
    {
        // Convert degrees to radians
        public static double DegreesToRadians(double degrees)
        {
            return Math.PI * degrees / 180.0;
        }

        // Convert radians to degrees
        public static double RadiansToDegrees(double radians)
        {
            return radians * 180.0 / Math.PI;
        }

        // Sine function
        public static double Sine(double angle)
        {
            return Math.Sin(angle);
        }

        // Cosine function
        public static double Cosine(double angle)
        {
            return Math.Cos(angle);
        }

        // Tangent function
        public static double Tangent(double angle)
        {
            return Math.Tan(angle);
        }

        // Cosecant function
        public static double Cosecant(double angle)
        {
            return 1 / Math.Sin(angle);
        }

        // Secant function
        public static double Secant(double angle)
        {
            return 1 / Math.Cos(angle);
        }

        // Cotangent function
        public static double Cotangent(double angle)
        {
            return 1 / Math.Tan(angle);
        }

        public static void Main()
        {
            // You can test your functions here if needed

        }
    }
}
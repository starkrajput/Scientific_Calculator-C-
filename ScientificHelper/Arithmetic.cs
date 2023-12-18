using System;

public class Arithmetic
{
    public static double SolveTwoVariable(double a, double b, char oper)
    {
        if (oper == '+')
        {
            return a + b;
        }
        else if (oper == '-')
        {
            return Math.Abs(a - b);
        }
        else if (oper == '*')
        {
            return a * b;
        }
        else if (oper == '/')
        {
            return a / b;
        }
        else if (oper == '%')
        {
            return a % b;
        }
        else if (oper == '^')
        {
            return (long)(Math.Pow((double)a, (double)b));
        }
        return 0;
    }
}

using System;
using System.Text;
using System.IO;
using System.Collections;
//using System.Linq.Expressions;
using System.Net.Http.Headers;
using ScientificHelper;
using System.Diagnostics;
using System.Data;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using NCalc;
using System.Numerics;

class Program
{
    static double solveExpression(String expression)
    {
        //[{(2+4)*2+5}+2^5+{(6*6)/6+32}]
        // Stack Approach :
        /*
        Stack<char> operatorStack = new Stack<char>();
        Stack<double> operandStack = new Stack<double>();

        for (int i = 0; i < expression.Length; i++)
        {
            if (expression[i] == '(' || expression[i] == '[' || expression[i] == '{')
            {
                operatorStack.Push(expression[i]);
            }
            else if ((expression[i] >= '0' && expression[i] <= '9') || expression[i] == '.')
            {
                StringBuilder valueBuilder = new StringBuilder();
                while (i < expression.Length && (char.IsDigit(expression[i]) || expression[i] == '.'))
                {
                    valueBuilder.Append(expression[i]);
                    i++;
                }

                if (double.TryParse(valueBuilder.ToString(), out double value))
                {
                    operandStack.Push(value);
                }
                else
                {
                    // Handle parsing error
                    throw new InvalidOperationException("Invalid numeric value");
                }

                i--;
            }
            else if (expression[i] == ')' || expression[i] == ']' || expression[i] == '}')
            {
                if (expression[i] == ')')
                {
                    while (operatorStack.Count > 0 && operatorStack.Peek() != '(')
                    {
                        double b = operandStack.Pop();
                        double a = operandStack.Pop();
                        char op = operatorStack.Pop();
                        operandStack.Push(Arithmetic.SolveTwoVariable(a, b, op));
                    }
                }
                else if (expression[i] == '}')
                {
                    while (operatorStack.Count > 0 && operatorStack.Peek() != '{')
                    {
                        double b = operandStack.Pop();
                        double a = operandStack.Pop();
                        char op = operatorStack.Pop();
                        operandStack.Push(Arithmetic.SolveTwoVariable(a, b, op));
                    }
                }
                else if (expression[i] == ']')
                {
                    while (operatorStack.Count > 0 && operatorStack.Peek() != '[')
                    {
                        double b = operandStack.Pop();
                        double a = operandStack.Pop();
                        char op = operatorStack.Pop();
                        operandStack.Push(Arithmetic.SolveTwoVariable(a, b, op));
                    }
                }
                if (operatorStack.Count > 0)
                {
                    operatorStack.Pop();
                }
            }
            else
            {
                while (operatorStack.Count > 0 && Precedence(expression[i]) <= Precedence(operatorStack.Peek()))
                {
                    double b = operandStack.Pop();
                    double a = operandStack.Pop();
                    char op = operatorStack.Pop();
                    operandStack.Push(Arithmetic.SolveTwoVariable(a, b, op));
                }
                operatorStack.Push(expression[i]);
            }
        }
        while (operatorStack.Count > 0)

        {
            double b = operandStack.Pop();
            double a = operandStack.Pop();
            char op = operatorStack.Pop();
            operandStack.Push(Arithmetic.SolveTwoVariable(a, b, op));
        }
        Console.WriteLine(operandStack.Peek());
        return operandStack.Peek();
        */
        //expression = expression.Replace("^", "**");
        if (expression[expression.Length-1] == '*' || expression[expression.Length-1] == '/') {
            String s = expression + "1";
             expression=s;
        } else if ( expression[expression.Length - 1] == '+' ||  expression[expression.Length - 1] == '-') {
            String s = expression + "0";
             expression=s;
        }
       
        expression = expression.Replace("^", "**");

        try
        {
            Expression e = new Expression(expression);
            object result = e.Evaluate();
            return Convert.ToDouble(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error evaluating expression: {ex.Message}");
           // while (true) { }
            return double.NaN; // Handle error as needed
        }
        // Convert the result to double
        //return Convert.ToDouble(result);
    }

    static string BalanceParentheses(string expression)
    {
        int openCount = expression.Count(c => c == '(');
        int closeCount = expression.Count(c => c == ')');

         for (int i = 0; i < openCount - closeCount; i++)
        {
            expression += ")";
        }

        return expression;
    }
    // Using Inheritance
    public class memoryExpressions : ArrayList
    {
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Object i in this)
            {
                sb.Append(i + " -> ");
            }
            return sb.ToString();
        }
    }


    public delegate void KeyPressedEventHandler(ConsoleKeyInfo keyInfo);
    public static event KeyPressedEventHandler KeyPressed;

    static StringBuilder expressionHistory = new StringBuilder();
    static StringBuilder expressionSolver = new StringBuilder();
    static ArrayList memoryExpression = new memoryExpressions();

    static StringBuilder History = new StringBuilder();
    static String Current = "Radian";
 
    static public void Layout()
    {
        Console.WriteLine("Number Keys:");
        Console.WriteLine("  7                                    4                                  1                                    0");
        Console.WriteLine("  8                                    5                                  2                                    .");
        Console.WriteLine("  9                                    6                                  3                                    =");
        Console.WriteLine("  /                                    *                                  -                                    +");
        

        Console.WriteLine("\nAdditional Keys:                     Additional Shift + Keys:                   Other Special Keys:" );
        Console.WriteLine("=================                      =========================                  ===================="); 
        Console.WriteLine("Ctrl + M: Secant                       Shift + S: 10^x                            Backspace: Remove Last");
        Console.WriteLine("Ctrl + P: Cosecant                                                                Escape: Close");                  
        Console.WriteLine("Ctrl + Q: Cotangent                    Shift + T: LogBase_10                      S: Abs");
        Console.WriteLine("Ctrl + R: Round Up                     Shift + O: LogBase_e                       T: Exp");
        Console.WriteLine("Ctrl + L: Round Down                   Shift + U: Sin                             O: Modulus (%)");
        Console.WriteLine("Ctrl + G: Random (1-100)               Shift + I: Cos                             U: Factorial");
        Console.WriteLine("Ctrl + S: Rad to Deg                   Shift + J: Tan                             I: (");
        Console.WriteLine("Ctrl + U: Deg to Rad                   Shift + A: Tanh                            J: )");
        Console.WriteLine("Ctrl + I: Use as is (GRAD)             Shift + B: Sinh                            M: 1/x");
        Console.WriteLine("Ctrl + J: Scientific Notation          Shift + C: Cosh                            N: Clear All");
        Console.WriteLine("Ctrl + Y: Negation                     Shift + D: Sech                            Q: x^2");
        Console.WriteLine("Ctrl + Z: F-E                          Shift + E: Cosech                          V: √x");
        Console.WriteLine("Ctrl + A: e                            Shift + F: Coth                            X: x^y");
        Console.WriteLine("Ctrl + B: .                            Shift + H: Toggle                          Up Arror: M+");
        Console.WriteLine("                                       Shift + Z: ASin                            Left Arror: M-");
        Console.WriteLine("                                       Shift + X: ACos                            Right Arrow: MS");
        Console.WriteLine("                                       Shift + Y: ATan                            Down Arrow: MR");
        Console.WriteLine("                                       Shift + V: ASec                            G: MC");
        Console.WriteLine("                                       Shift + W: ACosec                          B: TriRoot");
        Console.WriteLine("                                       Shift + P: ACot                            E: ACoth");
        Console.WriteLine("                                       Shift + Q: ASinh                           D: ASech   ");
        Console.WriteLine("                                       Shift + G: ACosh                            C: ACosech               ");
        Console.WriteLine("                                       Shift + K: ATanh                           F: PI               ");
        Console.WriteLine("                                                                               ");
        Console.WriteLine(); 
        Console.WriteLine("Input Validation -> Please use * before putting ( bracket. Also, Please ensure you are providing Valid Expression.");
    }

    static bool isPower = false;
    static void Main()
    {
       
            Console.Title = " Calculater ";
            //Console.WriteLine("Expression");
            String Expression = "(((2+4)*2+5)+2^5+((6*6)/6+32))";
            // Console.WriteLine(solveExpression(Expression));
            ConsoleKeyInfo cki;
            Console.TreatControlCAsInput = true;
            // Console.Write(" Scientific Calculater ");

            KeyPressed += HandleKeys;
            memoryExpression.Add("0");
            Console.WriteLine();
            Layout();
            while (true)
            {
                // Start a console read operation. Do not display the input.
                cki = Console.ReadKey(true);
                if (cki.Key == ConsoleKey.Escape) break;
                else if (((cki.Modifiers & ConsoleModifiers.Shift) != 0) || ((cki.Modifiers & ConsoleModifiers.Control) != 0) || (((cki.Modifiers & ConsoleModifiers.Shift) != 0) && ((cki.Modifiers & ConsoleModifiers.Control) != 0))) ;
                // Announce the name of the key that was pressed .
                KeyPressed?.Invoke(cki);
                //Console.WriteLine($"  Key pressed: {cki.Key}\t Modifiers pressed : {cki.Modifiers}.");

            }
       
        }
        

    static void HandleKeys(ConsoleKeyInfo keyInfo)
    {
        
            string specialCharecters = keyInfo.Modifiers.ToString();

            if (specialCharecters == "Control")
            {
                switch (keyInfo.Key)
                {
                    case ConsoleKey.M:
                    // sec
            
                    if (double.TryParse(expressionSolver.ToString(), out double intVaals))
                        {
                        if (Current == "Degree")
                        {
                            double er = (Convert.ToDouble(System.Math.PI) / 180) * (Convert.ToDouble(intVaals));
                            expressionSolver = new StringBuilder((1 / Math.Cos(er)).ToString());
                        }
                        else if (Current == "Radian")
                        {
                            //double er = Math.PI * (intVl / 180.0);
                            expressionSolver = new StringBuilder((1 / Math.Cos(Convert.ToDouble(intVaals))).ToString());
                        }
                        else if (Current == "Gradian")
                        {
                          
                            double radianValue = intVaals * (Math.PI / 200);

                            // Calculate Sin in gradient
                            // double resultInGradient = Math.Sin(radianValue) * (10.0 / 9.0);
                            expressionSolver = new StringBuilder((1 / Math.Cos(radianValue) * (10.0 / 9.0)).ToString());
                        }

                        //expressionSolver = new StringBuilder((1 / (Math.Cos(intVaals))).ToString());
                        History.Append("Sec(" + intVaals + ")");
                            Console.Clear();
                            Console.WriteLine(expressionHistory + "                            -> Current Expression");
                            Console.WriteLine(expressionSolver + "                             -> Current Variable");
                            Console.WriteLine(History + "                                      -> History ");
                            Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                            Layout();
                        }
                        break;
                    case ConsoleKey.P:
                        // csc
                        if (double.TryParse(expressionSolver.ToString(), out double intVaalss))
                        {
                        if (Current == "Degree")
                        {
                            double er = (Convert.ToDouble(System.Math.PI) / 180) * (Convert.ToDouble(intVaalss));
                            expressionSolver = new StringBuilder((1 / Math.Sin(er)).ToString());
                        }
                        else if (Current == "Radian")
                        {
                            //double er = Math.PI * (intVl / 180.0);
                            expressionSolver = new StringBuilder((1 / Math.Sin(Convert.ToDouble(intVaalss))).ToString());
                        }
                        else if (Current == "Gradian")
                        {

                            double radianValue = intVaalss * (Math.PI / 200);

                            // Calculate Sin in gradient
                            // double resultInGradient = Math.Sin(radianValue) * (10.0 / 9.0);
                            expressionSolver = new StringBuilder((1 / Math.Sin(radianValue) * (10.0 / 9.0)).ToString());
                        }

                        // expressionSolver = new StringBuilder((1 / (Math.Sin(intVaalss))).ToString());
                        History.Append("Cosec(" + intVaalss + ")");
                            Console.Clear();
                            Console.WriteLine(expressionHistory + "                            -> Current Expression");

                            Console.WriteLine(expressionSolver + "                             -> Current Variable");
                            Console.WriteLine(History + "                                      -> History ");
                            Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                            Layout();
                        }
                        break;
                    case ConsoleKey.Q:
                        //cot
                        if (double.TryParse(expressionSolver.ToString(), out double intVaalsss))
                        {
                        if (Current == "Degree")
                        {
                            double er = (Convert.ToDouble(System.Math.PI) / 180) * (Convert.ToDouble(intVaalsss));
                            expressionSolver = new StringBuilder((1 / Math.Tan(er)).ToString());
                        }
                        else if (Current == "Radian")
                        {
                            //double er = Math.PI * (intVl / 180.0);
                            expressionSolver = new StringBuilder((1 / Math.Tan(Convert.ToDouble(intVaalsss))).ToString());
                        }
                        else if (Current == "Gradian")
                        {

                            double radianValue = intVaalsss * (Math.PI / 200);

                            // Calculate Sin in gradient
                            // double resultInGradient = Math.Sin(radianValue) * (10.0 / 9.0);
                            expressionSolver = new StringBuilder((1 / Math.Tan(radianValue) * (10.0 / 9.0)).ToString());
                        }

                        //expressionSolver = new StringBuilder((1 / (Math.Tan(intVaalsss))).ToString());
                        History.Append("Cot(" + intVaalsss + ")");
                            Console.Clear();
                            Console.WriteLine(expressionHistory + "                            -> Current Expression");

                            Console.WriteLine(expressionSolver + "                             -> Current Variable");
                            Console.WriteLine(History + "                                      -> History ");
                            Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                            Layout();
                        }
                        break;
                    case ConsoleKey.R:
                        //ceil
                        if (double.TryParse(expressionSolver.ToString(), out double intVaalssss))
                        {
                            expressionSolver = new StringBuilder((Math.Ceiling(intVaalssss)).ToString());
                            History.Append("Ceil(" + intVaalssss + ")");
                            Console.Clear();
                            Console.WriteLine(expressionHistory + "                            -> Current Expression");

                            Console.WriteLine(expressionSolver + "                             -> Current Variable");
                            Console.WriteLine(History + "                                      -> History ");
                            Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                            Layout();
                        }
                        break;
                    case ConsoleKey.L:
                        //floor
                        if (double.TryParse(expressionSolver.ToString(), out double intVaalsssss))
                        {
                            expressionSolver = new StringBuilder((Math.Floor(intVaalsssss)).ToString());
                            History.Append("Floor(" + intVaalsssss + ")");
                            Console.Clear();
                            Console.WriteLine(expressionHistory + "                            -> Current Expression");

                            Console.WriteLine(expressionSolver + "                             -> Current Variable");
                            Console.WriteLine(History + "                                      -> History ");
                            Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                            Layout();
                        }
                        break;
                    case ConsoleKey.G:
                        //rand
                        expressionSolver.Append(new Random().Next(1, 100));
                        History.Append("Random()");
                        Console.Clear();
                        Console.WriteLine(expressionHistory + "                            -> Current Expression");

                        Console.WriteLine(expressionSolver + "                             -> Current Variable");
                        Console.WriteLine(History + "                                      -> History ");
                        Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                        Layout();
                        break;
                    case ConsoleKey.S:
                        //->To DEG
                        if (double.TryParse(expressionSolver.ToString(), out double inttVaalssss))
                        {
                            expressionSolver = new StringBuilder((Math.PI * inttVaalssss / 180.0).ToString());
                            History.Append("DEG(" + inttVaalssss + ")");
                            Console.Clear();
                            Console.WriteLine(expressionHistory + "                            -> Current Expression");

                            Console.WriteLine(expressionSolver + "                             -> Current Variable");
                            Console.WriteLine(History + "                                      -> History ");
                            Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                            Layout();
                        }
                        break;

                    case ConsoleKey.U:
                        // To RAD
                        if (double.TryParse(expressionSolver.ToString(), out double intttVaalssss))
                        {
                            expressionSolver = new StringBuilder((intttVaalssss * 180.0 / Math.PI).ToString());
                            History.Append("RAD(" + intttVaalssss + ")");
                            Console.Clear();
                            Console.WriteLine(expressionHistory + "                            -> Current Expression");

                            Console.WriteLine(expressionSolver + "                             -> Current Variable");
                            Console.WriteLine(History + "                                      -> History ");
                            Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                            Layout();
                        }
                        break;
                    case ConsoleKey.I:
                        // To  GRAD
                        if (double.TryParse(expressionSolver.ToString(), out double inttttVaalssss))
                        {
                            expressionSolver = new StringBuilder((inttttVaalssss = inttttVaalssss * 200.0 / Math.PI).ToString());
                            History.Append("GRAD(" + inttttVaalssss + ")");
                            Console.Clear();
                            Console.WriteLine(expressionHistory + "                            -> Current Expression");

                            Console.WriteLine(expressionSolver + "                             -> Current Variable");
                            Console.WriteLine(History + "                                      -> History ");
                            Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                            Layout();
                        }
                        break;
                    case ConsoleKey.J:
                        // e key
                        if (double.TryParse(expressionSolver.ToString(), out double intttttVaalssss))
                        {

                            string formattedNumber = intttttVaalssss.ToString($"F{2}");

                            // Convert the formatted number to scientific notation
                            string scientificNotation = $"{double.Parse(formattedNumber):0.##e+0}";
                            expressionSolver = new StringBuilder((formattedNumber).ToString());
                            History.Append("e(" + formattedNumber + ")");
                            Console.Clear();
                            Console.WriteLine(expressionHistory + "                            -> Current Expression");

                            Console.WriteLine(expressionSolver + "                             -> Current Variable");
                            Console.WriteLine(History + "                                      -> History ");
                            Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                            Layout();
                        }
                        break;



                    case ConsoleKey.Y:
                        // Negation
                        if (double.TryParse(expressionSolver.ToString(), out double suma))
                        {
                            expressionSolver = new StringBuilder(("-" + suma).ToString());
                            History.Append("NEG(" + suma + ")");
                            Console.Clear();
                            Console.WriteLine(expressionHistory + "                            -> Current Expression");

                            Console.WriteLine(expressionSolver + "                             -> Current Variable");
                            Console.WriteLine(History + "                                      -> History ");
                            Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                            Layout();
                        }
                        break;
                        //case ConsoleKey.N:
                        //    // ... add more Ctrl + Key combinations as needed
                        //    Console.WriteLine($"Ctrl + {keyInfo.Key} pressed");
                        break;
                    case ConsoleKey.Z:
                        // F-E
                        if (double.TryParse(expressionSolver.ToString(), out double sumaz))
                        {
                            expressionSolver = new StringBuilder(sumaz.ToString($"0.{new string('#', 8)}e+0"));
                            History.Append("F-E(" + sumaz.ToString("0.e+0") + ")");
                            Console.Clear();
                            Console.WriteLine(expressionHistory + "                            -> Current Expression");

                            Console.WriteLine(expressionSolver + "                             -> Current Variable");
                            Console.WriteLine(History + "                                      -> History ");
                            Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                            Layout();
                        }
                        break;
                    case ConsoleKey.A:
                        // e
                            expressionSolver = new StringBuilder((Math.E).ToString(""));
                            History.Append("e(" + (Math.E).ToString() + ")");
                            Console.Clear();
                            Console.WriteLine(expressionHistory + "                            -> Current Expression");
                            Console.WriteLine(expressionSolver + "                             -> Current Variable");
                            Console.WriteLine(History + "                                      -> History ");
                            Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                            Layout();
                        
                        break;
                    case ConsoleKey.B:
                        // .
                        expressionSolver.Append(".");
                        History.Append(".");
                        Console.Clear();
                        Console.WriteLine(expressionHistory + "                            -> Current Expression");
                        Console.WriteLine(expressionSolver + "                             -> Current Variable");
                        Console.WriteLine(History + "                                      -> History ");
                        Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                        Layout();

                        break;
                }
            }
            else if (specialCharecters == "Shift")
            {
                switch (keyInfo.Key)
                {
                    case ConsoleKey.S:
                        // 10x
                        if (double.TryParse(expressionSolver.ToString(), out double intValue))
                        {
                            expressionSolver = new StringBuilder((Math.Pow(10, intValue)).ToString());
                            History.Append("10^(" + intValue + ")");
                            Console.Clear();
                            Console.WriteLine(expressionHistory + "                            -> Current Expression");

                            Console.WriteLine(expressionSolver + "                             -> Current Variable");
                            Console.WriteLine(History + "                                      -> History ");
                            Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                            Layout();
                        }
                        break;
                    case ConsoleKey.T:
                        // log 
                        if (double.TryParse(expressionSolver.ToString(), out double intVale))
                        {
                            expressionSolver = new StringBuilder((Math.Log10(intVale)).ToString());
                            History.Append("log(" + intVale + ")");
                            Console.Clear();
                            Console.WriteLine(expressionHistory + "                            -> Current Expression");

                            Console.WriteLine(expressionSolver + "                             -> Current Variable");
                            Console.WriteLine(History + "                                      -> History ");
                            Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                            Layout();
                        }
                        break;
                    case ConsoleKey.O:
                        //ln
                        if (double.TryParse(expressionSolver.ToString(), out double intVal))
                        {
                            expressionSolver = new StringBuilder(((double)Math.ILogB(intVal)).ToString());
                            History.Append("ln(" + intVal + ")");
                            Console.Clear();
                            Console.WriteLine(expressionHistory + "                            -> Current Expression");

                            Console.WriteLine(expressionSolver + "                             -> Current Variable");
                            Console.WriteLine(History + "                                      -> History ");
                            Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                            Layout();
                        }
                        break;
                    case ConsoleKey.U:
                        // Sin
                        if (double.TryParse(expressionSolver.ToString(), out double intVl))
                        {
                        if (Current == "Degree")
                        {
                            double er = (Convert.ToDouble(System.Math.PI) / 180) * (Convert.ToDouble(intVl));
                            expressionSolver = new StringBuilder(( Math.Sin(er)).ToString());
                        }
                        else if (Current == "Radian")
                        {
                            //double er = Math.PI * (intVl / 180.0);
                            expressionSolver = new StringBuilder(( Math.Sin(Convert.ToDouble(intVl))).ToString());
                        }
                        else if (Current == "Gradian")
                        {

                            double radianValue = intVl * (Math.PI / 200);

                            // Calculate Sin in gradient
                            // double resultInGradient = Math.Sin(radianValue) * (10.0 / 9.0);
                            expressionSolver = new StringBuilder((Math.Sin(radianValue) ).ToString());
                        }

                        //expressionSolver = new StringBuilder((Math.Sin(intVl)).ToString());
                        History.Append("Sin(" + intVl + ")");
                            Console.Clear();
                            Console.WriteLine(expressionHistory + "                            -> Current Expression");

                            Console.WriteLine(expressionSolver + "                             -> Current Variable");
                            Console.WriteLine(History + "                                      -> History ");
                            Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                            Layout();
                        }
                        break;
                    case ConsoleKey.I:
                        // cos
                        if (double.TryParse(expressionSolver.ToString(), out double intVaal))
                        {
                        if (Current == "Degree")
                        {
                            double er = (Convert.ToDouble(System.Math.PI) / 180) * (Convert.ToDouble(intVaal));
                            expressionSolver = new StringBuilder((Math.Cos(er)).ToString());
                        }
                        else if (Current == "Radian")
                        {
                            //double er = Math.PI * (intVl / 180.0);
                            expressionSolver = new StringBuilder((Math.Cos(Convert.ToDouble(intVaal))).ToString());
                        }
                        else if (Current == "Gradian")
                        {

                            double radianValue = intVaal * (Math.PI / 200);

                            // Calculate Sin in gradient
                            // double resultInGradient = Math.Sin(radianValue) * (10.0 / 9.0);
                            expressionSolver = new StringBuilder((Math.Cos(radianValue) ).ToString());
                        }
                        // expressionSolver = new StringBuilder((Math.Cos(intVaal)).ToString());
                        History.Append("Cos(" + intVaal + ")");
                            Console.Clear();
                            Console.WriteLine(expressionHistory + "                            -> Current Expression");

                            Console.WriteLine(expressionSolver + "                             -> Current Variable");
                            Console.WriteLine(History + "                                      -> History ");
                            Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                            Layout();
                        }
                        break;
                    case ConsoleKey.J:
                        //tan
                        if (double.TryParse(expressionSolver.ToString(), out double intVaals))
                        {
                        if (Current == "Degree")
                        {
                            double er = (Convert.ToDouble(System.Math.PI) / 180) * (Convert.ToDouble(intVaals));
                            expressionSolver = new StringBuilder((Math.Tan(er)).ToString());
                        }
                        else if (Current == "Radian")
                        {
                            //double er = Math.PI * (intVl / 180.0);
                            expressionSolver = new StringBuilder((Math.Tan(Convert.ToDouble(intVaals))).ToString());
                        }
                        else if (Current == "Gradian")
                        {

                            double radianValue = intVaals * (Math.PI / 200);

                            // Calculate Sin in gradient
                            // double resultInGradient = Math.Sin(radianValue) * (10.0 / 9.0);
                            expressionSolver = new StringBuilder((Math.Tan(radianValue) ).ToString());
                        }
                        //expressionSolver = new StringBuilder((Math.Tan(intVaals)).ToString());
                        History.Append("Tan(" + intVaals + ")");
                            Console.Clear();
                            Console.WriteLine(expressionHistory + "                            -> Current Expression");

                            Console.WriteLine(expressionSolver + "                             -> Current Variable");
                            Console.WriteLine(History + "                                      -> History ");
                            Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                            Layout();
                        }
                        break;
                     case ConsoleKey.A:
                    //tanh
                    if (double.TryParse(expressionSolver.ToString(), out double intVaalsa))
                    {





                        if (Current == "Degree")
                        {
                            expressionSolver = new StringBuilder((Math.Tanh(Convert.ToDouble(intVaalsa))).ToString());
                        }
                        else if (Current == "Radian")
                        {
                            //double er = Math.PI * (intVl / 180.0);
                            expressionSolver = new StringBuilder((Math.Tanh(Convert.ToDouble(intVaalsa))).ToString());
                        }
                        else if (Current == "Gradian")
                        {

                            expressionSolver = new StringBuilder((Math.Tanh(Convert.ToDouble(intVaalsa))).ToString());
                        }
                        //expressionSolver = new StringBuilder((Math.Tanh(intVaalsa)).ToString());
                        History.Append("Tanh(" + intVaalsa + ")");
                        Console.Clear();
                        Console.WriteLine(expressionHistory + "                            -> Current Expression");

                        Console.WriteLine(expressionSolver + "                             -> Current Variable");
                        Console.WriteLine(History + "                                      -> History ");
                        Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                        Layout();
                    }
                    break;
                case ConsoleKey.B:
                    //sinh
                    if (double.TryParse(expressionSolver.ToString(), out double intVaalse))
                    {
                        if (Current == "Degree")
                        {
                            expressionSolver = new StringBuilder((Math.Sinh(Convert.ToDouble(intVaalse))).ToString());
                        }
                        else if (Current == "Radian")
                        {
                            //double er = Math.PI * (intVl / 180.0);
                            expressionSolver = new StringBuilder((Math.Sinh(Convert.ToDouble(intVaalse))).ToString());
                        }
                        else if (Current == "Gradian")
                        {

                            expressionSolver = new StringBuilder((Math.Sinh(Convert.ToDouble(intVaalse))).ToString());
                        }
                        //expressionSolver = new StringBuilder((Math.Sinh(intVaalse)).ToString());
                        History.Append("Sinh(" + intVaalse + ")");
                        Console.Clear();
                        Console.WriteLine(expressionHistory + "                            -> Current Expression");

                        Console.WriteLine(expressionSolver + "                             -> Current Variable");
                        Console.WriteLine(History + "                                      -> History ");
                        Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                        Layout();
                    }
                    break;
                case ConsoleKey.C:
                    //cosh
                    if (double.TryParse(expressionSolver.ToString(), out double intVaalis))
                    {
                        if (Current == "Degree")
                        {
                            expressionSolver = new StringBuilder((Math.Cosh(Convert.ToDouble(intVaalis))).ToString());
                        }
                        else if (Current == "Radian")
                        {
                            //double er = Math.PI * (intVl / 180.0);
                            expressionSolver = new StringBuilder((Math.Cosh(Convert.ToDouble(intVaalis))).ToString());
                        }
                        else if (Current == "Gradian")
                        {

                            expressionSolver = new StringBuilder((Math.Cosh(Convert.ToDouble(intVaalis))).ToString());
                        }
                        //expressionSolver = new StringBuilder((Math.Cosh(intVaalis)).ToString());
                        History.Append("Cosh(" + intVaalis + ")");
                        Console.Clear();
                        Console.WriteLine(expressionHistory + "                            -> Current Expression");

                        Console.WriteLine(expressionSolver + "                             -> Current Variable");
                        Console.WriteLine(History + "                                      -> History ");
                        Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                        Layout();
                    }
                    break;
                case ConsoleKey.D:
                    //sech
                    if (double.TryParse(expressionSolver.ToString(), out double intVaalsq))
                    {
                        if (Current == "Degree")
                        {
                            expressionSolver = new StringBuilder((1 / Math.Cosh(Convert.ToDouble(intVaalsq))).ToString());
                        }
                        else if (Current == "Radian")
                        {
                            //double er = Math.PI * (intVl / 180.0);
                            expressionSolver = new StringBuilder((1/Math.Cosh(Convert.ToDouble(intVaalsq))).ToString());
                        }
                        else if (Current == "Gradian")
                        {

                            expressionSolver = new StringBuilder((1 / Math.Cosh(Convert.ToDouble(intVaalsq))).ToString());
                        }
                        //expressionSolver = new StringBuilder((1/Math.Cosh(intVaalsq)).ToString());
                        History.Append("Sech(" + intVaalsq + ")");
                        Console.Clear();
                        Console.WriteLine(expressionHistory + "                            -> Current Expression");

                        Console.WriteLine(expressionSolver + "                             -> Current Variable");
                        Console.WriteLine(History + "                                      -> History ");
                        Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                        Layout();
                    }
                    break;
                case ConsoleKey.E:
                    //cosech
                    if (double.TryParse(expressionSolver.ToString(), out double intVaalws))
                    {
                        if (Current == "Degree")
                        {
                            expressionSolver = new StringBuilder((1 / Math.Cosh(Convert.ToDouble(intVaalws))).ToString());
                        }
                        else if (Current == "Radian")
                        {
                            //double er = Math.PI * (intVl / 180.0);
                            expressionSolver = new StringBuilder((1 / Math.Sinh(Convert.ToDouble(intVaalws))).ToString());
                        }
                        else if (Current == "Gradian")
                        {

                            expressionSolver = new StringBuilder((1 / Math.Cosh(Convert.ToDouble(intVaalws))).ToString());
                        }

                        //expressionSolver = new StringBuilder((1/Math.Sinh(intVaalws)).ToString());
                        History.Append("Cosech(" + intVaalws + ")");
                        Console.Clear();
                        Console.WriteLine(expressionHistory + "                            -> Current Expression");

                        Console.WriteLine(expressionSolver + "                             -> Current Variable");
                        Console.WriteLine(History + "                                      -> History ");
                        Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                        Layout();
                    }
                    break;
                case ConsoleKey.F:
                    //coth
                    if (double.TryParse(expressionSolver.ToString(), out double iintVaals))
                    {
                        if (Current == "Degree")
                        {
                            expressionSolver = new StringBuilder((1 / Math.Tanh(Convert.ToDouble(iintVaals))).ToString());
                        }
                        else if (Current == "Radian")
                        {
                            //double er = Math.PI * (intVl / 180.0);
                            expressionSolver = new StringBuilder((1 / Math.Tanh(Convert.ToDouble(iintVaals))).ToString());
                        }
                        else if (Current == "Gradian")
                        {

                            expressionSolver = new StringBuilder((1 / Math.Tanh(Convert.ToDouble(iintVaals))).ToString());
                        }

                        //expressionSolver = new StringBuilder((1/Math.Tanh(iintVaals)).ToString());
                        History.Append("Coth(" + iintVaals + ")");
                        Console.Clear();
                        Console.WriteLine(expressionHistory + "                            -> Current Expression");

                        Console.WriteLine(expressionSolver + "                             -> Current Variable");
                        Console.WriteLine(History + "                                      -> History ");
                        Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                        Layout();
                    }
                    break;
                case ConsoleKey.H:
                    if (Current == "Radian")
                    {
                        Current = "Degree";
                    }
                    else if (Current == "Degree")
                    {
                        Current = "Gradient";
                    }
                    else if(Current == "Gradient") {
                        Current = "Radian";
                     }
                    Console.WriteLine(Current+"- Current Sign");
                       
                       // Console.WriteLine($"Shift + {keyInfo.Key} pressed");
                        break;

                // Inside your switch statement
                case ConsoleKey.Z:
                    // asin
                    if (double.TryParse(expressionSolver.ToString(), out double asinValue))
                    {
                        double result = 0;
                        if (Current == "Degree")
                        {
                            result = (180 / Math.PI) * Math.Asin(asinValue);
                        }
                        else if (Current == "Radian")
                        {
                            result = Math.Asin(asinValue);
                        }
                        else if (Current == "Gradian")
                        {
                            double gradianValue = (200 / Math.PI) * Math.Asin(asinValue);
                            result = gradianValue * (Math.PI / 180);
                        }

                        expressionSolver = new StringBuilder(result.ToString());
                        History.Append("Asin(" + asinValue + ")");
                        Console.Clear();
                        Console.WriteLine(expressionHistory + "                            -> Current Expression");

                        Console.WriteLine(expressionSolver + "                             -> Current Variable");
                        Console.WriteLine(History + "                                      -> History ");
                        Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                        Layout();
                    }
                    break;

                case ConsoleKey.X:
                    // acos
                    if (double.TryParse(expressionSolver.ToString(), out double acosValue))
                    {
                        double result = 0;
                        if (Current == "Degree")
                        {
                            result = (180 / Math.PI) * Math.Acos(acosValue);
                        }
                        else if (Current == "Radian")
                        {
                            result = Math.Acos(acosValue);
                        }
                        else if (Current == "Gradian")
                        {
                            double gradianValue = (200 / Math.PI) * Math.Acos(acosValue);
                            result = gradianValue * (Math.PI / 180);
                        }

                        expressionSolver = new StringBuilder(result.ToString());
                        History.Append("Acos(" + acosValue + ")");
                        Console.Clear();
                        Console.WriteLine(expressionHistory + "                            -> Current Expression");

                        Console.WriteLine(expressionSolver + "                             -> Current Variable");
                        Console.WriteLine(History + "                                      -> History ");
                        Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                        Layout();
                    }
                    break;

                case ConsoleKey.Y:
                    // atan
                    if (double.TryParse(expressionSolver.ToString(), out double atanValue))
                    {
                        double result = 0;
                        if (Current == "Degree")
                        {
                            result = (180 / Math.PI) * Math.Atan(atanValue);
                        }
                        else if (Current == "Radian")
                        {
                            result = Math.Atan(atanValue);
                        }
                        else if (Current == "Gradian")
                        {
                            double gradianValue = (200 / Math.PI) * Math.Atan(atanValue);
                            result = gradianValue * (Math.PI / 180);
                        }

                        expressionSolver = new StringBuilder(result.ToString());
                        History.Append("Atan(" + atanValue + ")");
                        Console.Clear();
                        Console.WriteLine(expressionHistory + "                            -> Current Expression");

                        Console.WriteLine(expressionSolver + "                             -> Current Variable");
                        Console.WriteLine(History + "                                      -> History ");
                        Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                        Layout();
                    }
                    break;
                // Inside your switch statement
                case ConsoleKey.V:
                    // asec
                    if (double.TryParse(expressionSolver.ToString(), out double asecValue))
                    {
                        double result = 0;
                        if (Current == "Degree")
                        {
                            result = (180 / Math.PI) * Math.Acos(1 / asecValue);
                        }
                        else if (Current == "Radian")
                        {
                            result = Math.Acos(1 / asecValue);
                        }
                        else if (Current == "Gradian")
                        {
                            double gradianValue = (200 / Math.PI) * Math.Acos(1 / asecValue);
                            result = gradianValue * (Math.PI / 180);
                        }

                        expressionSolver = new StringBuilder(result.ToString());
                        History.Append("Asec(" + asecValue + ")");
                        Console.Clear();
                        Console.WriteLine(expressionHistory + "                            -> Current Expression");

                        Console.WriteLine(expressionSolver + "                             -> Current Variable");
                        Console.WriteLine(History + "                                      -> History ");
                        Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                        Layout();
                    }
                    break;

                case ConsoleKey.W:
                    // acsc
                    if (double.TryParse(expressionSolver.ToString(), out double acscValue))
                    {
                        double result = 0;
                        if (Current == "Degree")
                        {
                            result = (180 / Math.PI) * Math.Asin(1 / acscValue);
                        }
                        else if (Current == "Radian")
                        {
                            result = Math.Asin(1 / acscValue);
                        }
                        else if (Current == "Gradian")
                        {
                            double gradianValue = (200 / Math.PI) * Math.Asin(1 / acscValue);
                            result = gradianValue * (Math.PI / 180);
                        }

                        expressionSolver = new StringBuilder(result.ToString());
                        History.Append("Acsc(" + acscValue + ")");
                        Console.Clear();
                        Console.WriteLine(expressionHistory + "                            -> Current Expression");

                        Console.WriteLine(expressionSolver + "                             -> Current Variable");
                        Console.WriteLine(History + "                                      -> History ");
                        Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                        Layout();
                    }
                    break;

                    case ConsoleKey.P:
                    // acot
                    if (double.TryParse(expressionSolver.ToString(), out double acotValue))
                    {
                        double result = 0;
                        if (Current == "Degree")
                        {
                            result = (180 / Math.PI) * Math.Atan(1 / acotValue);
                        }
                        else if (Current == "Radian")
                        {
                            result = Math.Atan(1 / acotValue);
                        }
                        else if (Current == "Gradian")
                        {
                            double gradianValue = (200 / Math.PI) * Math.Atan(1 / acotValue);
                            result = gradianValue * (Math.PI / 180);
                        }

                        expressionSolver = new StringBuilder(result.ToString());
                        History.Append("Acot(" + acotValue + ")");
                        Console.Clear();
                        Console.WriteLine(expressionHistory + "                            -> Current Expression");

                        Console.WriteLine(expressionSolver + "                             -> Current Variable");
                        Console.WriteLine(History + "                                      -> History ");
                        Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                        Layout();
                    }
                    break;
                // Inside your switch statement
                case ConsoleKey.Q:
                    // asinh
                    if (double.TryParse(expressionSolver.ToString(), out double asinhValue))
                    {
                        double result = 0;
                        if (asinhValue >= 0)
                        {
                            result = Math.Log(asinhValue + Math.Sqrt((asinhValue * asinhValue) + 1));
                        }
                        else
                        {
                            Console.WriteLine("Invalid input for asinh. Please enter a non-negative value.");
                            Layout();
                            break;
                        }

                        expressionSolver = new StringBuilder(result.ToString());
                        History.Append("Asinh(" + asinhValue + ")");
                        Console.Clear();
                        Console.WriteLine(expressionHistory + "                            -> Current Expression");

                        Console.WriteLine(expressionSolver + "                             -> Current Variable");
                        Console.WriteLine(History + "                                      -> History ");
                        Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                        Layout();
                    }
                    break;

                case ConsoleKey.G:
                    // acosh
                    if (double.TryParse(expressionSolver.ToString(), out double acoshValue))
                    {
                        double result = 0;
                        if (acoshValue >= 1)
                        {
                            result = Math.Log(acoshValue + Math.Sqrt((acoshValue * acoshValue) - 1));
                        }
                        else
                        {
                            Console.WriteLine("Invalid input for acosh. Please enter a value greater than or equal to 1.");
                            Layout();
                            break;
                        }

                        expressionSolver = new StringBuilder(result.ToString());
                        History.Append("Acosh(" + acoshValue + ")");
                        Console.Clear();
                        Console.WriteLine(expressionHistory + "                            -> Current Expression");

                        Console.WriteLine(expressionSolver + "                             -> Current Variable");
                        Console.WriteLine(History + "                                      -> History ");
                        Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                        Layout();
                    }
                    break;

                case ConsoleKey.K:
                    // atanh
                    if (double.TryParse(expressionSolver.ToString(), out double atanhValue))
                    {
                        double result = 0;
                        if (Math.Abs(atanhValue) < 1)
                        {
                            result = 0.5 * Math.Log((1 + atanhValue) / (1 - atanhValue));
                        }
                        else
                        {
                            Console.WriteLine("Invalid input for atanh. Please enter a value with absolute value less than 1.");
                            Layout();
                            break;
                        }

                        expressionSolver = new StringBuilder(result.ToString());
                        History.Append("Atanh(" + atanhValue + ")");
                        Console.Clear();
                        Console.WriteLine(expressionHistory + "                            -> Current Expression");

                        Console.WriteLine(expressionSolver + "                             -> Current Variable");
                        Console.WriteLine(History + "                                      -> History ");
                        Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                        Layout();
                    }
                    break;


                    // Similar cases for inverse hyperbolic cosine (acosh), tangent (atanh), cotangent (acoth), 
                    // secant (asech), cosecant (acsch)

                    // Continue adding cases for other functions as needed

                    // Add cases for acot, asec, acosec similarly

            }
        }
            else if (specialCharecters == "Shift, Control")
            {
                switch (keyInfo.Key)
                {
                    case ConsoleKey.S:
                    case ConsoleKey.T:
                    case ConsoleKey.O:
                    case ConsoleKey.U:
                    case ConsoleKey.I:
                    case ConsoleKey.J:
                        // ... add more Ctrl + Key combinations as needed
                        Console.WriteLine($"Control + Shift + {keyInfo.Key} pressed");
                        break;
                }
            }
            else
            {
                // Handle other special keys
                switch (keyInfo.Key)
                {
                    case ConsoleKey.Add:
                    expressionHistory.Append(expressionSolver.ToString());
                    Console.Clear();
                    if (expressionHistory.Length == 0 && expressionSolver.Length==0)
                    {
                        expressionHistory.Append("0");
                    }
                    while(expressionHistory[expressionHistory.Length - 1] == '/' || expressionHistory[expressionHistory.Length - 1] == '+' || expressionHistory[expressionHistory.Length - 1] == '-' || expressionHistory[expressionHistory.Length - 1] == '*')
                    {
                        expressionHistory.Remove(expressionHistory.Length - 1, 1);
                    }
                    if ((expressionHistory.Length) == 0)
                    {
                        /*expressionHistory.Append("0+");
                        expressionSolver.Append("0+");
                        History.Append("0+");*/

                    }
                    else
                    {
                        expressionHistory.Append("+");
                        expressionSolver.Append("+");
                        //History.Append("+");
                    }
                    Console.WriteLine(expressionHistory + "                         -> Current Expression");
                        expressionSolver = new StringBuilder();
                        Console.WriteLine(expressionSolver + "                           -> Current Variable");
                        History.Append("+");
                        
                        
                        Console.WriteLine(History + "                                      -> History ");
                        Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                        Layout();
                        break;
                    case ConsoleKey.Subtract:
                        expressionHistory.Append(expressionSolver.ToString());
                        Console.Clear();
                    if (expressionHistory.Length == 0 && expressionSolver.Length == 0)
                    {
                        expressionHistory.Append("0");
                    }
                    while (( expressionHistory[expressionHistory.Length - 1] == '/' || expressionHistory[expressionHistory.Length - 1] == '+' || expressionHistory[expressionHistory.Length - 1] == '-' || expressionHistory[expressionHistory.Length - 1] == '*'))
                    {
                        expressionHistory.Remove(expressionHistory.Length - 1, 1);
                    }
                    if ((expressionHistory.Length ) == 0)
                    {
                        /*expressionHistory.Append("0-");
                        expressionSolver.Append("0-");
                        History.Append("0-");*/

                    }
                    else {
                        expressionHistory.Append("-");
                        expressionSolver.Append("-");
                        History.Append("-");
                    }
                        
                        Console.WriteLine(expressionHistory + "   -> Current Expression");
                        expressionSolver = new StringBuilder();
                        Console.WriteLine(expressionSolver + "    -> Current Variable");
                        Console.WriteLine(History + "                                      -> History ");
                        Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                        Layout();
                        break;

                    case ConsoleKey.Multiply:
                        expressionHistory.Append(expressionSolver.ToString());
                        Console.Clear();
                    if (expressionHistory.Length == 0 && expressionSolver.Length == 0) {
                        expressionHistory.Append("1");
                    }
                        while ( ( expressionHistory[expressionHistory.Length - 1] == '*' || expressionHistory[expressionHistory.Length - 1] == '+' || expressionHistory[expressionHistory.Length - 1] == '-' || expressionHistory[expressionHistory.Length - 1] == '/')) {
                            expressionHistory.Remove(expressionHistory.Length - 1,1);
                        }
                    if ((expressionHistory.Length) == 0)
                    {
                        /*expressionHistory.Append("*");
                        expressionSolver.Append("*");
                        History.Append("*");*/
                    }
                    else {
                        expressionHistory.Append("*");
                        expressionSolver.Append("*");
                        History.Append("*");
                    }
                    
                        Console.WriteLine(expressionHistory + "   -> Current Expression");
                        expressionSolver = new StringBuilder();
                        Console.WriteLine(expressionSolver + "    -> Current Variable");
                        Console.WriteLine(History + "                                      -> History ");
                        Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                        Layout();
                        break;
                    case ConsoleKey.Divide:
                        expressionHistory.Append(expressionSolver.ToString());
                        Console.Clear();
                    if (expressionHistory.Length == 0 && expressionSolver.Length == 0 )
                    {
                        expressionHistory.Append("1");
                    }
                    while (( expressionHistory[expressionHistory.Length - 1] == '/' || expressionHistory[expressionHistory.Length - 1] == '+' || expressionHistory[expressionHistory.Length - 1] == '-' || expressionHistory[expressionHistory.Length - 1] == '*'))
                     {
                            expressionHistory.Remove(expressionHistory.Length - 1, 1);
                     }
                    if ((expressionHistory.Length ) == 0)
                    {
                       /* expressionHistory.Append("*1/");
                        expressionSolver.Append("*1/");
                        History.Append("1/");*/
                    }
                    else {
                        expressionHistory.Append("/");
                        expressionSolver.Append("/");
                        History.Append("/");
                    }
  
                        Console.WriteLine(expressionHistory + "   -> Current Expression");
                        expressionSolver = new StringBuilder();
                        Console.WriteLine(expressionSolver + "    -> Current Variable");
                        
                        Console.WriteLine(History + "                                      -> History ");
                        Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                        Layout(); break;
                    case ConsoleKey.D0:
                        // expressionHistory.Append(0);
                        Console.Clear();
                    if (expressionSolver.Length == 1 && expressionSolver[0] == '0')
                    {
                        expressionSolver.Remove(0, 1);
                    }
                    expressionSolver.Append(0);
                        History.Append(0);
                        Console.WriteLine(expressionHistory + "   -> Current Expression");
                        Console.WriteLine(expressionSolver + "    -> Current Variable");
                        Console.WriteLine(History + "                                      -> History ");
                        Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                        Layout();
                        break;
                    case ConsoleKey.D1:
                        Console.Clear();
                    if (expressionSolver.Length == 1 && expressionSolver[0] == '0')
                    {
                        expressionSolver.Remove(0, 1);

                    }
                    // expressionHistory.Append(1);
                    expressionSolver.Append(1);
                        History.Append(1);
                        Console.WriteLine(expressionHistory + "   -> Current Expression");
                        Console.WriteLine(expressionSolver + "    -> Current Variable"); 
                        Console.WriteLine(History + "                                      -> History ");
                        Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                        Layout();
                        break;
                    case ConsoleKey.D2:
                        // expressionHistory.Append(2);
                        Console.Clear();
                    if (expressionSolver.Length == 1 && expressionSolver[0] == '0')
                    {
                        expressionSolver.Remove(0, 1);

                    }
                    expressionSolver.Append(2);
                        History.Append(2);
                        Console.WriteLine(expressionHistory + "   -> Current Expression");
                        Console.WriteLine(expressionSolver + "    -> Current Variable");
                        Console.WriteLine(History + "                                      -> History ");
                        Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                        Layout();
                        break;
                    case ConsoleKey.D3:
                        // expressionHistory.Append(3);
                        Console.Clear();
                    if (expressionSolver.Length == 1 && expressionSolver[0] == '0')
                    {
                        expressionSolver.Remove(0, 1);

                    }
                    expressionSolver.Append(3);
                        History.Append(3);
                        Console.WriteLine(expressionHistory + "   -> Current Expression");
                        Console.WriteLine(expressionSolver + "    -> Current Variable");
                        
                        Console.WriteLine(History + "                                      -> History ");
                        Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                        Layout();
                        break;
                    case ConsoleKey.D4:
                        // expressionHistory.Append(4);
                        Console.Clear();
                    if (expressionSolver.Length == 1 && expressionSolver[0] == '0')
                    {
                        expressionSolver.Remove(0, 1);

                    }
                    expressionSolver.Append(4);
                        History.Append(4);
                        Console.WriteLine(expressionHistory + "   -> Current Expression");
                        Console.WriteLine(expressionSolver + "    -> Current Variable");

                        Console.WriteLine(History + "                                      -> History ");
                        Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                        Layout();
                        break;
                    case ConsoleKey.D5:
                        // expressionHistory.Append(5);
                        Console.Clear();
                    if (expressionSolver.Length == 1 && expressionSolver[0] == '0')
                    {
                        expressionSolver.Remove(0, 1);

                    }
                    expressionSolver.Append(5);
                        History.Append(5);
                        Console.WriteLine(expressionHistory + "   -> Current Expression");
                        Console.WriteLine(expressionSolver + "    -> Current Variable");

                        Console.WriteLine(History + "                                      -> History ");
                        Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                        Layout();
                        break;
                    case ConsoleKey.D6:
                        // expressionHistory.Append(6);
                        Console.Clear();
                    if (expressionSolver.Length == 1 && expressionSolver[0] == '0')
                    {
                        expressionSolver.Remove(0, 1);

                    }
                    expressionSolver.Append(6);
                        History.Append(6);
                        Console.WriteLine(expressionHistory + "   -> Current Expression");
                        Console.WriteLine(expressionSolver + "    -> Current Variable");

                        Console.WriteLine(History + "                                      -> History ");
                        Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                        Layout();
                        break;
                    case ConsoleKey.D7:
                        //  expressionHistory.Append(7);
                        Console.Clear();
                    if (expressionSolver.Length == 1 && expressionSolver[0] == '0')
                    {
                        expressionSolver.Remove(0, 1);

                    }
                    expressionSolver.Append(7);
                        History.Append(7);
                        Console.WriteLine(expressionHistory + "   -> Current Expression");
                        Console.WriteLine(expressionSolver + "    -> Current Variable");

                        Console.WriteLine(History + "                                      -> History ");
                        Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                        Layout();
                        break;
                    case ConsoleKey.D8:
                        //  expressionHistory.Append(8);
                        Console.Clear();
                    if (expressionSolver.Length == 1 && expressionSolver[0] == '0')
                    {
                        expressionSolver.Remove(0, 1);

                    }
                    expressionSolver.Append(8);
                        History.Append(8);
                        Console.WriteLine(expressionHistory + "   -> Current Expression");
                        Console.WriteLine(expressionSolver + "    -> Current Variable");

                        Console.WriteLine(History + "                                      -> History ");
                        Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                        Layout();
                        break;
                    case ConsoleKey.D9:
                        //  expressionHistory.Append(9);
                        Console.Clear();
                    if (expressionSolver.Length == 1 && expressionSolver[0] == '0')
                    {
                        expressionSolver.Remove(0,1);

                    }
                    
                        expressionSolver.Append(9);
                    
                        History.Append(9);
                        Console.WriteLine(expressionHistory + "   -> Current Expression");
                        Console.WriteLine(expressionSolver + "    -> Current Variable");

                        Console.WriteLine(History + "                                      -> History ");
                        Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                        Layout();
                        break;
                    case ConsoleKey.OemPlus:
                    if (isPower == true)
                    {
                        expressionHistory.Append($"{expressionSolver.ToString()})");
                        isPower = false;
                    }
                    else {
                        expressionHistory.Append(expressionSolver.ToString());
                    }
                        //expressionHistory.Append(expressionSolver.ToString());
                        //Console.WriteLine(expressionHistory.ToString());
                        double ans = solveExpression(expressionHistory.ToString());
                        //Console.WriteLine(ans+"-----------------------ANS");
                        expressionHistory = new StringBuilder(ans.ToString());
                        expressionSolver = new StringBuilder(ans.ToString());
                        expressionHistory = new StringBuilder();
                        Console.Clear();
                        Console.WriteLine(expressionSolver + "   -> Current Expression");
                        Console.WriteLine(expressionSolver + "    -> Current Variable");
                        Console.WriteLine(History + "                                      -> History ");
                        Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                        Layout();
                        break;
                    case ConsoleKey.Backspace:
                        if (expressionSolver.Length > 0)
                        {
                            expressionSolver.Remove(expressionSolver.Length - 1, 1);
                        }
                        
                        Console.WriteLine(expressionHistory + "   -> Current Expression");
                        Console.WriteLine(expressionSolver + "    -> Current Variable");

                        Console.WriteLine(History + "                                      -> History ");
                        Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                        Layout();
                        break;
                    //case ConsoleKey.Delete:
                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        break;
                    //case ConsoleKey.F9:
                    //case ConsoleKey.R:
                    case ConsoleKey.DownArrow:
                        // Memory Recall
                        if (memoryExpression.Count > 0) {
                            expressionSolver = new StringBuilder(memoryExpression[(memoryExpression.Count - 1)].ToString());
                        }
                        Console.Clear();
                        Console.WriteLine(expressionHistory + "   -> Current Expression");
                        Console.WriteLine(expressionSolver + "    -> Current Variable");
                        Console.WriteLine(History + "                                      -> History ");
                        Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                        Layout();
                        break;
                    case ConsoleKey.UpArrow:
                        // M+
                        if (double.TryParse(expressionSolver.ToString(), out double intVale))
                        {
                           
                            double k = double.Parse((String)memoryExpression[(memoryExpression.Count - 1)]);
                            memoryExpression.RemoveAt(memoryExpression.Count - 1);
                            double anss = k + intVale;
                            memoryExpression.Add(anss.ToString());
                            //History.Append("log(" + intVale + ")");
                            Console.Clear();
                            Console.WriteLine(expressionHistory + "   -> Current Expression");
                            Console.WriteLine(expressionSolver + "    -> Current Variable");
                            Console.WriteLine(History + "                                      -> History ");
                            Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                            Layout();
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        // M-
                        if (double.TryParse(expressionSolver.ToString(), out double intValee))
                        {
                            double k = double.Parse((String)memoryExpression[(memoryExpression.Count - 1)]);
                            memoryExpression.RemoveAt(memoryExpression.Count - 1);

                            double anss = Math.Abs(k - intValee);
                            memoryExpression.Add(anss.ToString());
                            //History.Append("M-(" + intVale + ")");
                            Console.Clear();
                            Console.WriteLine(expressionHistory + "   -> Current Expression");
                            Console.WriteLine(expressionSolver + "    -> Current Variable");
                            Console.WriteLine(History + "                                      -> History ");
                            Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                            Layout();
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        // Memory Store 
                        if (double.TryParse(expressionSolver.ToString(), out double intValeee))
                        {
                           
                            memoryExpression.Add(intValeee.ToString());
                            //History.Append("log(" + intVale + ")");
                            Console.Clear();
                            Console.WriteLine(expressionHistory + "   -> Current Expression");
                            Console.WriteLine(expressionSolver + "    -> Current Variable");
                            Console.WriteLine(History + "                                      -> History ");
                            Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                            Layout();
                        }
                        break;
                    //case ConsoleKey.F3:
                    //case ConsoleKey.F4:
                    //case ConsoleKey.F5:
                    case ConsoleKey.G:
                        // Clear Memory
                        memoryExpression.RemoveRange(0,memoryExpression.Count);
                        memoryExpression.Add(0.ToString());
                        // pi
                        Console.Clear();
                        //expressionSolver.Append(Math.PI);
                        //History.Append(2);
                        Console.WriteLine(expressionHistory + "   -> Current Expression");
                        Console.WriteLine(expressionSolver + "    -> Current Variable");
                        Console.WriteLine(History + "                                      -> History ");
                        Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                        Layout();
                        break;
                    case ConsoleKey.S:
                        // absolute x
                        if (double.TryParse(expressionSolver.ToString(), out double intVl))
                        {
                            expressionSolver = new StringBuilder((Math.Abs(intVl)).ToString());
                            History.Append("Absolute(" + intVl + ")");
                            Console.Clear();
                            Console.WriteLine(expressionHistory + "   -> Current Expression");
                            Console.WriteLine(expressionSolver + "    -> Current Variable");
                            Console.WriteLine(History + "                                      -> History ");
                            Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                            Layout();
                        }
                        break;
                    case ConsoleKey.T:
                        //exp
                        if (double.TryParse(expressionSolver.ToString(), out double intVsl))
                        {
                            expressionSolver = new StringBuilder((Math.Exp(intVsl)).ToString());
                            History.Append("Exp(" + intVsl + ")");
                            Console.Clear();
                            Console.WriteLine(expressionHistory + "   -> Current Expression");
                            Console.WriteLine(expressionSolver + "    -> Current Variable");
                            Console.WriteLine(History + "                                      -> History ");
                            Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                            Layout();
                        }
                        break;
                    case ConsoleKey.O:
                        //modulus
                        expressionHistory.Append(expressionSolver.ToString());
                        Console.Clear();
                        expressionHistory.Append("%");
                        expressionSolver.Append("%");
                        History.Append("%");
                        Console.WriteLine(expressionHistory + "   -> Current Expression");
                        expressionSolver = new StringBuilder();
                        Console.WriteLine(expressionSolver + "    -> Current Variable");
                        Console.WriteLine(History + "                                      -> History ");
                        Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                        Layout();
                        break;
                    case ConsoleKey.U:
                    //n!
                    try
                    {
                        if (BigInteger.TryParse(expressionSolver.ToString(), out BigInteger intVssl))
                        {
                            expressionSolver = new StringBuilder((Factorial(intVssl)).ToString());
                            History.Append("Factorial(" + intVssl + ")");
                            Console.Clear();
                            Console.WriteLine(expressionHistory + "   -> Current Expression");

                            Console.WriteLine(expressionSolver + "    -> Current Variable");
                            Console.WriteLine(History + "                                      -> History ");
                            Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                            Layout();
                        }
                    }
                    catch  {
                        Console.WriteLine("Invalid Input. Enter System Predefined Bounded Values. ");
                        Main();
                    }
                        break;
                    case ConsoleKey.I:
                    //(
                   
                        expressionHistory.Append(expressionSolver.ToString());
                    if (expressionHistory[expressionHistory.Length - 1] == '/')
                    {
                        expressionHistory.Append("(");
                    }
                    else if (expressionHistory.Length == 0)
                    {
                        expressionHistory.Append("1*(");
                    }
                    else {
                        expressionHistory.Append("*(");
                    }
                    
                        Console.Clear();
                        Console.WriteLine(expressionHistory + "   -> Current Expression");
                        expressionSolver = new StringBuilder();
                        Console.WriteLine(expressionSolver + "    -> Current Variable");
                        Console.WriteLine(History + "                                      -> History ");
                        Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                        Layout();
                        break;
                    case ConsoleKey.J:
                        //)
                        expressionHistory.Append(expressionSolver.ToString());
                    if (expressionHistory[expressionHistory.Length - 1] == '(')
                    {
                        expressionHistory.Append("0)*");
                    }
                    else {
                        expressionHistory.Append(")*");
                    }
                       
                        Console.WriteLine(expressionHistory+"------------");
                        expressionSolver.Append(")*");
                        Console.Clear();
                        Console.WriteLine(expressionHistory + "   -> Current Expression");
                        expressionSolver = new StringBuilder();
                        Console.WriteLine(expressionSolver + "    -> Current Variable");
                        Console.WriteLine(History + "                                      -> History ");
                        Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                        Layout();

                        break;
                    case ConsoleKey.M:
                        //1/x
                        if (double.TryParse(expressionSolver.ToString(), out double intVsssl))
                        {
                            expressionSolver = new StringBuilder((1 / (intVsssl)).ToString());
                            History.Append("1/x -> (" + intVsssl + ")");
                            Console.Clear();
                            Console.WriteLine(expressionHistory + "   -> Current Expression");

                            Console.WriteLine(expressionSolver + "    -> Current Variable");
                            Console.WriteLine(History + "                                      -> History ");
                            Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                            Layout();
                        }
                        break;
                    case ConsoleKey.N:
                        // clear
                        Console.Clear();
                        expressionHistory = new StringBuilder();
                        expressionSolver = new StringBuilder();
                        Console.WriteLine(History + "                                      -> History ");
                        Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                        Layout();
                        break;
                 
                    case ConsoleKey.Q:
                        // x2
                        if (double.TryParse(expressionSolver.ToString(), out double intVssssl))
                        {
                            expressionSolver = new StringBuilder(Math.Pow(intVssssl, 2).ToString());
                            History.Append("\n x^2 -> (" + intVssssl + ")");
                            Console.Clear();
                            Console.WriteLine(expressionHistory + "   -> Current Expression");

                            Console.WriteLine(expressionSolver + "    -> Current Variable");
                            Console.WriteLine(History + "                                      -> History ");
                            Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                            Layout();
                        }
                        break;
                    case ConsoleKey.V:
                        // UnderRoot
                        if (double.TryParse(expressionSolver.ToString(), out double intVssssle))
                        {
                            expressionSolver = new StringBuilder(Math.Pow(intVssssle, 0.5).ToString());
                            History.Append("\n UnderRoot -> (" + intVssssle + ")");
                            Console.Clear();
                            Console.WriteLine(expressionHistory + "   -> Current Expression");

                            Console.WriteLine(expressionSolver + "    -> Current Variable");
                            Console.WriteLine(History + "                                    -> History   ");
                            Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");

                            Layout();

                        }
                        break;
                    case ConsoleKey.X:
                        //x power y
                        expressionHistory.Append($"Pow({expressionSolver.ToString()},");
                        Console.Clear();
                        isPower = true;
                        //expressionHistory.Append("^");
                        expressionSolver.Append("^");
                        History.Append("^");
                        Console.WriteLine(expressionHistory + "   -> Current Expression");
                        expressionSolver = new StringBuilder();
                        Console.WriteLine(expressionSolver + "    -> Current Variable");
                        Console.WriteLine(History + "                                    -> History");
                        Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                        Layout();
                        break;
                    case ConsoleKey.A:
                    //Toggle
                   
                        Console.WriteLine(Current);
                        Console.WriteLine(expressionHistory + "   -> Current Expression");
                        expressionSolver = new StringBuilder();
                        Console.WriteLine(expressionSolver + "    -> Current Variable");
                        Console.WriteLine(History + "                                    -> History");
                        Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                        Layout();
                        break;
                    case ConsoleKey.B:
                    // Triple Root
                    
                    if (double.TryParse(expressionSolver.ToString(), out double intVsssssle))
                    {
                        expressionSolver = new StringBuilder((Math.Pow(intVsssssle, (double)1 / 3)).ToString());
                        History.Append("\n UnderTriRoot -> (" + intVsssssle + ")");
                        Console.Clear();
                        Console.WriteLine(expressionHistory + "   -> Current Expression");

                        Console.WriteLine(expressionSolver + "    -> Current Variable");
                        Console.WriteLine(History + "                                    -> History   ");
                        Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");

                        Layout();

                    }
                    break;
                // Inside your switch statement
                case ConsoleKey.C:
                    // acsch
                    if (double.TryParse(expressionSolver.ToString(), out double acschValue))
                    {
                        double result = 0;
                        if (Math.Abs(acschValue) > 0)
                        {
                            result = Math.Log(1 / acschValue + Math.Sqrt(1 / (acschValue * acschValue) + 1));
                        }
                        else
                        {
                            Console.WriteLine("Invalid input for acsch. Please enter a value other than 0.");
                            Layout();
                            break;
                        }

                        expressionSolver = new StringBuilder(result.ToString());
                        History.Append("Acsch(" + acschValue + ")");
                        Console.Clear();
                        Console.WriteLine(expressionHistory + "                            -> Current Expression");

                        Console.WriteLine(expressionSolver + "                             -> Current Variable");
                        Console.WriteLine(History + "                                      -> History ");
                        Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                        Layout();
                    }
                    break;

                case ConsoleKey.D:
                    // asech
                    if (double.TryParse(expressionSolver.ToString(), out double asechValue))
                    {
                        double result = 0;
                        if (asechValue >= 1)
                        {
                            result = Math.Log(1 / asechValue + Math.Sqrt(1 / (asechValue * asechValue) - 1));
                        }
                        else
                        {
                            Console.WriteLine("Invalid input for asech. Please enter a value greater than or equal to 1.");
                            Layout();
                            break;
                        }

                        expressionSolver = new StringBuilder(result.ToString());
                        History.Append("Asech(" + asechValue + ")");
                        Console.Clear();
                        Console.WriteLine(expressionHistory + "                            -> Current Expression");

                        Console.WriteLine(expressionSolver + "                             -> Current Variable");
                        Console.WriteLine(History + "                                      -> History ");
                        Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                        Layout();
                    }
                    break;

                case ConsoleKey.E:
                    // acoth
                    if (double.TryParse(expressionSolver.ToString(), out double acothValue))
                    {
                        double result = 0;
                        if (Math.Abs(acothValue) > 1)
                        {
                            result = 0.5 * Math.Log((acothValue + 1) / (acothValue - 1));
                        }
                        else
                        {
                            Console.WriteLine("Invalid input for acoth. Please enter a value with absolute value greater than 1.");
                            Layout();
                            break;
                        }

                        expressionSolver = new StringBuilder(result.ToString());
                        History.Append("Acoth(" + acothValue + ")");
                        Console.Clear();
                        Console.WriteLine(expressionHistory + "                            -> Current Expression");

                        Console.WriteLine(expressionSolver + "                             -> Current Variable");
                        Console.WriteLine(History + "                                      -> History ");
                        Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                        Layout();
                    }
                    break;
                case ConsoleKey.F:
                    // e
                    expressionSolver = new StringBuilder(((int)Math.PI).ToString(""));
                    History.Append("PI(" + (Math.PI).ToString() + ")");
                    Console.Clear();
                    Console.WriteLine(expressionHistory + "                            -> Current Expression");
                    Console.WriteLine(expressionSolver + "                             -> Current Variable");
                    Console.WriteLine(History + "                                      -> History ");
                    Console.WriteLine(memoryExpression.ToString() + "                             -> Memory ");
                    Layout();

                    break;

            }
        }
        }
        

   












    //---------------------------------------------------------------------------------------------------------------------------
    static BigInteger Factorial(BigInteger n)
    {
        try
        {
            if (n < 0)
            {
                throw new ArgumentException("Input must be a non-negative integer.", nameof(n));
            }

            if (n == 0 || n == 1)
            {
                return 1;
            }

            BigInteger result = 1;
            for (int i = 2; i <= n; i++)
            {
                result *= i;
            }

            return result;
        }
        catch (Exception e) {
            Console.WriteLine("Invalid Input. Enter System Predefined Bounded Values. ");
            Main();
        }
        return 0;
    }

    
    static int Precedence(char Operand)
        {
            if (Operand == '+' || Operand == '-')
            {
                return 1;
            }
            if (Operand == '*' || Operand == '/' || Operand == '^' || Operand=='%') { return 2; }
            return 0;
        }
        


        /*static int SolveBrackets(String subExpression)
        {
            //3 + 2222 * 336 / 3 + 24 * 2 +/ 2 * 8
            string currentstring = "";
            int ni = 0;
            int nj = 0;
            string firstdigit = "";
            string seconddigit = "";
            while (ni < subExpression.Length)
            {
                firstdigit += subExpression[ni];
                if (subExpression[ni] == '*' || subExpression[ni] == '/')
                {

                }
                if (subExpression[ni] == '+' || subExpression[ni] == '-')
                {

                }
            }
            return 0;
        }*/
    }



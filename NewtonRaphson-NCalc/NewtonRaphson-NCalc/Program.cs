using NCalc;
using System;
using System.Linq.Expressions;

namespace NRM
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter function f(x): ");
            string function = Console.ReadLine();

            Console.Write("Enter initial guess X0: ");
            if (!double.TryParse(Console.ReadLine(), out double x0))
            {
                Console.WriteLine("Invalid number. Program stopped.");
                return;
            }

            NewtonRaphson(function, x0);
        }

        static void NewtonRaphson(string functionText, double x0)
        {
            int maxIter = 20;
            double tol = 1e-5;
            double x = x0;

            Console.WriteLine();
            Console.WriteLine("n\t   x\t\t   f(x)\t\t   f'(x)");
            Console.WriteLine("----------------------------------------------------------");

            for (int n = 1; n <= maxIter; n++)
            {
                double fx, dfx;

                try
                {
                    fx = F(functionText, x);
                    dfx = D(functionText, x);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error evaluating function: {ex.Message}");
                    return;
                }

                Console.WriteLine($"{n}\t{x:F6}\t{fx:F6}\t{dfx:F6}");

                if (Math.Abs(dfx) < 1e-12)
                {
                    Console.WriteLine("Derivative too small. Cannot continue.");
                    return;
                }

                double xNew = x - fx / dfx;

                if (double.IsNaN(xNew) || double.IsInfinity(xNew))
                {
                    Console.WriteLine("Values became unstable. Stopping.");
                    return;
                }

                if (Math.Abs(xNew - x) < tol)
                {
                    x = xNew;
                    break;
                }

                x = xNew;
            }

            Console.WriteLine();
            Console.WriteLine($"Approximate root: x = {x:F6}");
        }

        static double F(string functionText, double x)
        {
            var expr = new NCalc.Expression(functionText);

            expr.Parameters["x"] = x;
            object result = expr.Evaluate();
            return Convert.ToDouble(result);
        }

        static double D(string functionText, double x)
        {
            double h = 1e-6;
            double fplus = F(functionText, x + h);
            double fminus = F(functionText, x - h);
            return (fplus - fminus) / (2 * h);
        }
    }
}

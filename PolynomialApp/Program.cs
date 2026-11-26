using System;
using PolynomialLibrary;

namespace PolynomialApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            
            Polynomial pZero = new Polynomial();                       
            Polynomial p1 = new Polynomial(1, -2, 3);                  
            Polynomial p2 = new Polynomial(4, 1);                      
            Polynomial pCopy = new Polynomial(p1);                     

            Console.WriteLine($"pZero = {pZero}");
            Console.WriteLine($"p1    = {p1} (Ступінь: {p1.Degree})");
            Console.WriteLine($"p2    = {p2} (Ступінь: {p2.Degree})");
            Console.WriteLine($"pCopy = {pCopy}\n");

            
            Console.WriteLine($"p1 + p2   = {p1 + p2}");
            Console.WriteLine($"p1 - p2   = {p1 - p2}");
            Console.WriteLine($"p1 * p2   = {p1 * p2}");
            Console.WriteLine($"p1 + 5    = {p1 + 5}");
            Console.WriteLine($"5 + p1    = {5 + p1}");
            Console.WriteLine($"p1 * 2.5  = {p1 * 2.5}");
            Console.WriteLine($"2.5 * p1  = {2.5 * p1}\n");

            
            Console.WriteLine($"Коефіцієнт p1[2] = {p1[2]}");
            double x = 2.0;
            Console.WriteLine($"Значення p1({x}) = {p1[x]}\n");

            
            Console.WriteLine($"p1 == pCopy : {p1 == pCopy}");
            Console.WriteLine($"p1 != p2    : {p1 != p2}");
            Console.WriteLine($"p1 > p2     : {p1 > p2}");
            Console.WriteLine($"p1 < p2     : {p1 < p2}\n");

            
            if (p1)
                Console.WriteLine("p1 є ненульовим поліномом (true).");
            if (pZero)
                Console.WriteLine("pZero є ненульовим.");
            else
                Console.WriteLine("pZero є нульовим поліномом (false).\n");

            
            Polynomial pFromDouble = 7.5;          
            double freeTerm = (double)p1;          
            Console.WriteLine($"Неявне приведення 7.5 -> Polynomial: {pFromDouble}");
            Console.WriteLine($"Явне приведення (double)p1 (вільний член): {freeTerm}\n");

            
            Console.WriteLine("--- Введення власного полінома ---");
            Polynomial userPoly = new Polynomial();
            userPoly.Input();
            Console.WriteLine($"Ви ввели: {userPoly}");
            Console.WriteLine($"Значення при x = 1: {userPoly[1.0]}");

            Console.ReadKey();
        }
    }
}
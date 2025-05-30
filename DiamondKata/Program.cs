// See https://aka.ms/new-console-template for more information
using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Enter an uppercase letter (A-Z):");
        var input = Console.ReadLine();
        if (!string.IsNullOrEmpty(input) && input.Length == 1 && char.IsUpper(input[0]))
        {
            var diamond = Diamond.Create(input[0]);
            foreach (var line in diamond)
            {
                Console.WriteLine(line);
            }
        }
        else
        {
            Console.WriteLine("Invalid input.");
        }
    }
}

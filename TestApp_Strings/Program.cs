using System;

namespace TestApp_Strings
{
    class Program
    {
        static void Main(string[] args)
        {
            string s1 = "test text \" and , end";
            char[] ca1 = s1.ToCharArray();
            foreach (char a in s1)
            {
                Console.WriteLine(a);
            }
            Console.ReadKey();
        }
    }
}

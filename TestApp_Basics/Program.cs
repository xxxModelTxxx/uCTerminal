using System;
using System.Collections.Generic;

namespace TestApp_Basics
{
    class Program
    {
        static void Main(string[] args)
        {

        }

        public class TestClass<T>
        {
            T temp;

            public TestClass(T input)
            {
                temp = input ?? default
            }
        }

    }
}

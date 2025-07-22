using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NationalBank.BackEnd.Models
{
    public class TestModel
    {
        protected internal string Name { get; set; }
    }

    public class TestModel2
    {
        public string Description { get; set; }
        TestModel te = new TestModel();
        public TestModel2()
        {
            string gg = te.Name;    
        }
    }
    public class MainClass
    {
        public delegate int MyDelegate(int a,int b);

        int Sum(int a, int b) => a + b;
        int Multiply(int a, int b) => a * b;    
        int divide(int a, int b) => a / b;  

         void MainMethod()
        {
            MyDelegate myDelegate;

            myDelegate = Sum;
            myDelegate += Multiply;
            myDelegate += divide;
            myDelegate += delegate (int a,int b) { return a - b; };
            myDelegate += (int a, int b) => a + b;

            myDelegate(5,5);


            Dictionary<int,string> dict = new Dictionary<int, string>();

            dict.Add(1,"");

            int a = 1;
           bool ll =   int.TryParse("kjlj",out int b);

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            int[] ints = new int[5] { 1,2,3,4,5};

            Parallel.For(0, ints.Count(), new ParallelOptions { MaxDegreeOfParallelism = 5, CancellationToken = cancellationTokenSource.Token }
            , (i, loopstate) => 
            {
                Console.Write(i);
            });

            Parallel.ForEach(ints, new ParallelOptions { MaxDegreeOfParallelism = 5, CancellationToken = cancellationTokenSource.Token }
            ,(value, loopstate)=>{ });
        }


    }
}
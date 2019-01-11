using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_분기문을_피하자
{
    class doNotUseBranching_solution
    {

        static void Main(string[] args)
        {
            int[] arr = { 3, 111, 232, 443, 222, 2, 17, 13, 32 };

            Iselector selector = new oddRule();
            var ret = selector.Pick(arr);
            foreach(var i in ret) Console.WriteLine(i);

            Console.WriteLine();

            Iselector selector2 = new evenRule();
            var ret2 = selector2.Pick(arr);
            foreach(var i in ret2) Console.WriteLine(i);
        }
    }
}

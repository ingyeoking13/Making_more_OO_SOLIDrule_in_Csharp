using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_분기문을_피하자
{
    class doNotUseBranching_solution
    {

        /*
         * 옳게 된 해결법:
         * 그럼 일단
         * 제가 그런 역할을 하는 걸 만들어 둘게요.
         * 그 중에서 아무거나 골라 쓰세요.
         * 만약, 제가 없더라도, 다른 개발자분이 Iselector 인터페이스로 
         * 구현된 클래스를 한개 만드셔서 요구사항 반영하셔서 만들면되요.
        
         * 문제 정의 :
         * 배열을 순회하여 원소들을 반환하는 규칙을 객체화한다. OO!

         * 기술적인 방법을 말하다 :
         * 배열을 순회하여 원소들의 성격에 따라 반환하는 행위를 인터페이스화 한다.
         * 그 뒤 그 인터페이스로 구현된 클래스는,
         * 자신이 어떻게 구현되어야하는 조건에 따라 그 행위를 수행하게끔 작성되면 된다.
         * 결국, 이렇게 작성하게 되면 - 
         * 확장성에 대해 개방되어있고 Open for Extension
         * 수정에 대해 폐쇄적이게된다. closed for modification. OCP 만족.
         * 
         * 실제로 이전 조건에서는 이전에 작성된 함수를 소스에서 
         * 찾아내어 항상 수정해야만 했다. 그러나, 지금은 그럴필요가 없다.
         * 개이득이다.
         */


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

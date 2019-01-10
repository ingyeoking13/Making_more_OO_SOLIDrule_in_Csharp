using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_분기문을_피하자
{
    class doNotUseBranching
    {

        /*
         * 요청사항 : 
         * 자연수로 이루어진 배열의 모든 원소를 더해주세요
         * 개발자 : 
         * 네

         * 결과물 : * sum 함수 참조

         * 요청사항 2:
         * 어...음 좋아요 근데 홀수만 더해주세요
         * 개발자 :
         * 네

         * 결과물 : sumOnlyOdd 함수 참조

         * 요청사항 3:
         * 아!, 근데 어떨때는 모든 것을 더할때도 사용할거거든요
         * 결과물 : 
         * 네, 홀수만 더하실땐 true를 넘겨주세요

         * 결과물 : sumWithFlag 함수 참조

         * 요청사항 4:
         * 어,, 근데 짝수만 더해주는걸 만들어줄 수 있나요?
         * 결과물 :
         * ....ㅠ 
         */

        int sum(int[] arr)
        {
            int ret = 0;
            for (int i = 0; i < arr.Length; i++) ret += arr[i];
            return ret;
        }

        int sumOnlyOdd(int[] arr)
        {
            int ret = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] % 2 == 0) continue;
                ret += arr[i];
            }
            return ret;
        }

        int sumWithFlag(int[] arr, bool flag)
        {
            int ret = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] % 2 == 0 && flag==true) continue;
                ret += arr[i];
            }
            return ret;
        }
    }
}

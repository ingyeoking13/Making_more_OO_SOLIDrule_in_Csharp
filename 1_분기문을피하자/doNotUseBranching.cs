using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_분기문을_피하자
{
    class doNotUseBranching
    {

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PropertyBasedTesting
{
    public static class Calculator
    {
        public static int Add(int x, int y) => x + y;

        //public static int Add(int x, int y) {
        //    return (x, y) switch
        //    {
        //        (1,2)=> 3,
        //        (1, 0) => 1,
        //        (2,2)=> 4,
        //        (3,5)=> 5,
        //        _ => 0
        //    };
        //}
    }
}

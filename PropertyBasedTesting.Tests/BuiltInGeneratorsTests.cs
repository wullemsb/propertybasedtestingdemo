using FsCheck.Xunit;
using FsCheck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PropertyBasedTesting.Calculator;

namespace PropertyBasedTesting.Tests
{
    public class BuiltInGeneratorsTests
    {
        //No out-of-the-box support for value tuple(yet)
        [Property(Verbose =true)]
        public Property Adding_Two_PositiveNumbers_Doesnt_Depend_On_Parameter_Order_1(PositiveInt x, PositiveInt y)
        {
            return (Add(x.Get, y.Get) == Add(y.Get, x.Get)).ToProperty();
        }

        //No out-of-the-box support for value tuple(yet)
        [Property]
        public Property Adding_Two_Numbers_Doesnt_Depend_On_Parameter_Order_1(Tuple<int, int> values)
        {
            return (Add(values.Item1, values.Item2) == Add(values.Item1, values.Item2)).ToProperty();
        }

        [Property(Verbose = true)]
        public bool Generator1(NonEmptyArray<string> values)
        { 
            return true;
        }

        [Property(Verbose = true)]
        public bool Generator3(HostName hostnames)
        {
            return true;
        }

        [Property(Verbose =true)]
        public Property Generator2(NonEmptyArray<string> values)
        {
            return values.Get.Any(v => !string.IsNullOrEmpty(v)).ToProperty();
        }

        //No out-of-the-box support for value tuple(yet)
        // Will fail
        [Property]
        public Property Adding_Two_Numbers_Doesnt_Depend_On_Parameter_Order_2((int x, int y) values)
        {
            return (Add(values.x, values.y) == Add(values.y, values.x)).ToProperty();
        }
    }
}

using FsCheck.Xunit;
using FsCheck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PropertyBasedTesting.Calculator;
using FsCheck.Fluent;

namespace PropertyBasedTesting.Tests
{
    public class BuiltInGeneratorsTests
    {
        [Property(Verbose =true)]
        public Property Adding_Two_PositiveNumbers_Doesnt_Depend_On_Parameter_Order_1(PositiveInt x, PositiveInt y)
        {
            return (Add(x.Get, y.Get) == Add(y.Get, x.Get)).ToProperty();
        }

        [Property]
        public Property Adding_Two_Numbers_Doesnt_Depend_On_Parameter_Order_1((int a, int b) values)
        {
            return (Add(values.a, values.b) == Add(values.a, values.b)).ToProperty();
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
    }
}

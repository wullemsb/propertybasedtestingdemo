using FsCheck.Xunit;
using FsCheck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBasedTesting.Tests
{
    public class ConditionalPropertyTests
    {
        [Property]
        public Property Divide_Multiply_Number_With_Something_Result_In_Original_Number(int x, int y)
        {
            Func<bool> property = () => Divide(x * y, y) == x;
            return property.When(y != 0);
        }

        private int Divide(int x, int y)
        {
            return x / y;
        }
    }
}

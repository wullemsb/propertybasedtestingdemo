
using FsCheck;
using FsCheck.Xunit;
using static PropertyBasedTesting.Calculator;

namespace PropertyBasedTesting.Tests
{
    public class PropertyBasedTests2
    {
        [Property]
        public Property Multiply_With_2_Is_The_Same_As_Adding_The_Same_Number(int x)
        {
            return (x * 2 == Add(x, x)).ToProperty();
        }

        [Property]
        public Property Adding_1_Twice_Is_The_Same_As_Adding_Two(int x)
        {
            return (Add(1, Add(1, x)) == Add(x, 2)).ToProperty();
        }

        [Property]
        public Property Adding_Two_Numbers_Doesnt_Depend_On_Parameter_Order(int x, int y)
        {
            return (Add(x, y) == Add(y, x)).ToProperty();
        }

        [Property(Verbose =true)]
        public Property Adding_Two_Numbers_Doesnt_Depend_On_Parameter_Order_2(Tuple<int,int> values)
        {
            return (Add(values.Item1, values.Item2) == Add(values.Item1, values.Item2)).ToProperty();
        }

        [Property(MaxTest =200)]
        public Property Adding_Two_Numbers_Doesnt_Depend_On_Parameter_Order_3(Tuple<int, int> values)
        {
            return (Add(values.Item1, values.Item2) == Add(values.Item1, values.Item2)).ToProperty();
        }

    }
}
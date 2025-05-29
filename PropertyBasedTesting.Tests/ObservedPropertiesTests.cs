using FsCheck.Xunit;
using FsCheck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PropertyBasedTesting.Calculator;


namespace PropertyBasedTesting.Tests;

public class ObservedPropertiesTests
{
    //Trivial properties 
    [Property]
    public Property Multiply_With_2_Is_The_Same_As_Adding_The_Same_Number(int x)
    {
        return (x * 2 == Add(x, x)).Trivial(x < 0);
    }

    //Classified properties
    [Property]
    public Property Adding_1_Twice_Is_The_Same_As_Adding_Two(int x)
    {
        return
          (Add(1, Add(1, x)) == Add(x, 2))
            .Classify(x > 10, "Bigger than '10'")
            .Classify(x < 1000, "Smaller than '1000'");
    }

    //Collected properties
    [Property]
    public Property Adding_Two_Numbers_Doesnt_Depend_On_Parameter_Order(int x, int y)
    {
        return (Add(x, y) == Add(y, x)).Collect("Values together: " + (x + y));
    }

    //Combined observation properties
    [Property]
    public Property Adding_Two_Numbers_Doesnt_Depend_On_Parameter_Order_2(int x, int y)
    {
        return
          (Add(x, y) == Add(y, x))
            .Classify(x > 10, "Bigger than '10'")
            .Classify(x < 1000, "Smaller than '1000'")
            .Collect("Values together: " + (x + y));
    }
}

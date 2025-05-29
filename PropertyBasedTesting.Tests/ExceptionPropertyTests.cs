using FsCheck.Xunit;
using FsCheck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBasedTesting.Tests;

public class ExceptionPropertyTests
{
    [Property]
    public Property Divide_By_Zero_Result_In_Exception(int x)
    {
        //We need to provide a lazy type to support lazy evaluation
        return Prop.Throws<DivideByZeroException, int>(new Lazy<int>(() => Divide(x, 0)));
    }

    private int Divide(int x, int y)
    {
        return x / y;
    }
}


using FsCheck;
using FsCheck.Xunit;
using Xunit.Abstractions;
using static PropertyBasedTesting.Calculator;


namespace PropertyBasedTesting.Tests;

public class PropertyBasedTests
{
    private readonly ITestOutputHelper testOutputHelper;

    public PropertyBasedTests(ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void WhenIAddTwoRandomNumbersTheResultShouldNotDependOnParameterOrder_1()
    {
        var addingTwoRandomNumbers=(int x, int y) =>  Add(x, y) == Add(y, x);

        Prop.ForAll(new Func<int, int, bool>(addingTwoRandomNumbers))
        .VerboseCheck(testOutputHelper);
    }

    [Fact]
    public void WhenIAddTwoRandomNumbersTheResultShouldNotDependOnParameterOrder_2()
    {
        var config = Configuration.Default;
        config.MaxNbOfTest = 200;

        var addingTwoRandomNumbers = (int x, int y) => Add(x, y) == Add(y, x);

        Prop.ForAll(new Func<int, int, bool>(addingTwoRandomNumbers))
        .Check(config);
    }

    [Property]
    public void WhenIAddTwoRandomNumbersTheResultShouldNotDependOnParameterOrder(int input1, int input2)
    {
        //Act
        var result1 = Add(input1, input2);
        var result2 = Add(input2, input1);

        //Assert
        Assert.Equal(result1, result2);
    }

    [Property]
    public void WhenIAdd1TwiceTheResultIsTheSameAsWhenAdding2(int input)
    {

        //Act
        var result1 = Add(Add(input, 1), 1);
        var result2 = Add(input, 2);

        //Assert
        Assert.Equal(result1, result2);
    }

    [Property]
    public void WhenIAddZeroTheInputIsNotChanged(int input)
    {

        //Act
        var result1 = Add(input, 0);
        var result2 = input;

        //Assert
        Assert.Equal(result1, result2);
    }
}
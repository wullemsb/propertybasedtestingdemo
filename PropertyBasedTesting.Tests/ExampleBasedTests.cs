namespace PropertyBasedTesting.Tests
{
    public class ExampleBasedTests
    {
        [Fact]
        public void WhenIAdd1and2IExpect3()
        {
            var result = Calculator.Add(1, 2);
            Assert.Equal(3, result);
        }

        [Fact]
        public void WhenIAdd1and0IExpect1()
        {
            var result = Calculator.Add(1, 0);
            Assert.Equal(1, result);
        }






















       
        [Fact]
        public void WhenIAddTwoRandomNumbersTheResultShouldNotDependOnParameterOrder()
        {
            for (int i = 0; i < 100; i++)
            {
                //Arrange
                var random = new Random();
                int input1 = random.Next();
                int input2 = random.Next();

                //Act
                var result1 = Calculator.Add(input1, input2);
                var result2 = Calculator.Add(input2, input1);

                //Assert
                Assert.Equal(result1, result2);
            }
        }

        [Fact]
        public void WhenIAdd1TwiceTheResultIsTheSameAsWhenAdding2()
        {
            for (int i = 0; i < 100; i++)
            {
                //Arrange
                var random = new Random();
                int input = random.Next();

                //Act
                var result1 = Calculator.Add(Calculator.Add(input,1),1);
                var result2 = Calculator.Add(input, 2);

                //Assert
                Assert.Equal(result1, result2);
            }
        }

        [Fact]
        public void WhenIAddZeroTheInputIsNotChanged()
        {
            for (int i = 0; i < 100; i++)
            {
                //Arrange
                var random = new Random();
                int input = random.Next();

                //Act
                var result1 = Calculator.Add(input,0);
                var result2 = input;

                //Assert
                Assert.Equal(result1, result2);
            }
        }
    }
}
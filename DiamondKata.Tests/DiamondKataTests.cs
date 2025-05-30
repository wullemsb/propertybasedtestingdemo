using FsCheck;
using FsCheck.Xunit;
using Xunit;
using System.Linq;
using DiamondKata;
using FsCheck.Fluent;

namespace DiamondKata.Tests;

public class DiamondKataTests
{
    private static Gen<char> GenerateLetter =>
        from d in Arb.From<Char>().Generator
        where d >= 'A' && d <= 'Z'
        select d;

    public static Arbitrary<char> Letters() => GenerateLetter.ToArbitrary();

    // Helper to get N from letter
    private static int GetN(char letter) => letter - 'A' + 1;

    // 1. The upper left diagonal is filled with A, B, C, ...
    [Property(Arbitrary = [typeof(DiamondKataTests)], Verbose =true)]
    public void UpperLeftDiagonalIsFilled(char letter)
    {
        var lines = Diamond.Create(letter).ToList();
        int n = GetN(letter);
        for (int i = 0; i < n; i++)
        {
            Assert.Equal((char)('A' + i), lines[i][n - i - 1]);
        }
    }

    // 2. Horizontal symmetry
    [Property(Verbose =true)]
    public void HasHorizontalSymmetry(char letter)
    {
        var lines = Diamond.Create(letter).ToList();
        var reversed = lines.AsEnumerable().Reverse().ToList();
        Assert.Equal(lines, reversed);
    }

    // 3. Vertical symmetry
    [Property(Verbose = true)]
    public void HasVerticalSymmetry(char letter)
    {
        var lines = Diamond.Create(letter).ToList();
        foreach (var line in lines)
        {
            var reversed = new string(line.Reverse().ToArray());
            Assert.Equal(line, reversed);
        }
    }

    // 4. Height is 2N-1
    [Property(Verbose = true)]
    public void HeightIs2NMinus1(char letter)
    {
        var lines = Diamond.Create(letter).ToList();
        int n = GetN(letter);
        Assert.Equal(2 * n - 1, lines.Count);
    }

    // 5. Max width is 2N-1
    [Property(Verbose = true)]
    public void MaxWidthIs2NMinus1(char letter)
    {
        var lines = Diamond.Create(letter).ToList();
        int n = GetN(letter);
        int maxWidth = lines.Max(line => line.Length);
        Assert.Equal(2 * n - 1, maxWidth);
    }
}

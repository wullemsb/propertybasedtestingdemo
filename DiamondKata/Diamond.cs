public static class Diamond
{
    // Generates a diamond for the given uppercase letter (A-Z)
    public static IEnumerable<string> Create(char letter)
    {
        if (letter < 'A' || letter > 'Z')
            throw new ArgumentException("Input must be an uppercase letter from A to Z.");

        int size = letter - 'A';
        int width = size * 2 + 1;
        var lines = new List<string>();

        for (int i = 0; i <= size; i++)
        {
            char c = (char)('A' + i);
            string outerSpaces = new string(' ', size - i);
            if (i == 0)
            {
                lines.Add(outerSpaces + c + outerSpaces);
            }
            else
            {
                string innerSpaces = new string(' ', i * 2 - 1);
                lines.Add(outerSpaces + c + innerSpaces + c + outerSpaces);
            }
        }
        for (int i = size - 1; i >= 0; i--)
        {
            lines.Add(lines[i]);
        }
        return lines;
    }
}

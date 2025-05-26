using FsCheck;
using FsCheck.Fluent;
using PropertyBasedTesting;
// This property asserts that the reverse of the reverse of a list is the list itself. 
bool revRevIsOriginal(int[] xs) => xs.Reverse().Reverse().SequenceEqual(xs);
var property=Prop.ForAll<int[]>(revRevIsOriginal);

Check.Quick(property);

Prop.ForAll<double[]>(xs => xs.Reverse().Reverse().SequenceEqual(xs))
    .VerboseCheck();





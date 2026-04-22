// See https://aka.ms/new-console-template for more information
using System.Collections.Immutable;
using AdventOfCode.solutions.one;
using AdventOfCode.solutions.two;

Console.WriteLine("Hello, World!");
{
    var dayone = 
        File.ReadLines("data/dayone")
        .Select(Spin.LineToSpin)
        .ToImmutableList()
        .Aggregate(Spin.New(DirectionKind.Right, 50), Spin.SpinCalc);

    Console.WriteLine($"Final spins of dial gets {dayone.Direction.Number} with {dayone.Zeros} zeros");
}

{
    var daytwo = 
        File.ReadAllText("data/daytwo")
        .Split(",")
        .Select(IDRange.ParseString)
        .SelectMany(IDRange.GetInvalidIDs)
        .Sum(e => long.TryParse(e.Id.Value, out long value) 
            ? value 
            : throw new InvalidCastException()
        );

    Console.WriteLine($"the sum of all invalid id is {daytwo}");
}

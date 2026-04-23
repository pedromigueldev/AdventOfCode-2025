
using System.Collections.Immutable;

namespace AdventOfCode.solutions.two;
public readonly record struct InvalidID(NonEmptyString Id);
public readonly record struct FirstID(NonEmptyString Id);
public readonly record struct LastID(NonEmptyString Id);
public readonly record struct IDRange (FirstID FirstID, LastID LastID, bool IsRangeValid);

public static class IDRangeExt
{
    extension (in IDRange iDRange)
    {
        public static IDRange New (FirstID FirstID, LastID LastID) => 
                new IDRange(FirstID, LastID, false)
                    .Expand()
                    .Any(number => number[..(number.Length / 2)] == number[(number.Length / 2)..]) 
                        ? new (FirstID, LastID, false) 
                        : new (FirstID, LastID, true);
        public static IEnumerable<InvalidID> GetInvalidIDs (in IDRange range) => range.GetInvalids();
        public static IEnumerable<string> ExpandRange (in IDRange range) =>  range.Expand(); 
        public static IDRange ParseString (string supposedID) =>
            supposedID
                .Split('-')
                .Select(e => new NonEmptyString(e))
                .ToImmutableArray() switch
                {
                    [var first, var second] => IDRange.New(new (first), new (second)),
                    _ => throw new ArgumentException("Invalid ID range format")
                };
        public IEnumerable<string> Expand () =>
            long.TryParse(iDRange.FirstID.Id.ToString(), out long firstNumber) && 
            long.TryParse(iDRange.LastID.Id.ToString(), out long secondNumber) 
                ? Enumerable.LongRange(firstNumber, (int)(secondNumber - firstNumber + 1)).Select(n => n.ToString())
                : throw new Exception("Error parsing number from string");

        public IEnumerable<InvalidID> GetInvalids () => iDRange switch
        {
            (_, _, var valid) when valid is true => [],
            (var FirstID, var LastID, _) => 
                iDRange.Expand()
                    .Where(number => number[..(number.Length / 2)] == number[(number.Length / 2)..])
                    .Select(number => new InvalidID(new (number)))
        };
    }

    extension (Enumerable)
    {
        public static IEnumerable<long> LongRange(long start, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), "Count cannot be negative.");
            
            if (count > 0 && start > long.MaxValue - count + 1)
                throw new ArgumentOutOfRangeException(nameof(count), "Range exceeds long.MaxValue.");
            
            for (int i = 0; i < count; i++)
                yield return start + i;
        }
    }
}
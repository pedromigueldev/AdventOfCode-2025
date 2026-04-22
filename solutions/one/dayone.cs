using System.Data;

namespace AdventOfCode.solutions.one;


public enum DirectionKind { Left, Right }
public readonly record struct Direction(DirectionKind Kind, int Number);
public readonly record struct Spin(Direction Direction, int Zeros);
public static class SpinExt
{
    extension (Spin spin)
    {
        public static Spin New (DirectionKind Kind, int Number) =>
             new (new (Kind, Number), 0);

        public static Spin LineToSpin (string line) => 
            line switch
            {
                ['R', ..] => int.TryParse(line[1..], out int number) 
                    ? Spin.New(DirectionKind.Right, number)
                    : throw new InvalidCastException("A right turn had a invalid number"),
                ['L', ..] => int.TryParse(line[1..], out int number) 
                    ? Spin.New(DirectionKind.Left, number)
                    : throw new InvalidCastException("A left turn had a invalid number"),
                _ => throw new InvalidOperationException("Invalid direction")
            };

        public static Spin SpinCalc (Spin old, Spin newCalc) =>
            newCalc.Direction switch
            {
                (DirectionKind.Left, var number) => SpinDoCalc(old, old.Direction.Number - number),
                (DirectionKind.Right, var number) => SpinDoCalc(old, old.Direction.Number + number),
                _ => throw new InvalidExpressionException("Error while calculating zeros")
            };

        private static Spin SpinDoCalc(Spin old, int number) => old with
            {
                Direction = old.Direction with { Number = ((number % 100) + 100) % 100 },
                Zeros = old.Zeros + (old.Direction.Number != 0 && number <= 0 ? Math.Abs(number / 100) + 1 : Math.Abs(number / 100))
            };
    }
}
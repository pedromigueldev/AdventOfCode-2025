public record NonEmptyString(string Value)
{
    public string Value { get; } = !string.IsNullOrEmpty(Value)
        ? Value
        : throw new InvalidOperationException("ID cannot be null or empty");

    public static explicit operator NonEmptyString(string value) => new(value);
    public override string ToString() => Value;
}
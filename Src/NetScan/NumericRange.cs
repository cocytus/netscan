using System.Numerics;

namespace NetScan;

public readonly record struct NumericRange<T>(T From, T To)
    where T: INumber<T>, IParsable<T>
{
    public static NumericRange<T> Parse(string input)
    {
        var split = input.Split('-');
        if (split.Length == 1) 
        {         
            var b = T.Parse(input, null);
            return new(b, b);
        }
        else if (split.Length == 2)
        {
            var x = new NumericRange<T>(T.Parse(split[0], null), T.Parse(split[1], null));
            if (x.From > x.To)
            {
                throw new InvalidOperationException($"{input} has From > To");
            }

            return x;
        }
        else
        {
            throw new InvalidOperationException($"Invalid component lengths");
        }
    }

    public IEnumerable<T> GetValues()
    {
        for (var x = From; x <= To;)
        {
            yield return x;

            var old = x;
            x++;
            if (x < old)
            {
                break;
            }
        }
    }
}

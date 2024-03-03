namespace NetScan;

internal sealed class IPv4Range
{
    private readonly NumericRange<byte>[] _comps;

    public IPv4Range(NumericRange<byte>[] comps)
    {
        _comps = comps;
    }

    public static IPv4Range Create(string input)
    {
        var comps = input.Split('.');

        if (comps.Length != 4)
        {
            throw new InvalidOperationException("Invalid IP Range. Spec is a.b.c.d, where every component can be either a number or a range. Example \"10.0.0-2.1-255\".");
        }

        var el = comps.Select(NumericRange<byte>.Parse).ToArray();
        if (el.Length != 4)
        {
            throw new InvalidOperationException("Comps is not 4");
        }

        return new(el);
    }

    public IEnumerable<string> GetIps()
    {
        foreach (var a in _comps[0].GetValues())
        {
            foreach (var b in _comps[1].GetValues())
            {
                foreach (var c in _comps[2].GetValues())
                {
                    foreach (var d in _comps[3].GetValues())
                    {
                        yield return $"{a}.{b}.{c}.{d}";
                    }
                }
            }
        }
    }
}
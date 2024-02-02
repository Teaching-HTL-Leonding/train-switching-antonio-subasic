namespace TrainSwitching.Logic;

public static class SwitchingOperationParser
{
    /// <summary>
    /// Parses a line of input into a <see cref="SwitchingOperation"/>.
    /// </summary>
    /// <param name="inputLine">Line to parse. See readme.md for details</param>
    /// <returns>The parsed switching operation</returns>
    public static SwitchingOperation Parse(string inputLine)
    {
        var parts = inputLine.Split(' ');

        var track = int.Parse(parts[2].TrimEnd(','));
        var direction = Enum.Parse<Enums.Direction>(parts[^1]);

        var operation = new SwitchingOperation { TrackNumber = track, Direction = direction };

        if (parts.Contains("add"))
        {
            operation.OperationType = Enums.Operation.Add;
            operation.WagonType = Enum.Parse<Enums.Wagon>(string.Join(' ', parts[4..^2]).TrimEnd(" Wagon".ToCharArray()).Replace(" ", ""));
        }
        else if (parts.Contains("remove"))
        {
            operation.OperationType = Enums.Operation.Remove;
            operation.NumberOfWagons = int.Parse(parts[4]);
        }
        else if (parts.Contains("leaves"))
        {
            operation.OperationType = Enums.Operation.TrainLeave;
        }
        else { throw new InvalidOperationException("Invalid input"); }

        return operation;
    }
}
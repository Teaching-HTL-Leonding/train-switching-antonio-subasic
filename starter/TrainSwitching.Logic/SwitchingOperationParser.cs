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

    public static SwitchingOperation Parse(byte[] inputBytes)
    {
        int track = inputBytes[0] >> 4;
        var operationType = (Enums.Operation)(inputBytes[0] & 0b00001111);
        var direction = (Enums.Direction)(inputBytes[1] >> 7);

        var operation = new SwitchingOperation
        {
            TrackNumber = track,
            OperationType = operationType,
            Direction = direction
        };

        switch (operationType)
        {
            case Enums.Operation.Add:
                operation.WagonType = (inputBytes[1] & 0b01111111) switch 
                {
                    0 => Enums.Wagon.Passenger,
                    1 => Enums.Wagon.Locomotive,
                    2 => Enums.Wagon.Freight,
                    3 => Enums.Wagon.CarTransport,
                    _ => throw new InvalidOperationException("Invalid input")
                };
                break;
            case Enums.Operation.Remove:
                operation.NumberOfWagons = inputBytes[1] & 0b01111111;
                break;
        }

        return operation;
    }
}
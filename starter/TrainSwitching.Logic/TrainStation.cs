namespace TrainSwitching.Logic;

public class TrainStation
{
    public Track[] Tracks { get; }

    public TrainStation()
    {
        Tracks = new Track[10];

        for (var i = 0; i < Tracks.Length; i++)
        {
            Tracks[i] = new Track();
        }
    }

    /// <summary>
    /// Tries to apply the given operation to the train station.
    /// </summary>
    /// <param name="op">Operation to apply</param>
    /// <returns>Returns true if the operation could be applied, otherwise false</returns>
    public bool TryApplyOperation(SwitchingOperation op)
    {
        if (op.TrackNumber is <= 0 or > 10) { return false; }
        else
        {
            var track = Tracks[op.TrackNumber - 1];

            var removeInvalid = op.OperationType == Enums.Operation.Remove && (op.NumberOfWagons > track.Wagons.Count || (op.TrackNumber >= 9 && op.Direction == Enums.Direction.East));
            var leaveInvalid = op.OperationType == Enums.Operation.TrainLeave && track.Wagons.All(wagon => wagon != Enums.Wagon.Locomotive);

            var possible = !(removeInvalid || leaveInvalid);

            if (possible)
            {
                if (op.OperationType == Enums.Operation.Add)
                {
                    if (op.Direction == Enums.Direction.West) { track.Wagons.Insert(0, op.WagonType!.Value); }
                    else { track.Wagons.Add(op.WagonType!.Value); }
                }
                else if (op.OperationType == Enums.Operation.Remove)
                {
                    track.Wagons.RemoveRange(
                        op.Direction == Enums.Direction.West
                        ? 0
                        : track.Wagons.Count - op.NumberOfWagons!.Value, op.NumberOfWagons!.Value
                    );
                }
                else if (op.OperationType == Enums.Operation.TrainLeave) { track.Wagons.Clear(); }
            }

            return possible;
        }
    }

    /// <summary>
    /// Calculates the checksum of the train station.
    /// </summary>
    /// <returns>The calculated checksum</returns>
    /// <remarks>
    /// See readme.md for details on how to calculate the checksum.
    /// </remarks>
    public int CalculateChecksum()
    {
        var checksum = 0;

        for (var i = 0; i < Tracks.Length; i++)
        {
            checksum += (i + 1) * Tracks[i].Wagons.Select(wagon => (int)wagon).Sum();
        }

        return checksum;
    }
}
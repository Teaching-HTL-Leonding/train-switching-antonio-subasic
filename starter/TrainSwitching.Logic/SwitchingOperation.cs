namespace TrainSwitching.Logic;

public class SwitchingOperation
{
    public int TrackNumber { get; set; }

    public Enums.Operation OperationType { get; set; }

    public Enums.Direction Direction { get; set; }

    public Enums.Wagon? WagonType { get; set; }

    public int? NumberOfWagons { get; set; }
}
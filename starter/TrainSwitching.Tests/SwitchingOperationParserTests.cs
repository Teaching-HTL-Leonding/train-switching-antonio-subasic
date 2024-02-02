using TrainSwitching.Logic;
using static TrainSwitching.Logic.Enums;

namespace TrainSwitching.Tests;

public class SwitchingOperationParserTests
{
    [Theory]
    [InlineData("At track 1, add Passenger Wagon from East", 1, Operation.Add, Direction.East, Wagon.Passenger, null)]
    [InlineData("At track 2, add Locomotive from East", 2, Operation.Add, Direction.East, Wagon.Locomotive, null)]
    [InlineData("At track 3, add Freight Wagon from West", 3, Operation.Add, Direction.West, Wagon.Freight, null)]
    [InlineData("At track 4, add Car Transport Wagon from West", 4, Operation.Add, Direction.West, Wagon.CarTransport, null)]
    [InlineData("At track 5, remove 3 wagons from East", 5, Operation.Remove, Direction.East, null, 3)]
    [InlineData("At track 6, train leaves to West", 6, Operation.TrainLeave, Direction.West, null, null)]
    public void ParseOperation(string line, int trackNumber, Operation operationType, Direction direction, Wagon? wagonType, int? numberOfWagons)
    {
        var operation = SwitchingOperationParser.Parse(line);

        Assert.Equal(trackNumber, operation.TrackNumber);
        Assert.Equal(operationType, operation.OperationType);
        Assert.Equal(direction, operation.Direction);
        Assert.Equal(wagonType, operation.WagonType);
        Assert.Equal(numberOfWagons, operation.NumberOfWagons);
    }
}
using TrainSwitching.Logic;
using static TrainSwitching.Logic.Enums;

namespace TrainSwitching.Tests;

public class BinaryParseTests
{
    [Theory]
    [InlineData(new byte[] { 0b00010001, 0b00000001 }, 1, Operation.Add, Wagon.Locomotive, Direction.East, null)]
    [InlineData(new byte[] { 0b00100000, 0b10000000 }, 2, Operation.TrainLeave, null, Direction.West, null)]
    [InlineData(new byte[] { 0b00100010, 0b10000100 }, 2, Operation.Remove, null, Direction.West, 4)]
    public void Parse_Binary(byte[] inputBytes, int track, Operation operationType, Wagon? wagonType, Direction direction, int? numberOfWagons)
    {
        var operation = SwitchingOperationParser.Parse(inputBytes);

        Assert.Equal(track, operation.TrackNumber);
        Assert.Equal(operationType, operation.OperationType);
        Assert.Equal(wagonType, operation.WagonType);
        Assert.Equal(direction, operation.Direction);
        Assert.Equal(numberOfWagons, operation.NumberOfWagons);
    }
}
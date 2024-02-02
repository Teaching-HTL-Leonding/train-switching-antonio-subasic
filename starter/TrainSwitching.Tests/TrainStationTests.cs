using TrainSwitching.Logic;
using static TrainSwitching.Logic.Enums;

namespace TrainSwitching.Tests;

public class TrainStationTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(11)]
    public void TryApplyOperation_InvalidTrackNumber_ReturnsFalse(int trackNumber)
    {
        var station = new TrainStation();
        var op = new SwitchingOperation { TrackNumber = trackNumber };
        Assert.False(station.TryApplyOperation(op));
    }

    [Fact]
    public void TryApplyOperation_InvalidDirectionForTrack9And10_ReturnsFalse()
    {
        var station = new TrainStation();
        var op = new SwitchingOperation { TrackNumber = 9, Direction = Direction.East };
        Assert.False(station.TryApplyOperation(op));
    }

    [Fact]
    public void TryApplyOperation_AddOperation_AddsWagonToTrack()
    {
        var station = new TrainStation();
        var op = new SwitchingOperation { TrackNumber = 1, OperationType = Operation.Add, WagonType = Wagon.Freight, Direction = Direction.East };
        Assert.True(station.TryApplyOperation(op));
        Assert.Contains(Wagon.Freight, station.Tracks[0].Wagons);
    }

    [Fact]
    public void TryApplyOperation_RemoveOperation_RemovesWagonFromTrack()
    {
        var station = new TrainStation();
        var addOp = new SwitchingOperation { TrackNumber = 1, OperationType = Operation.Add, WagonType = Wagon.Freight, Direction = Direction.East };
        Assert.True(station.TryApplyOperation(addOp));
        var removeOp = new SwitchingOperation { TrackNumber = 1, OperationType = Operation.Remove, NumberOfWagons = 1, Direction = Direction.East };
        Assert.True(station.TryApplyOperation(removeOp));
        Assert.DoesNotContain(Wagon.Freight, station.Tracks[0].Wagons);
    }

    [Fact]
    public void TryApplyOperation_RemoveOperation_TooManyWagons()
    {
        var station = new TrainStation();
        var addOp = new SwitchingOperation { TrackNumber = 1, OperationType = Operation.Add, WagonType = Wagon.Freight, Direction = Direction.East };
        Assert.True(station.TryApplyOperation(addOp));
        var removeOp = new SwitchingOperation { TrackNumber = 1, OperationType = Operation.Remove, NumberOfWagons = 2, Direction = Direction.East };
        Assert.False(station.TryApplyOperation(removeOp));
        Assert.Contains(Wagon.Freight, station.Tracks[0].Wagons);
    }

    [Fact]
    public void TryApplyOperation_RemoveOperation_RemovesFromRightDirection()
    {
        var station = new TrainStation();
        var addOp = new SwitchingOperation { TrackNumber = 1, OperationType = Operation.Add, WagonType = Wagon.Freight, Direction = Direction.East };
        Assert.True(station.TryApplyOperation(addOp));
        addOp = new SwitchingOperation { TrackNumber = 1, OperationType = Operation.Add, WagonType = Wagon.CarTransport, Direction = Direction.East };
        Assert.True(station.TryApplyOperation(addOp));
        var removeOp = new SwitchingOperation { TrackNumber = 1, OperationType = Operation.Remove, NumberOfWagons = 1, Direction = Direction.West };
        Assert.True(station.TryApplyOperation(removeOp));
        Assert.DoesNotContain(Wagon.Freight, station.Tracks[0].Wagons);
        Assert.Contains(Wagon.CarTransport, station.Tracks[0].Wagons);
    }

    [Fact]
    public void TryApplyOperation_TrainLeaveOperation_ClearsTrack()
    {
        var station = new TrainStation();
        var addOp = new SwitchingOperation { TrackNumber = 1, OperationType = Operation.Add, WagonType = Wagon.Locomotive, Direction = Direction.East };
        station.TryApplyOperation(addOp);
        var leaveOp = new SwitchingOperation { TrackNumber = 1, OperationType = Operation.TrainLeave };
        Assert.True(station.TryApplyOperation(leaveOp));
        Assert.Empty(station.Tracks[0].Wagons);
    }

    [Fact]
    public void TryApplyOperation_TrainLeaveOperation_NoLocomotive_ReturnsFalse()
    {
        var station = new TrainStation();
        var addOp = new SwitchingOperation { TrackNumber = 1, OperationType = Operation.Add, WagonType = Wagon.Passenger, Direction = Direction.East };
        station.TryApplyOperation(addOp);
        var leaveOp = new SwitchingOperation { TrackNumber = 1, OperationType = Operation.TrainLeave };
        Assert.False(station.TryApplyOperation(leaveOp));
        Assert.NotEmpty(station.Tracks[0].Wagons);
    }
}
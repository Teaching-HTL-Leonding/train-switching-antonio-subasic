using TrainSwitching.Logic;
using static TrainSwitching.Logic.Enums;

namespace TrainSwitching.Tests;

public class ChecksumTests
{
    [Fact]
    public void CalculateChecksum_EmptyTracks_ReturnsZero()
    {
        var station = new TrainStation();
        Assert.Equal(0, station.CalculateChecksum());
    }

    [Theory]
    [InlineData(Wagon.Locomotive, 0)]
    [InlineData(Wagon.Freight, 0)]
    [InlineData(Wagon.CarTransport, 0)]
    [InlineData(Wagon.Passenger, 0)]
    [InlineData(Wagon.Locomotive, 5)]
    [InlineData(Wagon.Freight, 5)]
    [InlineData(Wagon.CarTransport, 5)]
    [InlineData(Wagon.Passenger, 5)]
    public void CalculateChecksum_SingleWagon(Wagon wagonType, int track)
    {
        var station = new TrainStation();
        station.Tracks[track].Wagons.Add(wagonType);
        var expected = wagonType switch
        {
            Wagon.Locomotive => 10,
            Wagon.Freight => 20,
            Wagon.CarTransport => 30,
            _ => 1,
        };
        Assert.Equal(expected * (track + 1), station.CalculateChecksum());
    }

    [Fact]
    public void CalculateChecksum_MultipleWagonsOnMultipleTracks_ReturnsCorrectValue()
    {
        var station = new TrainStation();
        station.Tracks[0].Wagons.Add(Wagon.Locomotive);
        station.Tracks[1].Wagons.Add(Wagon.Freight);
        station.Tracks[1].Wagons.Add(Wagon.CarTransport);
        Assert.Equal(10 + 2 * (20 + 30), station.CalculateChecksum());
    }
}
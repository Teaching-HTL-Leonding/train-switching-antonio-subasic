namespace TrainSwitching.Logic;

public static class Enums
{
    public enum Operation
    {
        Add = 1,
        TrainLeave = 0,
        Remove = -1
    }

    public enum Wagon
    {
        Passenger = 1,
        Locomotive = 10,
        Freight = 20,
        CarTransport = 30
    } 

    public enum Direction
    {
        East,
        West
    }
}
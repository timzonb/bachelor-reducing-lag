using System;

[Serializable]
public struct Event
{
    public Eventable Runner;
    public DateTime Time;
    public DateTime PreviousTime;

    public Event(Eventable runner, DateTime time, DateTime previousTime)
    {
        Runner = runner;
        Time = time;
        PreviousTime = previousTime;
    }
}

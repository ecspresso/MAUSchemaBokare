using System.Collections;

namespace Scraper;

public class Room(string name)
{
    private static readonly TimeSlot[] TimeSlots =
    {
        new() { Start = DateTime.Parse("08:15"), End = DateTime.Parse("10:00") },
        new() { Start = DateTime.Parse("10:15"), End = DateTime.Parse("13:00") },
        new() { Start = DateTime.Parse("13:15"), End = DateTime.Parse("15:00") },
        new() { Start = DateTime.Parse("15:15"), End = DateTime.Parse("17:00") },
        new() { Start = DateTime.Parse("17:15"), End = DateTime.Parse("20:00") }
    };

    public BitArray Slots { get; } = new(5);
    public string Name { get; } = name;

    public void Track(string time)
    {
        (var start, var end) = ParseTime(time);
        TimeIsInSlot(start, end);
    }

    private void TimeIsInSlot(DateTime start, DateTime end)
    {
        for (var i = 0; i < TimeSlots.Length; i++)
            if (start <= TimeSlots[i].Start && TimeSlots[i].End <= end)
                Slots[i] = true;
            else if (start <= TimeSlots[i].Start && end <= TimeSlots[i].End && end > TimeSlots[i].Start)
                Slots[i] = true;
            else if (start >= TimeSlots[i].Start && end <= TimeSlots[i].End)
                Slots[i] = true;
            else if (start >= TimeSlots[i].Start && start < TimeSlots[i].End && end >= TimeSlots[i].End)
                Slots[i] = true;
    }

    private (DateTime, DateTime) ParseTime(string time)
    {
        var times = time.Split('-');

        var start = DateTime.ParseExact(times[0], "HH:mm", null);
        var end = DateTime.ParseExact(times[1], "HH:mm", null);

        return (start, end);
    }

    public void Print()
    {
        Console.WriteLine($"Room: {Name}");
        for (var i = 0; i < TimeSlots.Length; i++)
            Console.WriteLine($"Slot {i} at {TimeSlots[i]:HH:mm}: {(Slots[i] ? "Busy" : "Free")}");
    }
}
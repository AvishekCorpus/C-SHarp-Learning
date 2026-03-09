namespace ShipmentLearning;

public class Parcel(int id, double length, double breadth, double height, double weight, double value)
// This is the blueprint — every parcel object in the program is built from this
// This is the blueprint — every parcel object in the program is built from this
{
    // ── Properties ───────────────────────────────────────────
    // private set — the outside world can READ but not OVERWRITE
    // this is encapsulation — protecting the data inside the object

    public int Id { get; private set; } = id;
    public double Weight { get; private set; } = weight;
    public double Dimensions { get; private set; } = length * breadth * height; // volume calculated here so nobody else has to
    public double Value { get; private set; } = value;
    public bool IsDelivered { get; private set; } = false; // every new parcel starts as not delivered
    public double Length { get; private set; } = length;
    public double Breadth { get; private set; } = breadth;
    public double Height { get; private set; } = height;


    // ── Methods ───────────────────────────────────────────────
    public void MarkAsDelivered()
    // Flips IsDelivered from false to true — one job, one line
    {
        IsDelivered = true;
    }

    public static void Elevatepuja ()
    {
        Console.WriteLine("I love you puja");
    }
    public void PrintParcelSummary()
{
    Console.WriteLine($"Parcel Weight: {Weight}, Height: {Height}");
}
}
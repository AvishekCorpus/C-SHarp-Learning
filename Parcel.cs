namespace ShipmentLearning;

public class Parcel(int id, double length, double breadth, double height, double weight, double value)
{
    // ── Properties ───────────────────────────────────────────
    public int    Id          { get; private set; } = id;
    public double Weight      { get; private set; } = weight;
    public double Dimensions  { get; private set; } = length * breadth * height;
    public double Value       { get; private set; } = value;
    public bool   IsDelivered { get; private set; } = false;
    public double Length      { get; private set; } = length;
    public double Breadth     { get; private set; } = breadth;
    public double Height      { get; private set; } = height;

    // ── Methods ───────────────────────────────────────────────
    public void MarkAsDelivered() => IsDelivered = true;

    // GAP 4 FIX: full display now lives here so Program.cs doesn't need
    //            its own DisplayParcel helper and both places stay in sync
    public void Display()
    {
        Console.WriteLine("Parcel ID:         " + Id);
        Console.WriteLine("Weight:            " + Weight);
        Console.WriteLine("Dimensions Volume: " + Dimensions);
        Console.WriteLine("Value:             " + Value);
        Console.WriteLine("Delivered Status:  " + IsDelivered);
    }

    public static void Elevatepuja()
    {
        Console.WriteLine("I love you puja");
    }
}
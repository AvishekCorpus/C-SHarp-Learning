namespace ShipmentLearning;

public class Parcel
{
    // ── Properties ───────────────────────────────────────────
    public int    Id          { get; set; }
    public double Weight      { get; set; }
    public double Dimensions  { get; set; }
    public double Value       { get; set; }
    public bool   IsDelivered { get; set; } = false;
    public double Length      { get; set; }
    public double Breadth     { get; set; }
    public double Height      { get; set; }
    public string Category    { get; set; } = string.Empty;
    private static int TotalParcelsCreated { get; set; } = 0;

    // Parameterless ctor for EF and serialization
    public Parcel() { }

    // Constructor
    public Parcel(int id, double length, double breadth, double height, double weight, double value, string category)
    {
        Id = id;
        Weight = weight;
        Dimensions = length * breadth * height;
        Value = value;
        Length = length;
        Breadth = breadth;
        Height = height;
        Category = category;
        TotalParcelsCreated++;
    }

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
        Console.WriteLine("Cost:            " + GetShippingCost());
        Console.WriteLine("Category:          " + Category);
        Console.WriteLine("TotalParcelsCreated:" + GetTotalParcelsCreated());
    }
    public static void Elevatepuja()
    {
        Console.WriteLine("I love you puja");
    }

    public static int GetTotalParcelsCreated()
    {
        return TotalParcelsCreated;
    }
    public double GetShippingCost()
    {
        double cost = Weight * 0.5 + Value * 0.01;
        return cost;
    }

        

        // Add a surcharge for high-value items
        
    
}
namespace ShipmentLearning;

class Program
{
    private static readonly List<Parcel> parcels = [];

    public static List<Parcel> Parcels => parcels;

    // GAP 3 FIX: GetParcels1() removed — it was a wrapper that only called
    //            itself and added no behaviour
    public static List<Parcel> GetParcels() => parcels;

    static void Main()
    {
        Console.WriteLine("Parcel Creation System");

        Console.WriteLine("Can I surprise you with a random fact? (yes/no): ");
        if ((Console.ReadLine() ?? "no").ToLower() == "yes")
            Parcel.Elevatepuja();

        // ── Single Parcel ─────────────────────────────────────
        Console.WriteLine("\n--- Enter details for your first parcel ---");

        Parcel parcel = CreateParcel(maxAttempts: 3);

        // GAP 1 FIX: add the first parcel to the shared list immediately so
        //            that the duplicate-ID check in CreateParcel can see it
        //            when the batch parcels are being entered
        GetParcels().Add(parcel);

        Console.WriteLine("\nParcel Created Successfully!");
        parcel.Display(); // GAP 4 FIX: delegate display to the class

        Console.Write("\nMark this parcel as delivered? (yes/no): ");
        if ((Console.ReadLine() ?? "no").ToLower() == "yes")
        {
            parcel.MarkAsDelivered();
            Console.WriteLine("Delivered Status: " + parcel.IsDelivered);
        }

        // ── Batch of Parcels ──────────────────────────────────
        int batchSize = 0;
        Console.Write("\nHow many parcels do you want to add to the batch? ");
        while (!int.TryParse(Console.ReadLine() ?? "", out batchSize) || batchSize < 0)
            Console.Write("Invalid input. Please enter a valid number of parcels: ");

        for (int i = 1; i <= batchSize; i++)
        {
            Console.WriteLine($"\n--- Enter details for Parcel {i} ---");
            GetParcels().Add(CreateParcel());
            Console.WriteLine($"Parcel {i} added successfully!");
        }

        // GAP 1 FIX (continued): the "add first parcel if missing" block is
        //            no longer needed because it was added above at creation time
        //            — removed to avoid confusion and dead logic

        // ── Search ────────────────────────────────────────────
        string searchAgain = "yes";

        while (searchAgain.ToLower() == "yes")
        {
            Console.Write("\nEnter Parcel ID to search: ");

            if (int.TryParse(Console.ReadLine() ?? "", out int searchId))
            {
                Parcel? found = GetParcels().Find(p => p.Id == searchId);

                if (found != null)
                {
                    Console.WriteLine("\nParcel found:");
                    found.Display(); // GAP 4 FIX: same here
                }
                else
                {
                    Console.WriteLine("Parcel with ID " + searchId + " not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid Parcel ID entered.");
            }

            Console.Write("\nSearch for another parcel? (yes/no): ");
            searchAgain = Console.ReadLine() ?? "no";
        }

        Console.WriteLine("\nThank you for using the Parcel Creation System. Goodbye!");
    }


    // ── Helper Methods ────────────────────────────────────────

    static Parcel CreateParcel(int maxAttempts = int.MaxValue)
    {
        int    id      = ReadInt   ("Enter Parcel ID: ",     "Invalid input. Please enter a unique Parcel ID: ",   maxAttempts, extraCheck: v => GetParcels().Find(p => p.Id == v) == null);
        double length  = ReadDouble("Enter Length: ",         "Invalid input. Please enter a valid Length: ",       maxAttempts);
        double breadth = ReadDouble("Enter Breadth: ",        "Invalid input. Please enter a valid Breadth: ",      maxAttempts);
        double height  = ReadDouble("Enter Height: ",         "Invalid input. Please enter a valid Height: ",       maxAttempts);
        double weight  = ReadDouble("Enter Weight: ",         "Invalid input. Please enter a valid Weight: ",       maxAttempts);
        double value   = ReadDouble("Enter Parcel Value: ",   "Invalid input. Please enter a valid Parcel Value: ", maxAttempts);
        string category = ReadString("Enter Parcel Category: ", "Invalid input. Please enter a valid Parcel Category: ", maxAttempts);

        return new Parcel(id: id, length: length, breadth: breadth, height: height, weight: weight, value: value, category: category);
    }


    // ── Input helpers ─────────────────────────────────────────

    static int ReadInt(string prompt, string errorPrompt, int maxAttempts, Func<int, bool>? extraCheck = null)
    {
        int attempts = 0;
        Console.Write(prompt);

        while (true)
        {
            string raw         = Console.ReadLine() ?? "";
            bool   parsed      = int.TryParse(raw, out int result);
            bool   passesExtra = extraCheck == null || extraCheck(result);

            if (parsed && result > 0 && passesExtra)
                return result;

            attempts++;

            if (attempts >= maxAttempts)
            {
                Console.WriteLine("\nToo many invalid attempts. Please try again later. Goodbye!");
                Environment.Exit(0);
            }

            Console.Write($"[Attempt {attempts}/{maxAttempts}] {errorPrompt}");
        }
    }

    static double ReadDouble(string prompt, string errorPrompt, int maxAttempts)
    {
        int attempts = 0;
        Console.Write(prompt);

        while (true)
        {
            string raw    = Console.ReadLine() ?? "";
            bool   parsed = double.TryParse(raw, out double result);

            if (parsed && result > 0)
                return result;

            attempts++;

            if (attempts >= maxAttempts)
            {
                Console.WriteLine("\nToo many invalid attempts. Please try again later. Goodbye!");
                Environment.Exit(0);
            }

            Console.Write($"[Attempt {attempts}/{maxAttempts}] {errorPrompt}");
        }
    }

    // GAP 4 FIX: DisplayParcel removed — Parcel.Display() does this job now
static string ReadString(string prompt, string errorPrompt, int maxAttempts)
    {
        int attempts = 0;
        Console.Write(prompt);

        while (true)
        {
            string result = Console.ReadLine() ?? "";

            if (!string.IsNullOrWhiteSpace(result))
                return result;

            attempts++;

            if (attempts >= maxAttempts)
            {
                Console.WriteLine("\nToo many invalid attempts. Please try again later. Goodbye!");
                Environment.Exit(0);
            }

            Console.Write($"[Attempt {attempts}/{maxAttempts}] {errorPrompt}");
        }
    }
}

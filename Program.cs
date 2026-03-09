namespace ShipmentLearning;

class Program
{
    static List<Parcel> parcels = new List<Parcel>();

    static void Main()
    {
        Console.WriteLine("Parcel Creation System");

        Console.WriteLine("Can I surprise you with a random fact? (yes/no): ");
        if ((Console.ReadLine() ?? "no").ToLower() == "yes")
        {
            Parcel.Elevatepuja();
        }

        // ── Single Parcel ─────────────────────────────────────
        Console.WriteLine("\n--- Enter details for your first parcel ---");

        Parcel parcel = CreateParcel(maxAttempts: 3);
        // ↑ First parcel only gets 3 attempts per field before the program exits

        Console.WriteLine("\nParcel Created Successfully!");
        DisplayParcel(parcel);

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
        {
            Console.Write("Invalid input. Please enter a valid number of parcels: ");
        }

        for (int i = 1; i <= batchSize; i++)
        {
            Console.WriteLine($"\n--- Enter details for Parcel {i} ---");
            parcels.Add(CreateParcel());
            // ↑ Batch parcels have unlimited attempts (default behaviour)
            Console.WriteLine($"Parcel {i} added successfully!");
        }

        if (parcels.Find(p => p.Id == parcel.Id) == null)
        {
            parcels.Add(parcel);
            Console.WriteLine("\nYour first parcel has also been added to the batch.");
        }

        // ── Search ────────────────────────────────────────────
        string searchAgain = "yes";

        while (searchAgain.ToLower() == "yes")
        {
            Console.Write("\nEnter Parcel ID to search: ");

            if (int.TryParse(Console.ReadLine() ?? "", out int searchId))
            {
                Parcel? found = parcels.Find(p => p.Id == searchId);

                if (found != null)
                {
                    Console.WriteLine("\nParcel found:");
                    DisplayParcel(found);
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
    // maxAttempts — how many wrong tries are allowed per field before quitting
    // Defaults to int.MaxValue so batch parcels behave exactly as before
    {
        int    id      = 0;
        double length  = 0, breadth = 0, height = 0, weight = 0, value = 0;

        id      = ReadInt   ("Enter Parcel ID: ",      "Invalid input. Please enter a unique Parcel ID: ",   maxAttempts, extraCheck: v => parcels.Find(p => p.Id == v) == null);
        length  = ReadDouble("Enter Length: ",          "Invalid input. Please enter a valid Length: ",       maxAttempts);
        breadth = ReadDouble("Enter Breadth: ",         "Invalid input. Please enter a valid Breadth: ",      maxAttempts);
        height  = ReadDouble("Enter Height: ",          "Invalid input. Please enter a valid Height: ",       maxAttempts);
        weight  = ReadDouble("Enter Weight: ",          "Invalid input. Please enter a valid Weight: ",       maxAttempts);
        value   = ReadDouble("Enter Parcel Value: ",    "Invalid input. Please enter a valid Parcel Value: ", maxAttempts);

        return new Parcel(id, length, breadth, height, weight, value);
    }


    // ── Input helpers ─────────────────────────────────────────

    static int ReadInt(string prompt, string errorPrompt, int maxAttempts, Func<int, bool>? extraCheck = null)
    // Reads an integer, re-prompts on bad input, exits if attempts run out
    // extraCheck — optional extra rule (e.g. ID must be unique)
    {
        int attempts = 0;
        Console.Write(prompt);

        while (true)
        {
            string raw = Console.ReadLine() ?? "";
            bool parsed = int.TryParse(raw, out int result);
            bool passesExtra = extraCheck == null || extraCheck(result);
            // if no extra rule exists, treat it as passing automatically

            if (parsed && result > 0 && passesExtra)
                return result;
                // ↑ valid — hand the number back to CreateParcel

            attempts++;

            if (attempts >= maxAttempts)
            // hit the limit — say goodbye and stop the program
            {
                Console.WriteLine($"\nToo many invalid attempts. Please try again later. Goodbye!");
                Environment.Exit(0);
                // Environment.Exit(0) — 0 means "ended normally, not a crash"
            }

            Console.Write($"[Attempt {attempts}/{maxAttempts}] {errorPrompt}");
            // shows the user how many tries they have used
        }
    }

    static double ReadDouble(string prompt, string errorPrompt, int maxAttempts)
    // Same pattern as ReadInt but for decimals — no extra check needed here
    {
        int attempts = 0;
        Console.Write(prompt);

        while (true)
        {
            string raw = Console.ReadLine() ?? "";
            bool parsed = double.TryParse(raw, out double result);

            if (parsed && result > 0)
                return result;

            attempts++;

            if (attempts >= maxAttempts)
            {
                Console.WriteLine($"\nToo many invalid attempts. Please try again later. Goodbye!");
                Environment.Exit(0);
            }

            Console.Write($"[Attempt {attempts}/{maxAttempts}] {errorPrompt}");
        }
    }


    static void DisplayParcel(Parcel p)
    {
        Console.WriteLine("Parcel ID:         " + p.Id);
        Console.WriteLine("Weight:            " + p.Weight);
        Console.WriteLine("Dimensions Volume: " + p.Dimensions);
        Console.WriteLine("Value:             " + p.Value);
        Console.WriteLine("Delivered Status:  " + p.IsDelivered);
    }
}
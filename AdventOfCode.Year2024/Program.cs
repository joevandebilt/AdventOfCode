using AdventOfCode.Shared.Base;

namespace AdventOfCode.Year2024;

public class Program
{
    static void Main(string[] args)
    {
        var tasks = new List<Task>();

        //Get all the Assemblies
        List<AdventOfCodeDay> assemblies = typeof(Program).Assembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(AdventOfCodeDay)) && !t.IsAbstract)
            .Select(t => (AdventOfCodeDay)Activator.CreateInstance(t)!).ToList();

        Console.WriteLine($"Got {assemblies.Count} days to run code for");

        //Run the tasks
        tasks.AddRange(assemblies.Select(a => a.Run()));
        Task.WaitAll(tasks.ToArray());

        Console.WriteLine("\n\n\t---Results---\n\n");

        //Print results
        tasks.AddRange(assemblies.Select(a => a.PrintResult()));
        Task.WaitAll(tasks.ToArray());
    }
}
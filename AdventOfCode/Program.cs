using AdventOfCode.Days;

namespace AdventOfCode;

public class Program
{
    static void Main(string[] args)
    {
        var tasks = new List<Task>();

        //Get all the Assemblies
        List<AdventOfCodeDay> assemblies = typeof(AdventOfCodeDay).Assembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(AdventOfCodeDay)) && !t.IsAbstract)
            .Select(t => (AdventOfCodeDay)Activator.CreateInstance(t)!).ToList();

        //Run the tasks
        tasks.AddRange(assemblies.Select(a => a.Run()));
        Task.WaitAll(tasks.ToArray());

        Console.WriteLine("\n\n\t---Results---\n\n");

        //Print results
        tasks.AddRange(assemblies.Select(a => a.PrintResult()));
        Task.WaitAll(tasks.ToArray());
    }
}
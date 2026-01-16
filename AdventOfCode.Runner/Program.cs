using AdventOfCode.Shared.Base;
using AdventOfCode.Year2015;
using System.Reflection;

public class Program
{
    static void Main(string[] args)
    {
        var tasks = new List<Task>();

        //Get all the runners
        var yearAssemblies = Directory.EnumerateFiles(AppDomain.CurrentDomain.BaseDirectory, "AdventOfCode.Year*.dll")
            .Select(assemblyPath => AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(assemblyPath)))
            .OrderByDescending(a => a.GetName().Name)
            .ToList();

        Console.Clear();
        Console.WriteLine($"Welcome to Advent of Code! - Found {yearAssemblies.Count} year runner projects");
        Console.WriteLine("Please Select a year\n\r> \t{0}", string.Join("\n\r\t", yearAssemblies.Select(y => y.GetName().Name)));
        
        int firstLine = 2;
        int selected = 0;
        ConsoleKey key;
        do
        {
            key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.UpArrow)
            {
                if (selected > 0)
                {
                    selected--;
                }
            }
            else if (key == ConsoleKey.DownArrow)
            {
                if (selected < yearAssemblies.Count-1)
                {
                    selected++;
                }
            }

            Console.SetCursorPosition(0, firstLine);
            for (int i = 0; i < yearAssemblies.Count; i++)
            {
                if (i == selected)
                {
                    Console.Write(">");
                }
                Console.WriteLine(" \t{0}", yearAssemblies[i].GetName().Name);
            }
        } while (key != ConsoleKey.Enter);

        Console.Clear();

        var runnerType = typeof(AdventOfCodeDay);
        var selectedYearAssembly = yearAssemblies[selected];        
        var runners = selectedYearAssembly.GetTypes()
                .Where(type => runnerType.IsAssignableFrom(type) && !type.IsAbstract && type != runnerType)
                .Select(type => (AdventOfCodeDay)Activator.CreateInstance(type)!)
                .ToList();

        Console.WriteLine($"Got {runners.Count} days to run code for");

        
        //Run the tasks
        tasks.AddRange(runners.Where(a => a.Validate()).Select(a => a.Run()));
        Task.WaitAll(tasks.ToArray());

        Console.WriteLine("\n\n\t---Results---\n\n");

        //Print results
        tasks.AddRange(runners.OrderBy(a => a.DayOfAdvent).Select(a => a.PrintResult()));
        Task.WaitAll(tasks.ToArray());
    }
}
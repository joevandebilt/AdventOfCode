using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;
using AdventOfCode.Shared.Models;

namespace AdventOfCode.Year2025.Days.DayEight;
public class DayEightMain : AdventOfCodeDay
{
    private const bool _debugging = false;
    public DayEightMain() : base(Day.Eight, _debugging) { }

    public override async Task Run()
    {
        var linesOfInput = await LoadFile();
        List<Vector> junctions = new List<Vector>();
        

        foreach (var line in linesOfInput)
        {
            junctions.Add(new Vector(line));
        }
       
        WriteLine($"Loaded {junctions.Count} junction boxes.");

        var connections = junctions.SelectMany(A => junctions.Where(B => B.Reference != A.Reference).Select(B => new Connection(A, B))).Distinct().OrderBy(c => c.Distance).ToList();
        var connectionCount = 0;

        List<Circuit> circuits = junctions.Select(j => new Circuit() { JunctionBoxes = new List<Vector> { j } }).ToList();
        while (circuits.Count > 1)
        {
            ResetCursor();

            var bestConnection = connections.First();
            var junction = bestConnection.JunctionA;
            var closest = bestConnection.JunctionB;
            connections.Remove(bestConnection);

            WriteLine($"Junction {junction.Reference} is closest to Junction {closest.Reference} at distance {junction.Distance(closest)}");            

            //Does our closest junction belong to a circuit already?
            var circuit = circuits.FirstOrDefault(c => c.Contains(junction));
            var closestCircuit = circuits.FirstOrDefault(c => c.Contains(closest));

            if (circuit == closestCircuit)
            {
                WriteLine("\tNo Action - junctions are in the same circuit");
            }
            else if (circuit != null && closestCircuit != null && circuit != closestCircuit)
            {
                WriteLine("\t2 Circuits - Merging");
                circuit.JunctionBoxes.AddRange(closestCircuit.JunctionBoxes);
                circuits.Remove(closestCircuit);
            }

            connectionCount++;
            if (connectionCount == 1000)
            {
                var top3 = circuits.OrderByDescending(c => c.JunctionBoxes.Count).Take(3).ToList();
                var product = top3.Select(t => t.JunctionBoxes.Count).Aggregate(1, (a, b) => a * b);

                SetResult1(product);
            }

            if (circuits.Count == 1)
            {
                //Final connection made
                WriteLine($"All junctions connected into a single circuit.");
                WriteLine($"\tFinal pair was {junction.Reference} & {closest.Reference}");
                SetResult2(junction.X * closest.X);
            }

            WriteLine($"Connections Processed: {connectionCount} - Number of distinct circuits {circuits.Count.ToString().PadLeft(3, '0')}");
        }
        WriteLine($"Created {circuits.Count} circuits containing {circuits.Sum(c => c.JunctionBoxes.Count)} junction boxes");
        await base.Run();
    }
}

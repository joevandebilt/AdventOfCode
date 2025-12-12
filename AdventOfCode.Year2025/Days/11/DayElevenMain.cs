using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;

namespace AdventOfCode.Year2025.Days.DayEleven;
public class DayElevenMain : AdventOfCodeDay
{
    private const bool _debugging = false;
    public DayElevenMain() : base(Day.Eleven, _debugging) { }

    private Dictionary<string, long> cachedPaths = new();

    public override async Task Run()
    {
        var linesOfInput = await LoadFile();
        var devices = linesOfInput.Select(line => new Device(line)).ToList();

        foreach(var device in devices)
        {
            foreach (var outputName in device.OutputConnections)
            {
                var outputDevice = devices.FirstOrDefault(d => d.Name == outputName);
                if (outputDevice != null)
                    device.Outputs.Add(outputDevice);
            }
        }

        var firstNode = devices.First(d => d.Name == "you");
        var totalPaths = TraverseNode(firstNode, new HashSet<Device>(), "out");

        SetResult1(totalPaths);

        var svrNode = devices.First(d => d.Name == "svr");
        var dacNode = devices.First(d => d.Name == "dac");
        var fftNode = devices.First(d => d.Name == "fft");

        var svrDacRoutes = TraverseNode(svrNode, new HashSet<Device>(), "dac");
        var svrFftRoutes = TraverseNode(svrNode, new HashSet<Device>(), "fft");
        
        var dacFftRoutes = TraverseNode(dacNode, new HashSet<Device>(), "fft");
        var dacOutRoutes = TraverseNode(dacNode, new HashSet<Device>(), "out");
        
        var fftDacRoutes = TraverseNode(fftNode, new HashSet<Device>(), "dac");
        var fftOutRoutes = TraverseNode(fftNode, new HashSet<Device>(), "out");        

        long paths = (svrDacRoutes * dacFftRoutes * fftDacRoutes) + (svrFftRoutes * fftDacRoutes * dacOutRoutes);

        SetResult2(paths);
        await base.Run();
    }

    private long TraverseNode(Device device, HashSet<Device> visited, string target)
    {
        if (visited.Contains(device))
        {
            //Going round in circles
            return 0;
        }

        var cacheKey = $"{device.Name}-{target}";
        if (cachedPaths.ContainsKey(cacheKey))
        {
            return cachedPaths[cacheKey];
        }

        visited.Add(device);
        long total = 0;
        foreach (var output in device.OutputConnections)
        {
            if (output == target)
            {
                total++;
                WriteLine($"{string.Join(" -> ", visited.Select(v => v.Name))} -> {output}");
            }
            else
            {
                var nextNode = device.Outputs.FirstOrDefault(d => d.Name == output);
                if (nextNode != null)
                {
                    total += TraverseNode(nextNode, visited.ToHashSet(), target);
                }
            }
        }

        cachedPaths.Add(cacheKey, total);
        return total;
    }
}

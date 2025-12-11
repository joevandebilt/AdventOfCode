namespace AdventOfCode.Year2025.Days.DayTen;

public record Machine
{
    public Machine() { }
    public Machine(string input)
    {
        var parts = input.Split(' ');
        foreach(var part in parts)
        {
            if (part.StartsWith('['))
            {
                var lights = part.Trim('[', ']');
                foreach (var light in lights)
                {
                    if (light == '#')
                    {
                        IndicatorLights.Add(true);
                    }
                    else
                    {
                        IndicatorLights.Add(false);
                    }                    
                }
            }
            else if (part.StartsWith('('))
            {
                var buttons = part.Trim('(', ')').Split(',');
                var button = new Button();
                foreach (var btn in buttons)
                {                    
                    button.Wires.Add(int.Parse(btn));
                }
                Buttons.Add(button);
            }
            else if (part.StartsWith('{'))
            {
                var requirements = part.Trim('{', '}').Split(',');
                foreach (var req in requirements)
                {
                    Requirements.Add(int.Parse(req));
                }
            }
        }
    }
    
    public List<bool> IndicatorLights { get; set; } = new();
    public List<Button> Buttons { get; set; } = new();
    public List<int> Requirements { get; set; } = new();
    public List<int> ButtonsPressed { get; set; } = new();
    public int PressedCount => ButtonsPressed.Count;

    public bool Safe => Requirements.All(req => req >= 0);
    public bool Ready => IndicatorLights.All(lights => lights == false); //&& Requirements.All(req => req == 0);

    public void PushButton(int idx)
    {
        var schematic = Buttons[idx];
        foreach (var lightIdx in schematic.Wires)
        {
            IndicatorLights[lightIdx] = !IndicatorLights[lightIdx];
            Requirements[lightIdx]--;
        }
        ButtonsPressed.Add(idx);
    }

    public Machine Copy()
    {
        var newMachine = new Machine
        {
            IndicatorLights = new List<bool>(IndicatorLights),
            Buttons = Buttons.Select(b => new Button { Wires = new List<int>(b.Wires) }).ToList(),
            Requirements = new List<int>(Requirements),
            ButtonsPressed = new List<int>(ButtonsPressed)
        };
        return newMachine;
    }
} 

public record Button
{
    public List<int> Wires { get; set; } = new();
}

namespace fftivc.utility.jobtreeeditor.uib;

public class JobPosition(int x, int y)
{
    public int X { get; set; } = x;
    public int Y { get; set; } = y;

    public JobPosition Clone() => new(X, Y);

    public override string ToString() => $"({X}, {Y})";
}

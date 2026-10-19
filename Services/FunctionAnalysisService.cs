using System.Globalization;

namespace FunktsUurimine.Services;

public sealed class AnalysisResult
{
    public string Domain { get; init; } = "R";
    public string Derivative { get; init; } = "";
    public string Zeros { get; init; } = "puuduvad";
    public string CriticalPoints { get; init; } = "puuduvad";
    public string Extrema { get; init; } = "puuduvad";
}

public sealed class FunctionAnalysisService
{
    private const double MinX = -20;
    private const double MaxX = 20;
    private const double Step = 0.02;

    public AnalysisResult Analyze(string formula)
    {
        var ast = ExpressionParser.Parse(formula);
        var derivative = ast.Derivative();
        var derivativeText = Simplify(derivative.Format());

        var zeros = FindRoots(ast);
        var critical = FindRoots(derivative);
        var extrema = ClassifyExtrema(ast, derivative, critical);
        var domain = EstimateDomain(ast);

        return new AnalysisResult
        {
            Domain = domain,
            Derivative = $"f'(x) = {derivativeText}",
            Zeros = FormatPoints(zeros),
            CriticalPoints = FormatPoints(critical),
            Extrema = extrema
        };
    }

    public static double Evaluate(Expr expression, double x)
    {
        var value = expression.Eval(x);
        return double.IsFinite(value) ? value : double.NaN;
    }

    private static List<double> FindRoots(Expr expression)
    {
        var roots = new List<double>();
        double prevX = MinX;
        double prevY = Evaluate(expression, prevX);

        for (double x = MinX + Step; x <= MaxX; x += Step)
        {
            var y = Evaluate(expression, x);
            if (double.IsFinite(prevY) && double.IsFinite(y))
            {
                if (Math.Abs(prevY) < 1e-5) AddUnique(roots, prevX);
                if (prevY * y < 0) AddUnique(roots, Bisect(expression, prevX, x));
            }
            prevX = x;
            prevY = y;
        }
        if (double.IsFinite(prevY) && Math.Abs(prevY) < 1e-5) AddUnique(roots, prevX);
        return roots;
    }

    private static double Bisect(Expr expression, double a, double b)
    {
        var fa = Evaluate(expression, a);
        for (var i = 0; i < 80; i++)
        {
            var c = (a + b) / 2;
            var fc = Evaluate(expression, c);
            if (!double.IsFinite(fc)) { a = c; continue; }
            if (Math.Abs(fc) < 1e-10) return c;
            if (fa * fc <= 0) b = c;
            else { a = c; fa = fc; }
        }
        return (a + b) / 2;
    }

    private static string ClassifyExtrema(Expr function, Expr derivative, List<double> critical)
    {
        var values = new List<string>();
        foreach (var x in critical)
        {
            var left = Evaluate(derivative, x - 0.001);
            var right = Evaluate(derivative, x + 0.001);
            var y = Evaluate(function, x);
            if (!double.IsFinite(left) || !double.IsFinite(right) || !double.IsFinite(y)) continue;
            if (left > 0 && right < 0) values.Add($"maksimum: ({Fmt(x)}; {Fmt(y)})");
            else if (left < 0 && right > 0) values.Add($"miinimum: ({Fmt(x)}; {Fmt(y)})");
        }
        return values.Count == 0 ? "puuduvad" : string.Join(", ", values);
    }

    private static string EstimateDomain(Expr ast)
    {
        var invalid = new List<double>();
        double previous = MinX;
        var prevOk = double.IsFinite(Evaluate(ast, previous));
        for (double x = MinX + 0.05; x <= MaxX; x += 0.05)
        {
            var ok = double.IsFinite(Evaluate(ast, x));
            if (ok != prevOk) invalid.Add((previous + x) / 2);
            previous = x;
            prevOk = ok;
        }
        if (invalid.Count == 0) return "R (reaalarvud)";
        return "R välja arvatud ligikaudsed katkestuskohad: " + string.Join(", ", invalid.Select(Fmt));
    }

    private static string FormatPoints(IEnumerable<double> points)
        => points.Any() ? string.Join(", ", points.Select((x, i) => $"x{i + 1} = {Fmt(x)}")) : "puuduvad";

    private static string Fmt(double x) => x.ToString("0.####", CultureInfo.InvariantCulture);

    private static void AddUnique(List<double> list, double value)
    {
        if (double.IsFinite(value) && !list.Any(x => Math.Abs(x - value) < 0.01)) list.Add(value);
    }

    private static string Simplify(string text)
    {
        return text
            .Replace("1 * ", "")
            .Replace("(1 * ", "(")
            .Replace(" + 0", "")
            .Replace(" - 0", "")
            .Replace("0 + ", "")
            .Replace("(0 * ", "(0");
    }
}

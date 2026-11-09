using FunktsUurimine.Services;

namespace FunktsUurimine.Tests;

public class FunctionAnalysisServiceTests
{
    private readonly FunctionAnalysisService _service = new();

    [Fact]
    public void Evaluate_FiniteValue_ReturnsValue()
    {
        var expr = ExpressionParser.Parse("x^2");

        Assert.Equal(9, FunctionAnalysisService.Evaluate(expr, 3), 9);
    }

    [Theory]
    [InlineData("1/x", 0)]       // lõpmatus
    [InlineData("sqrt(x)", -1)]  // NaN
    [InlineData("ln(x)", 0)]     // miinus lõpmatus
    public void Evaluate_NonFiniteValue_ReturnsNaN(string formula, double x)
    {
        var expr = ExpressionParser.Parse(formula);

        Assert.True(double.IsNaN(FunctionAnalysisService.Evaluate(expr, x)));
    }

    [Fact]
    public void Analyze_QuadraticWithTwoRoots_ReturnsBothZeros()
    {
        var result = _service.Analyze("x^2-4");

        Assert.Equal("x1 = -2, x2 = 2", result.Zeros);
    }

    [Fact]
    public void Analyze_PerfectSquare_ReturnsDoubleRootAsSingleZeroAndMinimum()
    {
        // (x-3)^2 puudutab telge punktis 3 ega muuda märki
        var result = _service.Analyze("x^2-6*x+9");

        Assert.Equal("x1 = 3", result.Zeros);
        Assert.Equal("x1 = 3", result.CriticalPoints);
        // y-väärtus on ligikaudu 0 (ujukomaviga võib anda "0" või "-0"), seetõttu kontrollitakse ainult liiki ja x-koordinaati
        Assert.StartsWith("miinimum: (3; ", result.Extrema);
    }

    [Fact]
    public void Analyze_DownwardParabola_ReturnsMaximum()
    {
        var result = _service.Analyze("4*x-x^2");

        Assert.Equal("x1 = 2", result.CriticalPoints);
        Assert.Equal("maksimum: (2; 4)", result.Extrema);
    }

    [Fact]
    public void Analyze_PolynomialWithoutZerosInRange_ReturnsNoZeros()
    {
        var result = _service.Analyze("x^2+1");

        Assert.Equal("puuduvad", result.Zeros);
    }

    [Fact]
    public void Analyze_LinearFunctionWithRootOutsideRange_ReturnsNoZerosCriticalPointsOrExtrema()
    {
        var result = _service.Analyze("x+100");

        Assert.Equal("puuduvad", result.Zeros);
        Assert.Equal("puuduvad", result.CriticalPoints);
        Assert.Equal("puuduvad", result.Extrema);
        Assert.Equal("R (reaalarvud)", result.Domain);
    }

    [Fact]
    public void Analyze_Sine_FindsAllThirteenZerosInRange()
    {
        // sin(x) nullkohad vahemikus [-20, 20]: k*pi, k = -6..6
        var result = _service.Analyze("sin(x)");

        Assert.Equal(13, result.Zeros.Split(", ").Length);
        Assert.Contains("x13 = 18.8496", result.Zeros);
        Assert.Equal("R (reaalarvud)", result.Domain);
    }

    [Fact]
    public void Analyze_EverywhereDefinedFunction_ReportsAllRealsAsDomain()
    {
        var result = _service.Analyze("x^2");

        Assert.Equal("R (reaalarvud)", result.Domain);
    }

    [Fact]
    public void Analyze_LogarithmDefinedOnlyForPositiveX_ReportsOneDiscontinuity()
    {
        var result = _service.Analyze("ln(x)");

        Assert.StartsWith("R välja arvatud ligikaudsed katkestuskohad:", result.Domain);
        Assert.Single(result.Domain.Split(':')[1].Split(','));
    }

    [Fact]
    public void Analyze_SquareRootOfUnitCircle_ReportsTwoBoundaryPoints()
    {
        // sqrt(1-x^2) on määratud ainult lõigus [-1; 1]
        var result = _service.Analyze("sqrt(1-x^2)");

        Assert.StartsWith("R välja arvatud ligikaudsed katkestuskohad:", result.Domain);
        Assert.Equal(2, result.Domain.Split(':')[1].Split(',').Length);
    }

    [Fact]
    public void Analyze_ValidFormula_ReturnsDerivativeWithPrefix()
    {
        var result = _service.Analyze("sin(x)");

        Assert.StartsWith("f'(x) = ", result.Derivative);
        Assert.Contains("cos(x)", result.Derivative);
    }

    [Fact]
    public void Analyze_EmptyFormula_ThrowsExpressionParseException()
    {
        Assert.Throws<ExpressionParseException>(() => _service.Analyze(""));
    }

    [Fact]
    public void Analyze_InvalidSyntax_ThrowsExpressionParseException()
    {
        Assert.Throws<ExpressionParseException>(() => _service.Analyze("x+*2"));
    }

    [Fact]
    public void Analyze_FormulaWithUnsupportedDerivative_ThrowsExpressionParseException()
    {
        Assert.Throws<ExpressionParseException>(() => _service.Analyze("2^x"));
    }
}

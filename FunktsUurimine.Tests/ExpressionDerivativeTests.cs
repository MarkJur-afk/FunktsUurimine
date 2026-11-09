using FunktsUurimine.Services;

namespace FunktsUurimine.Tests;

public class ExpressionDerivativeTests
{
    // Tuletist kontrollitakse arvuliselt (väärtus punktis x), mitte tekstikuju järgi,
    // et testid ei sõltuks tuletise sisemisest kujust.
    [Theory]
    [InlineData("5", 1, 0)]
    [InlineData("x", 7, 1)]
    [InlineData("x^2", 3, 6)]
    [InlineData("x^3", 2, 12)]
    [InlineData("3*x^2", 2, 12)]
    [InlineData("2*x+1", 5, 2)]
    [InlineData("x^2-6*x+9", 3, 0)]
    [InlineData("(x+1)^2", 2, 6)]
    [InlineData("x*x", 4, 8)]
    [InlineData("1/x", 2, -0.25)]
    [InlineData("x/(x+1)", 1, 0.25)]
    public void Derivative_PolynomialAndRationalExpressions_ReturnsExpectedSlope(string formula, double x, double expected)
    {
        var derivative = ExpressionParser.Parse(formula).Derivative();

        Assert.Equal(expected, derivative.Eval(x), 9);
    }

    [Theory]
    [InlineData("sin(x)", 0, 1)]
    [InlineData("cos(x)", Math.PI / 2, -1)]
    [InlineData("tan(x)", 0, 1)]
    [InlineData("sqrt(x)", 4, 0.25)]
    [InlineData("ln(x)", 2, 0.5)]
    [InlineData("log(x)", 10, 0.04342944819)]
    [InlineData("exp(x)", 1, Math.E)]
    [InlineData("abs(x)", -3, -1)]
    [InlineData("abs(x)", 3, 1)]
    public void Derivative_ElementaryFunction_ReturnsExpectedSlope(string formula, double x, double expected)
    {
        var derivative = ExpressionParser.Parse(formula).Derivative();

        Assert.Equal(expected, derivative.Eval(x), 8);
    }

    [Fact]
    public void Derivative_CompositeFunction_AppliesChainRule()
    {
        // d/dx sin(x^2) = cos(x^2) * 2x; punktis x = 1 on see 2*cos(1)
        var derivative = ExpressionParser.Parse("sin(x^2)").Derivative();

        Assert.Equal(2 * Math.Cos(1), derivative.Eval(1), 9);
    }

    [Theory]
    [InlineData("2^x")]
    [InlineData("x^x")]
    public void Derivative_NonConstantExponent_ThrowsExpressionParseException(string formula)
    {
        var expr = ExpressionParser.Parse(formula);

        Assert.Throws<ExpressionParseException>(() => expr.Derivative());
    }
}

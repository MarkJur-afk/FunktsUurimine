using System.Globalization;

namespace FunktsUurimine.Services;

public sealed class ExpressionParseException(string message) : Exception(message);

public abstract record Expr
{
    public abstract double Eval(double x);
    public abstract Expr Derivative();
    public abstract string Format();
}

public sealed record NumberExpr(double Value) : Expr
{
    public override double Eval(double x) => Value;
    public override Expr Derivative() => new NumberExpr(0);
    public override string Format() => Value.ToString("G", CultureInfo.InvariantCulture);
}

public sealed record VariableExpr : Expr
{
    public override double Eval(double x) => x;
    public override Expr Derivative() => new NumberExpr(1);
    public override string Format() => "x";
}

public sealed record UnaryExpr(char Op, Expr Inner) : Expr
{
    public override double Eval(double x) => Op == '-' ? -Inner.Eval(x) : Inner.Eval(x);
    public override Expr Derivative() => Op == '-' ? new UnaryExpr('-', Inner.Derivative()) : Inner.Derivative();
    public override string Format() => $"({Op}{Inner.Format()})";
}

public sealed record BinaryExpr(char Op, Expr Left, Expr Right) : Expr
{
    public override double Eval(double x) => Op switch
    {
        '+' => Left.Eval(x) + Right.Eval(x),
        '-' => Left.Eval(x) - Right.Eval(x),
        '*' => Left.Eval(x) * Right.Eval(x),
        '/' => Left.Eval(x) / Right.Eval(x),
        '^' => Math.Pow(Left.Eval(x), Right.Eval(x)),
        _ => throw new InvalidOperationException()
    };

    public override Expr Derivative() => Op switch
    {
        '+' => new BinaryExpr('+', Left.Derivative(), Right.Derivative()),
        '-' => new BinaryExpr('-', Left.Derivative(), Right.Derivative()),
        '*' => new BinaryExpr('+', new BinaryExpr('*', Left.Derivative(), Right), new BinaryExpr('*', Left, Right.Derivative())),
        '/' => new BinaryExpr('/', new BinaryExpr('-', new BinaryExpr('*', Left.Derivative(), Right), new BinaryExpr('*', Left, Right.Derivative())), new BinaryExpr('^', Right, new NumberExpr(2))),
        '^' when Right is NumberExpr n => new BinaryExpr('*', new BinaryExpr('*', new NumberExpr(n.Value), new BinaryExpr('^', Left, new NumberExpr(n.Value - 1))), Left.Derivative()),
        _ => throw new ExpressionParseException("Sümboolset tuletist ei saa selle astme jaoks arvutada.")
    };

    public override string Format() => $"({Left.Format()} {Op} {Right.Format()})";
}

public sealed record FunctionExpr(string Name, Expr Argument) : Expr
{
    public override double Eval(double x)
    {
        var a = Argument.Eval(x);
        return Name switch
        {
            "sin" => Math.Sin(a),
            "cos" => Math.Cos(a),
            "tan" => Math.Tan(a),
            "sqrt" => Math.Sqrt(a),
            "ln" => Math.Log(a),
            "log" => Math.Log10(a),
            "exp" => Math.Exp(a),
            "abs" => Math.Abs(a),
            _ => throw new ExpressionParseException($"Tundmatu funktsioon: {Name}")
        };
    }

    public override Expr Derivative()
    {
        var d = Argument.Derivative();
        Expr outer = Name switch
        {
            "sin" => new FunctionExpr("cos", Argument),
            "cos" => new UnaryExpr('-', new FunctionExpr("sin", Argument)),
            "tan" => new BinaryExpr('/', new NumberExpr(1), new BinaryExpr('^', new FunctionExpr("cos", Argument), new NumberExpr(2))),
            "sqrt" => new BinaryExpr('/', new NumberExpr(1), new BinaryExpr('*', new NumberExpr(2), new FunctionExpr("sqrt", Argument))),
            "ln" => new BinaryExpr('/', new NumberExpr(1), Argument),
            "log" => new BinaryExpr('/', new NumberExpr(1), new BinaryExpr('*', Argument, new NumberExpr(Math.Log(10)))),
            "exp" => new FunctionExpr("exp", Argument),
            "abs" => new BinaryExpr('/', Argument, new FunctionExpr("abs", Argument)),
            _ => throw new ExpressionParseException($"Funktsiooni {Name} tuletist ei toetata.")
        };
        return new BinaryExpr('*', outer, d);
    }

    public override string Format() => $"{Name}({Argument.Format()})";
}

public sealed class ExpressionParser
{
    private readonly string _text;
    private int _pos;

    private ExpressionParser(string text) => _text = text.Replace(" ", "").ToLowerInvariant();

    public static Expr Parse(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) throw new ExpressionParseException("Valem ei tohi olla tühi.");
        var p = new ExpressionParser(text);
        var result = p.ParseExpression();
        if (p._pos != p._text.Length) throw new ExpressionParseException($"Vigane süntaks positsioonil {p._pos + 1}.");
        return result;
    }

    private Expr ParseExpression()
    {
        var left = ParseTerm();
        while (Peek('+') || Peek('-'))
        {
            var op = Next();
            left = new BinaryExpr(op, left, ParseTerm());
        }
        return left;
    }

    private Expr ParseTerm()
    {
        var left = ParsePower();
        while (Peek('*') || Peek('/'))
        {
            var op = Next();
            left = new BinaryExpr(op, left, ParsePower());
        }
        return left;
    }

    private Expr ParsePower()
    {
        var left = ParseUnary();
        if (Peek('^'))
        {
            Next();
            left = new BinaryExpr('^', left, ParsePower());
        }
        return left;
    }

    private Expr ParseUnary()
    {
        if (Peek('+')) { Next(); return ParseUnary(); }
        if (Peek('-')) { Next(); return new UnaryExpr('-', ParseUnary()); }
        return ParsePrimary();
    }

    private Expr ParsePrimary()
    {
        if (Peek('('))
        {
            Next();
            var e = ParseExpression();
            Expect(')');
            return e;
        }

        if (char.IsDigit(Current()) || Current() == '.') return ParseNumber();
        if (Current() == 'x') { Next(); return new VariableExpr(); }
        if (char.IsLetter(Current())) return ParseFunction();
        throw new ExpressionParseException($"Ootamatu sümbol positsioonil {_pos + 1}: '{Current()}'.");
    }

    private Expr ParseNumber()
    {
        var start = _pos;
        while (char.IsDigit(Current()) || Current() == '.') Next();
        var s = _text[start.._pos];
        if (!double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out var value))
            throw new ExpressionParseException($"Vigane number: {s}");
        return new NumberExpr(value);
    }

    private Expr ParseFunction()
    {
        var start = _pos;
        while (char.IsLetter(Current())) Next();
        var name = _text[start.._pos];
        if (name == "pi") return new NumberExpr(Math.PI);
        if (name == "e") return new NumberExpr(Math.E);
        if (!name.Equals("sin") && !name.Equals("cos") && !name.Equals("tan") && !name.Equals("sqrt") && !name.Equals("ln") && !name.Equals("log") && !name.Equals("exp") && !name.Equals("abs"))
            throw new ExpressionParseException($"Tundmatu funktsioon: {name}");
        Expect('(');
        var arg = ParseExpression();
        Expect(')');
        return new FunctionExpr(name, arg);
    }

    private char Current() => _pos < _text.Length ? _text[_pos] : '\0';
    private bool Peek(char c) => Current() == c;
    private char Next() => _pos < _text.Length ? _text[_pos++] : '\0';
    private void Expect(char c)
    {
        if (!Peek(c)) throw new ExpressionParseException($"Oodatud '{c}' positsioonil {_pos + 1}.");
        Next();
    }
}

using System.ComponentModel.DataAnnotations;
using FunktsUurimine.Models;

namespace FunktsUurimine.Tests;

public class FunktsiooniUurimineValidationTests
{
    private static List<ValidationResult> Validate(FunktsiooniUurimine model)
    {
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(model, new ValidationContext(model), results, validateAllProperties: true);
        return results;
    }

    [Fact]
    public void Validate_NormalFormula_HasNoErrors()
    {
        var model = new FunktsiooniUurimine { Valem = "x^2-6*x+9" };

        Assert.Empty(Validate(model));
    }

    [Fact]
    public void Validate_EmptyFormula_ReturnsRequiredError()
    {
        var model = new FunktsiooniUurimine { Valem = string.Empty };

        var errors = Validate(model);

        var error = Assert.Single(errors);
        Assert.Equal("Funktsiooni valem on kohustuslik.", error.ErrorMessage);
        Assert.Contains(nameof(FunktsiooniUurimine.Valem), error.MemberNames);
    }

    [Fact]
    public void Validate_FormulaOfMaxLength200_HasNoErrors()
    {
        var model = new FunktsiooniUurimine { Valem = new string('x', 200) };

        Assert.Empty(Validate(model));
    }

    [Fact]
    public void Validate_FormulaLongerThan200_ReturnsLengthError()
    {
        var model = new FunktsiooniUurimine { Valem = new string('x', 201) };

        var error = Assert.Single(Validate(model));

        Assert.Equal("Valem võib olla kuni 200 märki pikk.", error.ErrorMessage);
    }

    [Fact]
    public void NewInstance_Defaults_AreEmptyStringsAndUtcCreationTime()
    {
        var before = DateTime.UtcNow;

        var model = new FunktsiooniUurimine();

        Assert.Equal(string.Empty, model.Valem);
        Assert.Equal(string.Empty, model.Tuletis);
        Assert.InRange(model.LuodudAeg, before, DateTime.UtcNow);
    }
}

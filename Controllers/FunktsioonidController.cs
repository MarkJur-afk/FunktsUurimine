using FunktsUurimine.Data;
using FunktsUurimine.Models;
using FunktsUurimine.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FunktsUurimine.Controllers;

public class FunktsioonidController(AppDbContext context, FunctionAnalysisService analysis) : Controller
{
    public async Task<IActionResult> Index()
    {
        var items = await context.FunktsiooniUurimised
            .AsNoTracking()
            .OrderByDescending(x => x.LuodudAeg)
            .ToListAsync();
        return View(items);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null) return NotFound();
        var item = await context.FunktsiooniUurimised.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return item is null ? NotFound() : View(item);
    }

    [HttpGet]
    public IActionResult Create() => View(new FunktsiooniUurimine());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(FunktsiooniUurimine model)
    {
        model.Valem = model.Valem.Trim();
        if (!ModelState.IsValid) return View(model);

        try
        {
            var result = analysis.Analyze(model.Valem);
            Apply(model, result);
            context.Add(model);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        catch (ExpressionParseException ex)
        {
            ModelState.AddModelError(nameof(model.Valem), ex.Message);
            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null) return NotFound();
        var item = await context.FunktsiooniUurimised.FindAsync(id);
        return item is null ? NotFound() : View(item);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, FunktsiooniUurimine model)
    {
        if (id != model.Id) return NotFound();
        model.Valem = model.Valem.Trim();
        if (!ModelState.IsValid) return View(model);

        var item = await context.FunktsiooniUurimised.FindAsync(id);
        if (item is null) return NotFound();

        try
        {
            var result = analysis.Analyze(model.Valem);
            item.Valem = model.Valem;
            Apply(item, result);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        catch (ExpressionParseException ex)
        {
            ModelState.AddModelError(nameof(model.Valem), ex.Message);
            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null) return NotFound();
        var item = await context.FunktsiooniUurimised.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return item is null ? NotFound() : View(item);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await context.FunktsiooniUurimised.FindAsync(id);
        if (item is null) return NotFound();
        context.FunktsiooniUurimised.Remove(item);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private static void Apply(FunktsiooniUurimine model, AnalysisResult result)
    {
        model.Maaramispiirkond = result.Domain;
        model.Tuletis = result.Derivative;
        model.Nullkohad = result.Zeros;
        model.KriitilisedPunktid = result.CriticalPoints;
        model.Ekstreemumid = result.Extrema;
    }
}

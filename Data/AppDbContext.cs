using FunktsUurimine.Models;
using Microsoft.EntityFrameworkCore;

namespace FunktsUurimine.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<FunktsiooniUurimine> FunktsiooniUurimised => Set<FunktsiooniUurimine>();
}

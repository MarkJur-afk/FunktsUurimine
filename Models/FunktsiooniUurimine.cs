using System.ComponentModel.DataAnnotations;

namespace FunktsUurimine.Models;

public class FunktsiooniUurimine
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Funktsiooni valem on kohustuslik.")]
    [StringLength(200, ErrorMessage = "Valem võib olla kuni 200 märki pikk.")]
    [Display(Name = "Funktsiooni valem")]
    public string Valem { get; set; } = string.Empty;

    [Display(Name = "Määramispiirkond")]
    public string Maaramispiirkond { get; set; } = string.Empty;

    [Display(Name = "Tuletis")]
    public string Tuletis { get; set; } = string.Empty;

    [Display(Name = "Nullkohad")]
    public string Nullkohad { get; set; } = string.Empty;

    [Display(Name = "Kriitilised punktid")]
    public string KriitilisedPunktid { get; set; } = string.Empty;

    [Display(Name = "Ekstreemumid")]
    public string Ekstreemumid { get; set; } = string.Empty;

    [Display(Name = "Loodud")]
    public DateTime LuodudAeg { get; set; } = DateTime.UtcNow;
}

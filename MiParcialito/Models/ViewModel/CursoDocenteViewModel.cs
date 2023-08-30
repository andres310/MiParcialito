using System.ComponentModel.DataAnnotations;

namespace MiParcialito.Models.ViewModel;

public class CursoDocenteViewModel
{
    public int? Id { get; set; }
    [Display(Name = "Curso")]
    public string NombreCurso { get; set; }
    [Display(Name = "Docente")]
    public string NombreDocente { get; set; }
}
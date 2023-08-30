using System.ComponentModel.DataAnnotations;

namespace MiParcialito.Models.ViewModel;

public class InscripcionCursoViewModel
{
    // Id del curso
    public int? Id { get; set; }
    public string Curso { get; set; }
    public string? Docente { get; set; }
    [Display(Name = "Estudiantes inscritos")]
    public int? EstudiantesInscritos { get; set; }
}
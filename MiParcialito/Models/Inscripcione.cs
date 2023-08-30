using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MiParcialito.Models;

public partial class Inscripcione
{
    public int Id { get; set; }
    
    [Display(Name = "Id Estudiante")]
    public int? IdEstudiante { get; set; }

    [Display(Name = "Id Curso")]
    public int? IdCurso { get; set; }

    public virtual Curso? IdCursoNavigation { get; set; }

    public virtual Estudiante? IdEstudianteNavigation { get; set; }
}

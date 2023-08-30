using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MiParcialito.Models;

public partial class Curso
{
    public int Id { get; set; }

    [Display(Name = "Id Docente")]
    public int? IdDocente { get; set; }

    public string? Nombre { get; set; }

    public virtual Docente? IdDocenteNavigation { get; set; }
}

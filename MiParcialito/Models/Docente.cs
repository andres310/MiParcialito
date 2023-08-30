using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MiParcialito.Models;

public partial class Docente
{
    public int Id { get; set; }

    [Display(Name = "Id Usuario")]
    public int? IdUsuario { get; set; }

    public virtual ICollection<Curso> Cursos { get; set; } = new List<Curso>();

    public virtual AspNetUser? IdUsuarioNavigation { get; set; }
}

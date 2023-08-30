using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MiParcialito.Models;

public partial class Estudiante
{
    public int Id { get; set; }

    [Display(Name = "Id Usuario")]
    public int? IdUsuario { get; set; }

    public virtual AspNetUser? IdUsuarioNavigation { get; set; }
}

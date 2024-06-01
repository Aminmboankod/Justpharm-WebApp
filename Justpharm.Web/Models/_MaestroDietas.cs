using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Justpharm.Web.Models;

public partial class _MaestroDietas
{
    [Key]
    public Guid UidMaestroDieta { get; set; }

    public int Orden { get; set; }

    [StringLength(450)]
    public string? Descripcion { get; set; }

    [StringLength(450)]
    public string Titulo { get; set; } = null!;

    [InverseProperty("UidMaestroDietaNavigation")]
    public virtual ICollection<Paciente> Paciente { get; set; } = new List<Paciente>();
}

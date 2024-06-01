using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Justpharm.Web.Models;

[Index("UidPaciente", Name = "IX_EfectoSecundario")]
public partial class EfectoSecundario
{
    [Key]
    public Guid UidEfectoSecundario { get; set; }

    public Guid UidPaciente { get; set; }

    public Guid? IdTratamiento { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Titulo { get; set; }

    [StringLength(255)]
    public string? Descripcion { get; set; }

    [ForeignKey("UidPaciente")]
    [InverseProperty("EfectoSecundario")]
    public virtual Paciente UidPacienteNavigation { get; set; } = null!;
}

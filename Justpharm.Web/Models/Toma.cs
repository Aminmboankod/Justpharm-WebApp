using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Justpharm.Web.Models;

public partial class Toma
{
    [Key]
    public Guid UidToma { get; set; }

    public Guid? UidTratamiento { get; set; }

    public Guid UidMedicamento { get; set; }

    [StringLength(450)]
    public string UsuarioId { get; set; } = null!;

    [StringLength(250)]
    public string Titulo { get; set; } = null!;

    [StringLength(255)]
    public string? Descripcion { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime StartTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime EndTime { get; set; }

    [StringLength(250)]
    public string? Color { get; set; }

    public double? Intervalo { get; set; }

    public Guid? UidPaciente { get; set; }

    public bool? Ingerido { get; set; }

    public int? Mejora { get; set; }

    public double Estado { get; set; }

    [StringLength(450)]
    public string? EmailAviso { get; set; }

    [StringLength(450)]
    public string? ProfesionalSuminstr { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FechaSuministr { get; set; }

    [ForeignKey("UidMedicamento")]
    [InverseProperty("Toma")]
    public virtual Medicamento UidMedicamentoNavigation { get; set; } = null!;

    [ForeignKey("UidPaciente")]
    [InverseProperty("Toma")]
    public virtual Paciente? UidPacienteNavigation { get; set; }

    [ForeignKey("UidTratamiento")]
    [InverseProperty("Toma")]
    public virtual Tratamiento? UidTratamientoNavigation { get; set; }
}

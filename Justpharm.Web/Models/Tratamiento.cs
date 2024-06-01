using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Justpharm.Web.Models;

public partial class Tratamiento
{
    [Key]
    public Guid UidTratamiento { get; set; }

    public Guid? UidTomas { get; set; }

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

    [StringLength(255)]
    public string? RecurrenceRule { get; set; }

    public Guid? RecurrenceId { get; set; }

    [StringLength(255)]
    public string? RecurrenceException { get; set; }

    [StringLength(255)]
    public string? RecurrenceSumary { get; set; }

    public int Tomas { get; set; }

    public double? Intervalo { get; set; }

    [StringLength(250)]
    public string? Color { get; set; }

    public Guid? UidPaciente { get; set; }

    [InverseProperty("IdTratamientoNavigation")]
    public virtual ICollection<Notificacion> Notificacion { get; set; } = new List<Notificacion>();

    [InverseProperty("UidTratamientoNavigation")]
    public virtual ICollection<Toma> Toma { get; set; } = new List<Toma>();

    [ForeignKey("UidMedicamento")]
    [InverseProperty("Tratamiento")]
    public virtual Medicamento UidMedicamentoNavigation { get; set; } = null!;

    [ForeignKey("UidPaciente")]
    [InverseProperty("Tratamiento")]
    public virtual Paciente? UidPacienteNavigation { get; set; }

    [ForeignKey("UsuarioId")]
    [InverseProperty("Tratamiento")]
    public virtual AspNetUsers Usuario { get; set; } = null!;
}

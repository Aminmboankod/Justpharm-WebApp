using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Justpharm.Web.Models;

[Index("DNI", Name = "IX_Paciente", IsUnique = true)]
public partial class Paciente
{
    [Key]
    public Guid UidPaciente { get; set; }

    [StringLength(450)]
    public string? UserId { get; set; }

    public int? Edad { get; set; }

    public double? Altura { get; set; }

    public double? Peso { get; set; }

    public bool? Alergias { get; set; }

    public bool? EnfermedadCronica { get; set; }

    public bool? SexoMasculino { get; set; }

    public Guid? UidUsuarioGestor { get; set; }

    [StringLength(255)]
    public string Nombre { get; set; } = null!;

    [StringLength(255)]
    public string Apellidos { get; set; } = null!;

    [StringLength(50)]
    public string? DNI { get; set; }

    [StringLength(50)]
    public string? Ubicacion { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FechaNacimiento { get; set; }

    public Guid? UidMaestroDieta { get; set; }

    [InverseProperty("UidPacienteNavigation")]
    public virtual ICollection<EfectoSecundario> EfectoSecundario { get; set; } = new List<EfectoSecundario>();

    [InverseProperty("UidPacienteNavigation")]
    public virtual ICollection<Toma> Toma { get; set; } = new List<Toma>();

    [InverseProperty("UidPacienteNavigation")]
    public virtual ICollection<Tratamiento> Tratamiento { get; set; } = new List<Tratamiento>();

    [ForeignKey("UidMaestroDieta")]
    [InverseProperty("Paciente")]
    public virtual _MaestroDietas? UidMaestroDietaNavigation { get; set; }

    [ForeignKey("UidUsuarioGestor")]
    [InverseProperty("Paciente")]
    public virtual PerfilUsuario? UidUsuarioGestorNavigation { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Paciente")]
    public virtual AspNetUsers? User { get; set; }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Justpharm.Web.Models;

[Index("IdAspNetUser", Name = "IX_PerfilUsuario_1", IsUnique = true)]
public partial class PerfilUsuario
{
    [Key]
    public Guid UidPerfil { get; set; }

    [StringLength(255)]
    public string Nombre { get; set; } = null!;

    public Guid UidMaestroPerfil { get; set; }

    public string IdAspNetUser { get; set; } = null!;

    public Guid? UidCentro { get; set; }

    [ForeignKey("IdAspNetUser")]
    [InverseProperty("PerfilUsuario")]
    public virtual AspNetUsers IdAspNetUserNavigation { get; set; } = null!;

    [InverseProperty("UidUsuarioGestorNavigation")]
    public virtual ICollection<Paciente> Paciente { get; set; } = new List<Paciente>();

    [ForeignKey("UidCentro")]
    [InverseProperty("PerfilUsuario")]
    public virtual Centros? UidCentroNavigation { get; set; }

    [ForeignKey("UidMaestroPerfil")]
    [InverseProperty("PerfilUsuario")]
    public virtual _MaestroPerfil UidMaestroPerfilNavigation { get; set; } = null!;
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Justpharm.Web.Models;

public partial class _MaestroPerfil
{
    [Key]
    public Guid UidMaestroPerfil { get; set; }

    [StringLength(255)]
    public string? CodigoPerfil { get; set; }

    [StringLength(255)]
    public string Tipo { get; set; } = null!;

    public int? Ref { get; set; }

    [InverseProperty("UidMaestroPerfilNavigation")]
    public virtual ICollection<PerfilUsuario> PerfilUsuario { get; set; } = new List<PerfilUsuario>();
}

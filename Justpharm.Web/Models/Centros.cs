using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Justpharm.Web.Models;

public partial class Centros
{
    [Key]
    public Guid UidCentro { get; set; }

    [StringLength(255)]
    public string Nombre { get; set; } = null!;

    [StringLength(255)]
    public string? Referencia { get; set; }

    [InverseProperty("UidCentroNavigation")]
    public virtual ICollection<PerfilUsuario> PerfilUsuario { get; set; } = new List<PerfilUsuario>();
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Justpharm.Web.Models;

public partial class Notificacion
{
    [Key]
    public Guid UidNotificacion { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Titulo { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Descripcion { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? UsuarioId { get; set; }

    public Guid? IdTratamiento { get; set; }

    [ForeignKey("IdTratamiento")]
    [InverseProperty("Notificacion")]
    public virtual Tratamiento? IdTratamientoNavigation { get; set; }
}

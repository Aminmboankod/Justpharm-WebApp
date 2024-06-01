using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Justpharm.Web.Models;

public partial class Medicamento
{
    [Key]
    public Guid UidMedicamento { get; set; }

    [StringLength(255)]
    public string Nombre { get; set; } = null!;

    [StringLength(250)]
    public string CN { get; set; } = null!;

    [StringLength(255)]
    public string Laboratorio { get; set; } = null!;

    [StringLength(650)]
    public string? Imagen { get; set; }

    public string? RecetaHtml { get; set; }

    [InverseProperty("UidMedicamentoNavigation")]
    public virtual ICollection<Toma> Toma { get; set; } = new List<Toma>();

    [InverseProperty("UidMedicamentoNavigation")]
    public virtual ICollection<Tratamiento> Tratamiento { get; set; } = new List<Tratamiento>();
}

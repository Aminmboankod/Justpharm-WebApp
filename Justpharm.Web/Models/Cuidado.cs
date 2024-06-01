using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Justpharm.Web.Models;

public partial class Cuidado
{
    [Key]
    public Guid UidCuidado { get; set; }

    [StringLength(450)]
    public string Titulo { get; set; } = null!;

    [StringLength(450)]
    public string? Descripción { get; set; }

    public Guid UidMaestroCuidados { get; set; }

    public Guid UidPaciente { get; set; }

    public Guid UidTecnicoAsign { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime FechaCuidado { get; set; }

    public bool Aviso { get; set; }

    public bool Leido { get; set; }

    public string? ListadoNecesidades { get; set; }

    public double? Tension { get; set; }

    public double? Fiebre { get; set; }

    public double? Insulina { get; set; }
}

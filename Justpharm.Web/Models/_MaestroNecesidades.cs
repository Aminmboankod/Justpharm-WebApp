using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Justpharm.Web.Models;

public partial class _MaestroNecesidades
{
    [Key]
    public Guid UidNecesidades { get; set; }

    public int Orden { get; set; }

    [StringLength(450)]
    public string TituloNecesidad { get; set; } = null!;

    public int CategoriaCuidado { get; set; }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Justpharm.Web.Models;

public partial class _MaestroCuidados
{
    [Key]
    public Guid UidMaestroCuidados { get; set; }

    public int Orden { get; set; }

    public int Categoria { get; set; }

    [StringLength(450)]
    public string TituloCuidado { get; set; } = null!;
}

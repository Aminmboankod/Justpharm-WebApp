using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Justpharm.Web.Models;

public partial class Favoritos
{
    [Key]
    public Guid UidFavoritos { get; set; }

    public int? IdMedicamento { get; set; }

    [StringLength(450)]
    public string? UserId { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Favoritos")]
    public virtual AspNetUsers? User { get; set; }
}

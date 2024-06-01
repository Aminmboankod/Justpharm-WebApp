using System;
using System.Collections.Generic;

namespace Justpharm.Web.Models;

public partial class Favorito
{
    public Guid UidFavoritos { get; set; }

    public int? IdMedicamento { get; set; }

    public string UserId { get; set; }

    public virtual AspNetUser User { get; set; }
}

using System;
using System.Collections.Generic;

namespace rotav1.Models;

public partial class UsuarioContato
{
    public int Usuarioid { get; set; }

    public int Contatoid { get; set; }

    public virtual Contato Contato { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
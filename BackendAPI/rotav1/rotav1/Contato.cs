using System;
using System.Collections.Generic;

namespace rotav1;

public partial class Contato
{
    public int Contatoid { get; set; }

    public int Usuarioid { get; set; }

    public string Nome { get; set; } = null!;

    public string Numerowhatsapp { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}

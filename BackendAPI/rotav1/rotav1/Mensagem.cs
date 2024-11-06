using System;
using System.Collections.Generic;

namespace rotav1;

public partial class Mensagem
{
    public int Mensagemid { get; set; }

    public int Remetenteid { get; set; }

    public int Destinatarioid { get; set; }

    public string Conteudo { get; set; } = null!;

    public DateTime? Timestamp { get; set; }

    public virtual Usuario Destinatario { get; set; } = null!;

    public virtual Usuario Remetente { get; set; } = null!;

    public virtual ICollection<UsuarioMensagem> UsuarioMensagems { get; set; } = new List<UsuarioMensagem>();
}

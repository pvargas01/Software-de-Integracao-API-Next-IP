using System;
using System.Collections.Generic;

namespace rotav1.Models;

public partial class UsuarioMensagem
{
    public int Usuarioid { get; set; }

    public int Mensagemid { get; set; }

    public string Tiporelacionamento { get; set; } = null!;

    public virtual Mensagem Mensagem { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}

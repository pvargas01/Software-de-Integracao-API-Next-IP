using System;
using System.Collections.Generic;

namespace rotav1.Models;

public partial class Usuario
{
    public int Usuarioid { get; set; }

    public string Nome { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Senhahash { get; set; } = null!;

    public string Role { get; set; } = "User";  // "User" ou "Admin"

    public virtual ICollection<Contato> Contatos { get; set; } = new List<Contato>();

    public virtual ICollection<Mensagem> MensagemDestinatarios { get; set; } = new List<Mensagem>();

    public virtual ICollection<Mensagem> MensagemRemetentes { get; set; } = new List<Mensagem>();

    public virtual ICollection<UsuarioMensagem> UsuarioMensagems { get; set; } = new List<UsuarioMensagem>();

    public virtual ICollection<Contato> ContatosNavigation { get; set; } = new List<Contato>();
}

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace rotav1;

public partial class PostgresContext : DbContext
{
    public PostgresContext()
    {
    }

    public PostgresContext(DbContextOptions<PostgresContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Contato> Contatos { get; set; }

    public virtual DbSet<Mensagem> Mensagems { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<UsuarioMensagem> UsuarioMensagems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=ConnectionStrings:PostgreSqlConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum("status_enum", new[] { "Enviado", "Entregue", "Lido" });

        modelBuilder.Entity<Contato>(entity =>
        {
            entity.HasKey(e => e.Contatoid).HasName("contato_pkey");

            entity.ToTable("contato");

            entity.Property(e => e.Contatoid).HasColumnName("contatoid");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");
            entity.Property(e => e.Numerowhatsapp)
                .HasMaxLength(15)
                .HasColumnName("numerowhatsapp");
            entity.Property(e => e.Usuarioid).HasColumnName("usuarioid");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Contatos)
                .HasForeignKey(d => d.Usuarioid)
                .HasConstraintName("contato_usuarioid_fkey");
        });

        modelBuilder.Entity<Mensagem>(entity =>
        {
            entity.HasKey(e => e.Mensagemid).HasName("mensagem_pkey");

            entity.ToTable("mensagem");

            entity.Property(e => e.Mensagemid).HasColumnName("mensagemid");
            entity.Property(e => e.Conteudo).HasColumnName("conteudo");
            entity.Property(e => e.Destinatarioid).HasColumnName("destinatarioid");
            entity.Property(e => e.Remetenteid).HasColumnName("remetenteid");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("timestamp");

            entity.HasOne(d => d.Destinatario).WithMany(p => p.MensagemDestinatarios)
                .HasForeignKey(d => d.Destinatarioid)
                .HasConstraintName("mensagem_destinatarioid_fkey");

            entity.HasOne(d => d.Remetente).WithMany(p => p.MensagemRemetentes)
                .HasForeignKey(d => d.Remetenteid)
                .HasConstraintName("mensagem_remetenteid_fkey");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Usuarioid).HasName("usuario_pkey");

            entity.ToTable("usuario");

            entity.HasIndex(e => e.Email, "usuario_email_key").IsUnique();

            entity.Property(e => e.Usuarioid).HasColumnName("usuarioid");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");
            entity.Property(e => e.Senhahash)
                .HasMaxLength(255)
                .HasColumnName("senhahash");

            entity.HasMany(d => d.ContatosNavigation).WithMany(p => p.Usuarios)
                .UsingEntity<Dictionary<string, object>>(
                    "UsuarioContato",
                    r => r.HasOne<Contato>().WithMany()
                        .HasForeignKey("Contatoid")
                        .HasConstraintName("usuario_contato_contatoid_fkey"),
                    l => l.HasOne<Usuario>().WithMany()
                        .HasForeignKey("Usuarioid")
                        .HasConstraintName("usuario_contato_usuarioid_fkey"),
                    j =>
                    {
                        j.HasKey("Usuarioid", "Contatoid").HasName("usuario_contato_pkey");
                        j.ToTable("usuario_contato");
                        j.IndexerProperty<int>("Usuarioid").HasColumnName("usuarioid");
                        j.IndexerProperty<int>("Contatoid").HasColumnName("contatoid");
                    });
        });

        modelBuilder.Entity<UsuarioMensagem>(entity =>
        {
            entity.HasKey(e => new { e.Usuarioid, e.Mensagemid, e.Tiporelacionamento }).HasName("usuario_mensagem_pkey");

            entity.ToTable("usuario_mensagem");

            entity.Property(e => e.Usuarioid).HasColumnName("usuarioid");
            entity.Property(e => e.Mensagemid).HasColumnName("mensagemid");
            entity.Property(e => e.Tiporelacionamento)
                .HasMaxLength(10)
                .HasColumnName("tiporelacionamento");

            entity.HasOne(d => d.Mensagem).WithMany(p => p.UsuarioMensagems)
                .HasForeignKey(d => d.Mensagemid)
                .HasConstraintName("usuario_mensagem_mensagemid_fkey");

            entity.HasOne(d => d.Usuario).WithMany(p => p.UsuarioMensagems)
                .HasForeignKey(d => d.Usuarioid)
                .HasConstraintName("usuario_mensagem_usuarioid_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

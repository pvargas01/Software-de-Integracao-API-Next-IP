using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace rotav1.Migrations
{
    /// <inheritdoc />
    public partial class andreUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:status_enum", "Enviado,Entregue,Lido");

            migrationBuilder.CreateTable(
                name: "usuario",
                columns: table => new
                {
                    usuarioid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    senhahash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("usuario_pkey", x => x.usuarioid);
                });

            migrationBuilder.CreateTable(
                name: "contato",
                columns: table => new
                {
                    contatoid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    usuarioid = table.Column<int>(type: "integer", nullable: false),
                    nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    numerowhatsapp = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("contato_pkey", x => x.contatoid);
                    table.ForeignKey(
                        name: "contato_usuarioid_fkey",
                        column: x => x.usuarioid,
                        principalTable: "usuario",
                        principalColumn: "usuarioid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mensagem",
                columns: table => new
                {
                    mensagemid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    remetenteid = table.Column<int>(type: "integer", nullable: false),
                    destinatarioid = table.Column<int>(type: "integer", nullable: false),
                    conteudo = table.Column<string>(type: "text", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("mensagem_pkey", x => x.mensagemid);
                    table.ForeignKey(
                        name: "mensagem_destinatarioid_fkey",
                        column: x => x.destinatarioid,
                        principalTable: "usuario",
                        principalColumn: "usuarioid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "mensagem_remetenteid_fkey",
                        column: x => x.remetenteid,
                        principalTable: "usuario",
                        principalColumn: "usuarioid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "usuario_contato",
                columns: table => new
                {
                    usuarioid = table.Column<int>(type: "integer", nullable: false),
                    contatoid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("usuario_contato_pkey", x => new { x.usuarioid, x.contatoid });
                    table.ForeignKey(
                        name: "usuario_contato_contatoid_fkey",
                        column: x => x.contatoid,
                        principalTable: "contato",
                        principalColumn: "contatoid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "usuario_contato_usuarioid_fkey",
                        column: x => x.usuarioid,
                        principalTable: "usuario",
                        principalColumn: "usuarioid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "usuario_mensagem",
                columns: table => new
                {
                    usuarioid = table.Column<int>(type: "integer", nullable: false),
                    mensagemid = table.Column<int>(type: "integer", nullable: false),
                    tiporelacionamento = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("usuario_mensagem_pkey", x => new { x.usuarioid, x.mensagemid, x.tiporelacionamento });
                    table.ForeignKey(
                        name: "usuario_mensagem_mensagemid_fkey",
                        column: x => x.mensagemid,
                        principalTable: "mensagem",
                        principalColumn: "mensagemid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "usuario_mensagem_usuarioid_fkey",
                        column: x => x.usuarioid,
                        principalTable: "usuario",
                        principalColumn: "usuarioid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_contato_usuarioid",
                table: "contato",
                column: "usuarioid");

            migrationBuilder.CreateIndex(
                name: "IX_mensagem_destinatarioid",
                table: "mensagem",
                column: "destinatarioid");

            migrationBuilder.CreateIndex(
                name: "IX_mensagem_remetenteid",
                table: "mensagem",
                column: "remetenteid");

            migrationBuilder.CreateIndex(
                name: "usuario_email_key",
                table: "usuario",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_usuario_contato_contatoid",
                table: "usuario_contato",
                column: "contatoid");

            migrationBuilder.CreateIndex(
                name: "IX_usuario_mensagem_mensagemid",
                table: "usuario_mensagem",
                column: "mensagemid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "usuario_contato");

            migrationBuilder.DropTable(
                name: "usuario_mensagem");

            migrationBuilder.DropTable(
                name: "contato");

            migrationBuilder.DropTable(
                name: "mensagem");

            migrationBuilder.DropTable(
                name: "usuario");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:Enum:status_enum", "Enviado,Entregue,Lido");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesafioVWFS.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_contratos",
                columns: table => new
                {
                    id_contrato = table.Column<Guid>(type: "uuid", nullable: false),
                    nr_cpf_cnpj_cliente = table.Column<string>(type: "character varying(18)", maxLength: 18, nullable: false),
                    vl_total = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    pc_taxa_mensal = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    qt_prazo_meses = table.Column<int>(type: "integer", nullable: false),
                    dt_vencimento_primeira_parcela = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    tp_veiculo = table.Column<int>(type: "integer", nullable: false),
                    tp_condicao_veiculo = table.Column<int>(type: "integer", nullable: false),
                    dh_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    dh_atualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    fl_ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_contratos", x => x.id_contrato);
                });

            migrationBuilder.CreateTable(
                name: "tb_pagamentos",
                columns: table => new
                {
                    id_pagamento = table.Column<Guid>(type: "uuid", nullable: false),
                    id_contrato = table.Column<Guid>(type: "uuid", nullable: false),
                    vl_parcela = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    vl_juros = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    vl_amortizacao = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    vl_saldo_devedor = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    dt_vencimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    dt_pagamento = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    dh_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    st_pagamento = table.Column<int>(type: "integer", nullable: false),
                    ds_observacao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_pagamentos", x => x.id_pagamento);
                    table.ForeignKey(
                        name: "FK_tb_pagamentos_tb_contratos_id_contrato",
                        column: x => x.id_contrato,
                        principalTable: "tb_contratos",
                        principalColumn: "id_contrato",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_tb_contratos_fl_ativo",
                table: "tb_contratos",
                column: "fl_ativo");

            migrationBuilder.CreateIndex(
                name: "ix_tb_contratos_nr_cpf_cnpj_cliente",
                table: "tb_contratos",
                column: "nr_cpf_cnpj_cliente");

            migrationBuilder.CreateIndex(
                name: "ix_tb_pagamentos_dt_vencimento",
                table: "tb_pagamentos",
                column: "dt_vencimento");

            migrationBuilder.CreateIndex(
                name: "ix_tb_pagamentos_id_contrato",
                table: "tb_pagamentos",
                column: "id_contrato");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_pagamentos");

            migrationBuilder.DropTable(
                name: "tb_contratos");
        }
    }
}

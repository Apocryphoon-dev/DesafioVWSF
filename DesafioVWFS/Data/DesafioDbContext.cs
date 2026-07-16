using DesafioVWFS.Application.Shared.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DesafioVWFS.Data
{
    public class DesafioDbContext : DbContext
    {
        public DesafioDbContext(DbContextOptions<DesafioDbContext> options) : base(options)
        {
        }

        public DbSet<Contrato> Contratos { get; set; } = null!;
        public DbSet<Pagamento> Pagamentos { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Contrato>(entity =>
            {
                entity.ToTable("tb_contratos");
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .HasColumnName("id_contrato");

                entity.Property(x => x.ClienteCpfCnpj)
                    .HasColumnName("nr_cpf_cnpj_cliente")
                    .IsRequired()
                    .HasMaxLength(18);

                entity.Property(x => x.ValorTotal)
                    .HasColumnName("vl_total")
                    .HasPrecision(18, 2)
                    .IsRequired();

                entity.Property(x => x.TaxaMensal)
                    .HasColumnName("pc_taxa_mensal")
                    .HasPrecision(5, 2)
                    .IsRequired();

                entity.Property(x => x.PrazoMeses)
                    .HasColumnName("qt_prazo_meses");

                entity.Property(x => x.DataVencimentoPrimeiraParcela)
                    .HasColumnName("dt_vencimento_primeira_parcela");

                entity.Property(x => x.TipoVeiculo)
                    .HasColumnName("tp_veiculo");

                entity.Property(x => x.CondicaoVeiculo)
                    .HasColumnName("tp_condicao_veiculo");

                entity.Property(x => x.DataCriacao)
                    .HasColumnName("dh_criacao")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(x => x.DataAtualizacao)
                    .HasColumnName("dh_atualizacao");

                entity.Property(x => x.Ativo)
                    .HasColumnName("fl_ativo");

                entity.HasIndex(x => x.ClienteCpfCnpj)
                    .HasDatabaseName("ix_tb_contratos_nr_cpf_cnpj_cliente");

                entity.HasIndex(x => x.Ativo)
                    .HasDatabaseName("ix_tb_contratos_fl_ativo");

                entity.HasMany(x => x.Pagamentos)
                    .WithOne(p => p.Contrato)
                    .HasForeignKey(p => p.ContratoId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Pagamento>(entity =>
            {
                entity.ToTable("tb_pagamentos");
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .HasColumnName("id_pagamento");

                entity.Property(x => x.ContratoId)
                    .HasColumnName("id_contrato");

                entity.Property(x => x.ValorParcela)
                    .HasColumnName("vl_parcela")
                    .HasPrecision(18, 2)
                    .IsRequired();

                entity.Property(x => x.Juros)
                    .HasColumnName("vl_juros")
                    .HasPrecision(18, 2)
                    .IsRequired();

                entity.Property(x => x.Amortizacao)
                    .HasColumnName("vl_amortizacao")
                    .HasPrecision(18, 2)
                    .IsRequired();

                entity.Property(x => x.SaldoDevedor)
                    .HasColumnName("vl_saldo_devedor")
                    .HasPrecision(18, 2)
                    .IsRequired();

                entity.Property(x => x.DataVencimento)
                    .HasColumnName("dt_vencimento");

                entity.Property(x => x.DataPagamento)
                    .HasColumnName("dt_pagamento");

                entity.Property(x => x.DataCriacao)
                    .HasColumnName("dh_criacao")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(x => x.StatusPagamento)
                    .HasColumnName("st_pagamento");

                entity.Property(x => x.Observacoes)
                    .HasColumnName("ds_observacao")
                    .HasMaxLength(500);

                entity.HasIndex(x => x.ContratoId)
                    .HasDatabaseName("ix_tb_pagamentos_id_contrato");

                entity.HasIndex(x => x.DataVencimento)
                    .HasDatabaseName("ix_tb_pagamentos_dt_vencimento");
            });
        }
    }
}

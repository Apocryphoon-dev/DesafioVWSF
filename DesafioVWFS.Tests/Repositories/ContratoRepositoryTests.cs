using DesafioVWFS.Application.Shared.Repository;

namespace DesafioVWFS.Tests.Repositories;

public class ContratoRepositoryTests
{
    [Fact]
    public async Task ObterPorClienteAsync_DeveRetornarSomenteContratosAtivosDoCliente()
    {
        await using var context = DefaultFixture.CreateInMemoryContext();

        var contratoAtivo = DefaultFixture.CreateContrato("12345678909", ativo: true);
        var contratoInativo = DefaultFixture.CreateContrato("12345678909", ativo: false);
        var contratoOutroCliente = DefaultFixture.CreateContrato("98765432100", ativo: true);

        context.Contratos.AddRange(contratoAtivo, contratoInativo, contratoOutroCliente);
        await context.SaveChangesAsync();

        var repository = new ContratoRepository(context);

        var result = await repository.ObterPorClienteAsync("12345678909");

        Assert.Single(result);
        Assert.Equal(contratoAtivo.Id, result.Single().Id);
    }

    [Fact]
    public async Task ObterPorClienteAsync_DeveConsiderarCpfComOuSemMascara()
    {
        await using var context = DefaultFixture.CreateInMemoryContext();

        var contratoComMascara = DefaultFixture.CreateContrato("529.982.247-25", ativo: true);
        var contratoOutroCliente = DefaultFixture.CreateContrato("111.444.777-35", ativo: true);

        context.Contratos.AddRange(contratoComMascara, contratoOutroCliente);
        await context.SaveChangesAsync();

        var repository = new ContratoRepository(context);

        var result = await repository.ObterPorClienteAsync("52998224725");

        Assert.Single(result);
        Assert.Equal(contratoComMascara.Id, result.Single().Id);
    }

    [Fact]
    public async Task DeletarAsync_DeveMarcarContratoComoInativo()
    {
        await using var context = DefaultFixture.CreateInMemoryContext();

        var contrato = DefaultFixture.CreateContrato("12345678909", ativo: true);
        context.Contratos.Add(contrato);
        await context.SaveChangesAsync();

        var repository = new ContratoRepository(context);

        var deleted = await repository.DeletarAsync(contrato.Id);
        var updated = await repository.ObterPorIdAsync(contrato.Id);

        Assert.True(deleted);
        Assert.NotNull(updated);
        Assert.False(updated!.Ativo);
    }
}

namespace DesafioVWFS.Application.Validators;

public static class CpfCnpjValidator
{
    public static bool ValidarCpfCnpj(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        var digits = new string(value.Where(char.IsDigit).ToArray());
        if (digits.Length == 11)
            return ValidarCpf(digits);

        if (digits.Length == 14)
            return ValidarCnpj(digits);

        return false;
    }

    private static bool ValidarCpf(string cpf)
    {
        if (cpf.Length != 11 || cpf.All(c => c == cpf[0]))
            return false;

        var digito1 = CalcularDigito(cpf[..9], 10);
        var digito2 = CalcularDigito(cpf[..9] + digito1, 11);
        return cpf.EndsWith(digito1.ToString() + digito2.ToString());
    }

    private static bool ValidarCnpj(string cnpj)
    {
        if (cnpj.Length != 14 || cnpj.All(c => c == cnpj[0]))
            return false;

        var digito1 = CalcularDigito(cnpj[..12], 5);
        var digito2 = CalcularDigito(cnpj[..12] + digito1, 6);
        return cnpj.EndsWith(digito1.ToString() + digito2.ToString());
    }

    private static int CalcularDigito(string baseDigits, int pesoInicial)
    {
        var soma = 0;
        var peso = pesoInicial;

        foreach (var digit in baseDigits)
        {
            soma += (digit - '0') * peso;
            peso = peso == 2 ? 9 : peso - 1;
        }

        var resto = soma % 11;
        var digito = resto < 2 ? 0 : 11 - resto;
        return digito;
    }
}

using FluentValidation.Results;

namespace DesafioVWFS.Application.Shared.Core;

/// <summary>
/// Representa o resultado de um use case com um valor tipado.
/// </summary>
/// <typeparam name="T">Tipo do resultado.</typeparam>
public class Output<T> where T : notnull
{
    private List<string>? _messages;
    private List<string>? _errorMessages;
    protected T? _result;

    /// <summary>
    /// Cria uma instância de Output com estado inicial de validação.
    /// </summary>
    /// <param name="isValid">Indica se o resultado é válido.</param>
    public Output(bool isValid = true)
    {
        IsValid = isValid;
    }

    /// <summary>
    /// Cria uma instância de Output com um resultado já definido.
    /// </summary>
    /// <param name="result">Resultado do caso de uso.</param>
    public Output(T result)
    {
        IsValid = true;
        AddResult(result);
    }

    /// <summary>
    /// Cria uma instância de Output a partir de um ValidationResult.
    /// </summary>
    /// <param name="validationResult">Resultado de validação.</param>
    public Output(ValidationResult validationResult)
    {
        ProcessValidationResults(validationResult);
    }

    /// <summary>
    /// Lista das mensagens de erro retornadas pelo fluxo.
    /// </summary>
    public IReadOnlyCollection<string> ErrorMessages => GetMessages(_errorMessages);

    /// <summary>
    /// Indica se o processo finalizou com sucesso.
    /// </summary>
    public bool IsValid { get; protected set; }

    /// <summary>
    /// Lista de mensagens informativas do fluxo.
    /// </summary>
    public IReadOnlyCollection<string> Messages => GetMessages(_messages);

    private static IReadOnlyCollection<string> GetMessages(List<string>? messages)
        => messages is null ? Array.Empty<string>() : messages.AsReadOnly();

    /// <summary>
    /// Adiciona uma mensagem de erro ao resultado.
    /// </summary>
    /// <param name="message">Mensagem de erro.</param>
    public void AddErrorMessage(string message) => AddErrorMessages(message);

    /// <summary>
    /// Adiciona uma coleção de mensagens de erro ao resultado.
    /// </summary>
    /// <param name="messages">Mensagens de erro.</param>
    public void AddErrorMessages(params string[] messages)
    {
        _errorMessages ??= new List<string>();
        foreach (var message in messages)
        {
            if (string.IsNullOrWhiteSpace(message)) continue;
            _errorMessages.Add(message);
        }

        IsValid = _errorMessages.Count == 0;
    }

    /// <summary>
    /// Adiciona uma mensagem informativa ao resultado.
    /// </summary>
    /// <param name="message">Mensagem informativa.</param>
    public void AddMessage(string message) => AddMessages(message);

    /// <summary>
    /// Adiciona mensagens informativas ao resultado.
    /// </summary>
    /// <param name="messages">Mensagens informativas.</param>
    public void AddMessages(params string[] messages)
    {
        _messages ??= new List<string>();
        foreach (var message in messages)
        {
            if (string.IsNullOrWhiteSpace(message)) continue;
            _messages.Add(message);
        }
    }

    /// <summary>
    /// Define o resultado principal do use case.
    /// </summary>
    /// <param name="result">Resultado a ser armazenado.</param>
    public void AddResult(T result)
    {
        _result = result ?? throw new InvalidOperationException("Result cannot be null");
        IsValid = true;
    }

    /// <summary>
    /// Retorna o resultado principal armazenado.
    /// </summary>
    /// <returns>Resultado do use case.</returns>
    public T GetResult() => _result ?? throw new InvalidOperationException("No result available");

    /// <summary>
    /// Processa um resultado de validação do FluentValidation.
    /// </summary>
    /// <param name="validationResults">Resultados de validação.</param>
    public void ProcessValidationResults(params ValidationResult[] validationResults)
    {
        foreach (var validationResult in validationResults)
        {
            if (validationResult is null) continue;
            AddErrorMessages(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        IsValid = ErrorMessages.Count == 0;
    }
}

/// <summary>
/// Representa o resultado de um use case sem valor de retorno.
/// </summary>
public class Output : Output<object>
{
    /// <summary>
    /// Cria uma instância de Output sem resultado.
    /// </summary>
    public Output() : base() { }

    /// <summary>
    /// Cria uma instância de Output com estado inicial de sucesso ou falha.
    /// </summary>
    /// <param name="isValid">Indica se o resultado é válido.</param>
    public Output(bool isValid = true) : base(isValid) { }

    /// <summary>
    /// Cria uma instância de Output com um resultado genérico.
    /// </summary>
    /// <param name="result">Resultado a ser armazenado.</param>
    public Output(object result) : base(result) { }

    /// <summary>
    /// Define o resultado principal para o caso sem tipo específico.
    /// </summary>
    /// <param name="result">Resultado a ser armazenado.</param>
    public new void AddResult(object result) => _result = result ?? throw new InvalidOperationException("Result cannot be null");
}

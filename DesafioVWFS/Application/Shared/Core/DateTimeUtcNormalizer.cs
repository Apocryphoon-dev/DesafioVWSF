namespace DesafioVWFS.Application.Shared.Core;

public static class DateTimeUtcNormalizer
{
    public static DateTime Normalize(DateTime value)
    {
        return value.Kind switch
        {
            DateTimeKind.Utc => value,
            DateTimeKind.Local => value.ToUniversalTime(),
            _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
        };
    }

    public static DateTime? Normalize(DateTime? value)
    {
        if (!value.HasValue)
        {
            return null;
        }

        return Normalize(value.Value);
    }
}
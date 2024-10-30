using Balta.Domain.SharedContext.Abstractions;

namespace Balta.Domain.AccountContext.Providers;

public class DateTimeProvider : IDateTimeProvider
{
    public static IDateTimeProvider Default { get; } = new DateTimeProvider();
    public DateTime UtcNow => DateTime.UtcNow;
}
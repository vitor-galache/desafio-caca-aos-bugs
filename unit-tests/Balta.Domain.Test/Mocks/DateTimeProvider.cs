using Balta.Domain.SharedContext.Abstractions;

namespace Balta.Domain.Test.Mocks;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
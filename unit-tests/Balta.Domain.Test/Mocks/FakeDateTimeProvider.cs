using Balta.Domain.SharedContext.Abstractions;

namespace Balta.Domain.Test.Mocks;

public class FakeDateTimeProvider : IDateTimeProvider
{
    public static IDateTimeProvider Default { get;} = new FakeDateTimeProvider();
    private DateTime? _staticDateTime;
    public DateTime UtcNow  => _staticDateTime ?? DateTime.UtcNow;

    public FakeDateTimeProvider(DateTime? staticDate)
    {
        _staticDateTime = staticDate;        
    }

    public FakeDateTimeProvider() : this(null)
    {
        
    }

    public void ChangeDate(DateTime date)
    {
        _staticDateTime = date;
    }
}
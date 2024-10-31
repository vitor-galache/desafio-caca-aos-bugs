using Balta.Domain.AccountContext.ValueObjects;
using Balta.Domain.AccountContext.ValueObjects.Exceptions;
using Balta.Domain.Test.Mocks;

namespace Balta.Domain.Test.AccountContext.ValueObjects;

public class VerificationCodeTest
{
    [Fact]
    public void ShouldGenerateVerificationCode()
    {
        var verificationCode = VerificationCode.ShouldCreate(new FakeDateTimeProvider());

        Assert.NotNull(verificationCode);
    }

    [Fact]
    public void ShouldGenerateExpiresAtInFuture()
    {
        var verificationCode = VerificationCode.ShouldCreate(new FakeDateTimeProvider());

        Assert.True(verificationCode.ExpiresAtUtc > DateTime.UtcNow);
    }

    [Fact]
    public void ShouldGenerateVerifiedAtAsNull()
    {
        var verificationCode = VerificationCode.ShouldCreate(new FakeDateTimeProvider());

        Assert.Null(verificationCode.VerifiedAtUtc);
    }


    [Fact]
    public void ShouldFailIfExpired()
    {
        var provider = new FakeDateTimeProvider();

        var verificationCode = VerificationCode.ShouldCreate(provider);

        Assert.False(verificationCode.IsExpired);
    }

    [Fact]
    public void ShouldFailIfCodeIsInvalid()
    {
        var provider = new FakeDateTimeProvider();
        var verificationCode = VerificationCode.ShouldCreate(provider);

        provider.ChangeDate(DateTime.UtcNow.AddMinutes(5));
        
        var act = () => verificationCode.ShouldVerify(verificationCode);

        Assert.Throws<InvalidVerificationCodeException>(act);
    }

    [Fact]
    public void ShouldFailIfCodeIsLessThanSixChars()
    {
        var verificationCode = VerificationCode.ShouldCreate(new FakeDateTimeProvider());

        Assert.False(verificationCode.Code.Length < 6);
    }

    [Fact]
    public void ShouldFailIfCodeIsGreaterThanSixChars()
    {
        var verificationCode = VerificationCode.ShouldCreate(new FakeDateTimeProvider());

        Assert.False(verificationCode.Code.Length > 6);
    }

    [Fact]
    public void ShouldFailIfIsNotActive()
    {
        var provider = new FakeDateTimeProvider();
        var verificationCode = VerificationCode.ShouldCreate(provider);
        
        provider.ChangeDate(DateTime.UtcNow.AddMinutes(5));
        
        var act  = ()=> verificationCode.ShouldVerify(verificationCode);
        
        Assert.Throws<InvalidVerificationCodeException>(act);
        Assert.False(verificationCode.IsActive);
    }

    [Fact]
    public void ShouldFailIfIsAlreadyVerified()
    {
        var provider = new FakeDateTimeProvider();
        var verificationCode = VerificationCode.ShouldCreate(provider);
        
        // First Verification
        verificationCode.ShouldVerify(verificationCode);

        
        // Checked again
        var act = ()=> verificationCode.ShouldVerify(verificationCode);

        Assert.Throws<InvalidVerificationCodeException>(act);
    }


    [Fact]
    public void ShouldFailIfIsVerificationCodeDoesNotMatch()
    {
        var provider = new FakeDateTimeProvider();

        var result = VerificationCode.ShouldCreate(provider);

        Assert.Matches(@"[a-zA-Z0-9]{6}", result.Code);
    }

    [Fact]
    public void ShouldVerify()
    {
      var provider = new FakeDateTimeProvider();
      var verificationCode = VerificationCode.ShouldCreate(provider);
      
      verificationCode.ShouldVerify(verificationCode);
      
      Assert.True(verificationCode.VerifiedAtUtc < DateTime.UtcNow);
      Assert.False(verificationCode.IsActive);
    } 
}
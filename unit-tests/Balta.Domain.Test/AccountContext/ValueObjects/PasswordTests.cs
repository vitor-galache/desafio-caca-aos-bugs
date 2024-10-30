using Balta.Domain.AccountContext.ValueObjects;
using Balta.Domain.AccountContext.ValueObjects.Exceptions;
using Balta.Domain.Test.Mocks;

namespace Balta.Domain.Test.AccountContext.ValueObjects;

[Trait("ValueObject", "Password")]
public class PasswordTests
{
    private const string ValidPassword = "Abcd@123";
    private const string InvalidPassword = "1234567";
    private const string EmptyPassword = "";
    private const string PasswordIsWhiteSpace = " ";
    [Fact]
    public void ShouldFailIfPasswordIsNull()
    {
        var act = () => Password.ShouldCreate(null);
        Assert.Throws<InvalidPasswordException>(act);
    }

    [Fact]
    public void ShouldFailIfPasswordIsEmpty()
    {
        var act = () => Password.ShouldCreate(EmptyPassword);

        Assert.Throws<InvalidPasswordException>(act);
    }

    [Fact]
    public void ShouldFailIfPasswordIsWhiteSpace()
    {
        var act = () => Password.ShouldCreate(PasswordIsWhiteSpace);

        Assert.Throws<InvalidPasswordException>(act);
    }

    [Fact]
    public void ShouldFailIfPasswordLenIsLessThanMinimumChars()
    {
        var act = () => Password.ShouldCreate(InvalidPassword);
        Assert.Throws<InvalidPasswordException>(act);
    }

    [Fact]
    public void ShouldFailIfPasswordLenIsGreaterThanMaxChars()
    {
        var act = () => Password.ShouldCreate(ValidPassword.PadRight(49, '0'));

        Assert.Throws<InvalidPasswordException>(act);
    }

    [Fact]
    public void ShouldHashPassword()
    {
        var password = Password.ShouldCreate(ValidPassword);

        Assert.NotEmpty(password.Hash);
    }

    [Fact]
    public void ShouldVerifyPasswordHash()
    {
        var result = Password.ShouldCreate(ValidPassword);

        var match = Password.ShouldMatch(result.Hash,ValidPassword);

        Assert.True(match);
    }

    [Fact]
    public void ShouldGenerateStrongPassword()
    {
        bool IsSpecialCharacter(char c)
        {
            return !char.IsLetterOrDigit(c) && !char.IsWhiteSpace(c);
        }

        var strongPassword = Password.ShouldGenerate();

        Assert.True(strongPassword.Any(char.IsNumber) && strongPassword.Any(char.IsLetter) &&
                    strongPassword.Any(IsSpecialCharacter));
    }

    [Fact]
    public void ShouldImplicitConvertToString()
    {
        var password = Password.ShouldCreate(ValidPassword);

        Assert.Equal((string)password, password.ToString());
    }

    [Fact]
    public void ShouldReturnHashAsStringWhenCallToStringMethod()
    {
        var password = Password.ShouldCreate(ValidPassword);

        Assert.Equal(password.Hash, password.ToString());
    }

    [Fact]
    public void ShouldMarkPasswordAsExpired()
    {
        var provider = new FakeDateTimeProvider();
        var password = Password.ShouldCreate(ValidPassword,provider);
        
        provider.ChangeDate(DateTime.UtcNow.AddMinutes(120.1));
        
        Assert.True(password.IsExpired());
    }

    [Fact]
    public void ShouldFailIfPasswordIsExpired()
    {
        var password = Password.ShouldCreate(ValidPassword);
        
        Assert.False(password.IsExpired());
    }

    [Fact]
    public void ShouldMarkPasswordAsMustChange()
    {
        var provider = new FakeDateTimeProvider();
        var password = Password.ShouldCreate(ValidPassword,provider);
        
        provider.ChangeDate(DateTime.UtcNow.AddMinutes(120.1));
        
        Assert.True(password.MustChange);
    }

    [Fact]
    public void ShouldFailIfPasswordIsMarkedAsMustChange()
    {
        var password = Password.ShouldCreate(ValidPassword);
        
        Assert.False(password.MustChange);
    } 
}
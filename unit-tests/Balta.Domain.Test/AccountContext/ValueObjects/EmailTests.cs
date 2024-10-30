using Balta.Domain.AccountContext.ValueObjects;
using Balta.Domain.AccountContext.ValueObjects.Exceptions;
using Balta.Domain.Test.Mocks;

namespace Balta.Domain.Test.AccountContext.ValueObjects;

public class EmailTests
{
    [Theory]
    [InlineData("FaKeEmail@teste.io","fakeemail@teste.io")]
    [InlineData("EMAIL@GMAIL.COM","email@gmail.com")]
    [InlineData("email@OUtlook.com","email@outlook.com")]
    public void ShouldLowerCaseEmail(string emailAddress,string expectedEmailAddress)
    {
        var email = Email.ShouldCreate(emailAddress,FakeDateTimeProvider.Default);
        
        Assert.Equal(expectedEmailAddress, email);
    }

    [Theory]
    [InlineData("teste@balta.io  ","teste@balta.io")]
    [InlineData("  email@gmail.com","email@gmail.com")]
    [InlineData("  email@outlook.com   ","email@outlook.com")]
    public void ShouldTrimEmail(string emailAddress,string expectedEmailAddress)
    {
        var email = Email.ShouldCreate(emailAddress,FakeDateTimeProvider.Default);

        Assert.Equal(expectedEmailAddress, email.Address);
    }

    [Fact]
    public void ShouldFailIfEmailIsNull()
    {
        var act = () => Email.ShouldCreate(null,FakeDateTimeProvider.Default);
        
        Assert.Throws<NullEmailException>(act);
    }

    [Theory]
    [InlineData("")]
    public void ShouldFailIfEmailIsEmpty(string emptyString)
    {
        var act = () => Email.ShouldCreate(emptyString,FakeDateTimeProvider.Default);

        Assert.Throws<InvalidEmailException>(act);
    }

    [Theory]
    [InlineData("invalid.email")]
    [InlineData("invalid.email@gmail.com@gmail.com")]
    [InlineData("emailteste")]
    public void ShouldFailIfEmailIsInvalid(string invalidEmail)
    {
        var act = () => Email.ShouldCreate(invalidEmail,FakeDateTimeProvider.Default);

        Assert.Throws<InvalidEmailException>(act);
    }

    [Theory]
    [InlineData("validemail@gmail.com")]
    [InlineData("teste@gmail.com")]
    [InlineData("teste@balta.io")]
    public void ShouldPassIfEmailIsValid(string validEmail)
    {
        var email = Email.ShouldCreate(validEmail,FakeDateTimeProvider.Default);
        
        Assert.Equal(validEmail,email.Address);
    }

    [Fact]
    public void ShouldHashEmailAddress()
    {
        var email = Email.ShouldCreate("teste@gmail.com",FakeDateTimeProvider.Default);
        
        Assert.NotEmpty(email.Hash);
    }

    [Theory]
    [InlineData("teste@gmail.com")]
    public void ShouldExplicitConvertFromString(string validEmail)
    {   
        var email = (Email)validEmail;
        
        Assert.Equal(email.Address, validEmail);
        Assert.NotEmpty(email.Hash);
    }

    [Theory]
    [InlineData("teste@gmail.com")]
    [InlineData("meuemail@gmail.com")]
    [InlineData("teste@balta.io")]
    public void ShouldExplicitConvertToString(string validEmail)
    {
        var email = Email.ShouldCreate(validEmail,FakeDateTimeProvider.Default);
        
        Assert.Equal((string)email,validEmail);
    }

    [Theory]
    [InlineData("teste@balta.io")]
    public void ShouldReturnEmailWhenCallToStringMethod(string validEmail)
    {
        var email = Email.ShouldCreate(validEmail,FakeDateTimeProvider.Default);
        
        Assert.Equal(email.ToString(), validEmail);
    } 
}
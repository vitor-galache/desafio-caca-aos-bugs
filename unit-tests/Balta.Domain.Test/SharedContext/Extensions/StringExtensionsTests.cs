using Balta.Domain.SharedContext.Extensions;

namespace Balta.Domain.Test.SharedContext.Extensions;

public class StringExtensionsTests
{
    [Theory]
    [InlineData("TextoTeste","VGV4dG9UZXN0ZQ==")]
    [InlineData("","")]
    public void ShouldGenerateBase64FromString(string text,string textInbase64)
    {
      var expectedBase64 = text.ToBase64();
      
      Assert.Equal(textInbase64, expectedBase64);
    } 
}
using BaseConverter.Core;

namespace BaseConverter.Tests
{
    public class Ipv6FormatterTests
    {
        [Theory]
        [InlineData("2001:db8::1", "2001:0db8:0000:0000:0000:0000:0000:0001", "2001:db8:0:0:0:0:0:1", "2001:db8::1")]
        [InlineData("::1", "0000:0000:0000:0000:0000:0000:0000:0001", "0:0:0:0:0:0:0:1", "::1")]
        [InlineData("2001:db8::", "2001:0db8:0000:0000:0000:0000:0000:0000", "2001:db8:0:0:0:0:0:0", "2001:db8::")]
        [InlineData("::", "0000:0000:0000:0000:0000:0000:0000:0000", "0:0:0:0:0:0:0:0", "::")]
        [InlineData("2001:0:0:1:0:0:0:1", "2001:0000:0000:0001:0000:0000:0000:0001", "2001:0:0:1:0:0:0:1", "2001:0:0:1::1")]
        public void Format_ValidInput_ReturnsCorrectForms(string input, string expectedFull, string expectedTrimmed, string expectedShort)
        {
            // Act
            Ipv6Result result = Ipv6Formatter.Format(input);

            // Assert
            Assert.Equal(expectedFull, result.FullAddress);
            Assert.Equal(expectedTrimmed, result.TrimmedAddress);
            Assert.Equal(expectedShort, result.ShortAddress);
        }
    }
}
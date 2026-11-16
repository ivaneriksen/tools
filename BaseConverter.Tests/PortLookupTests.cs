using BaseConverter.Core;

namespace BaseConverter.Tests
{
    public class PortLookupTests
    {

        [Theory]
        [InlineData("ssh", new[] { 22 })]
        [InlineData("email", new[] { 25, 110, 143 })]
        [InlineData("33", new[] { 3306, 3389 })]
        public void Search_FindsPorts_IgnoringCase(string query, int[] expectedPortNumbers)
        {
            // Act
            List<Port> foundPorts = PortLookup.Search(query);
            int[] foundPortNumbers = foundPorts.Select(p => p.Number).ToArray();
            // Assert
            Assert.Equal(expectedPortNumbers, foundPortNumbers);
        }

        [Theory]
        [InlineData(0, "Well-known")]
        [InlineData(1023, "Well-known")]
        [InlineData(1024, "Registered")]
        [InlineData(49151, "Registered")]
        [InlineData(49152, "Dynamic")]
        [InlineData(65535, "Dynamic")]
        public void GetRange_ReturnsRangeName_AtBoundaries(int portNumber, string expectedString)
        {
            // Act & Assert
            Assert.Equal(expectedString, PortLookup.GetRange(portNumber));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(65536)]
        public void GetRange_ThrowsArgumentOutOfRangeException_WhenOutsideValidRange(int portNumber)
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => PortLookup.GetRange(portNumber));
        }
    }
}

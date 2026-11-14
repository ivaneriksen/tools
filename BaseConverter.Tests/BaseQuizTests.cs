
using BaseConverter.Core;

namespace BaseConverter.Tests
{
    public class BaseQuizTests
    {
        [Fact]
        public void Generate_ThrowsArgumentException_WhenLessThanTwoBasesProvided()
        {
            // Arrange
            int[] bases = { 2 };
            int maxValue = 10;
            Random random = new Random();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => BaseQuiz.Generate(bases, maxValue, random));
        }

        [Fact]
        public void Generate_DoesNotReturnSameBaseForFromAndTo()
        {
            // Arrange
            int[] bases = { 2, 16 };
            int maxValue = 255;
            Random random = new Random(42);
            for (int i = 0; i < 100; i++) // Run multiple times to ensure randomness
            {
                // Act
                var question = BaseQuiz.Generate(bases, maxValue, random);
                // Assert
                Assert.NotEqual(question.FromBase, question.ToBase);
            }
            
        }

        [Theory]
        [InlineData("B7", "00b7")]
        [InlineData("7", "07")]
        [InlineData("10001", "010001")]
        public void IsCorrect_ReturnsTrue_IgnoringCaseSpacesAndLeadingZeros(string correctAnswer, string userAnswer)
        {
            // Arrange
            QuizQuestion question = new QuizQuestion("183", 16, 10, correctAnswer);
            // Act & Assert
            Assert.True(BaseQuiz.IsCorrect(question, userAnswer));
        }

        [Theory]
        [InlineData("B7", "B8")]
        [InlineData("7", "8")]
        [InlineData("10001", "10002")]
        public void IsCorrect_ReturnsFalse_ForIncorrectAnswer(string correctAnswer, string userAnswer)
        {
            // Arrange
            QuizQuestion question = new QuizQuestion("183", 16, 10, correctAnswer);
            // Act & Assert
            Assert.False(BaseQuiz.IsCorrect(question, userAnswer));
        }
    }
}


namespace BaseConverter.Core
{
    public readonly record struct QuizQuestion(string Number, int FromBase, int ToBase, string Answer);
    public static class BaseQuiz
    {
        public static QuizQuestion Generate(int[] bases, int maxValue, Random random) 
        {
            if (bases.Distinct().Count() < 2)
            {
                throw new ArgumentException("At least two bases are required to generate a quiz question.");
            }

            int fromBase = bases[random.Next(bases.Length)];
            int toBase = bases[random.Next(bases.Length)];
            while (toBase == fromBase)
            {
                toBase = bases[random.Next(bases.Length)];
            }

            int value = random.Next(1, maxValue + 1);

            string number = Converter.Convert(value.ToString(), 10, fromBase);
            string answer = Converter.Convert(value.ToString(), 10, toBase);

            return new QuizQuestion(number, fromBase, toBase, answer);
        }

        public static bool IsCorrect(QuizQuestion question, string userAnswer)
        {
            userAnswer = userAnswer.Replace(" ", "").TrimStart('0'); // Remove leading zeros for comparison

            return string.Equals(question.Answer, userAnswer, StringComparison.OrdinalIgnoreCase);
        }

        // Groups binary numbers for readability
        public static string GroupDigits(string number)
        {
            if (number.Length < 5) return number;

            string numberGrouped = "";
            for (int i = number.Length-1; i >= 0; i--)
            {
                if ((number.Length - i) % 4 == 0 && i != 0)
                {
                    numberGrouped = " " + number[i] + numberGrouped;
                }
                else
                {
                    numberGrouped = number[i] + numberGrouped;
                }
            }
            return numberGrouped;
        }
    }
}

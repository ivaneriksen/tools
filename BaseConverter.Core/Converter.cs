namespace BaseConverter.Core
{
    // Convert to and from any base between 2 and 36 using Horner's method.
    public static class Converter
    {
        private const string Digits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        // Convert a string representation of a number in the specified base to a string representation in another base.
        public static string Convert(string digits, int fromBase, int toBase)
        {
            
            // If fromBase is less than 2 or greater than the length of Digits, throw an exception.
            if (fromBase < 2 || fromBase > Digits.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(fromBase), "Base must be between 2 and {Digits.Length}.");
            }
            // If toBase is less than 2 or greater than the length of Digits, throw an exception.
            if (toBase < 2 || toBase > Digits.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(toBase), "Base must be between 2 and {Digits.Length}.");
            }
            // If digits is null or empty, throw an exception.
            if (string.IsNullOrWhiteSpace(digits))
            {
                throw new ArgumentException("Digits cannot be null or empty.", nameof(digits));
            }

            long value = ParseFromBase(digits, fromBase);
            return ConvertToBase(value, toBase);
        }
        // Convert a long value to a string representation in the specified base.
        private static string ConvertToBase(long value, int toBase)
        {
            if (value == 0) return "0";
            string result = "";
            while (value > 0)
            {
                result = Digits[(int)(value % toBase)] + result;
                value /= toBase;
            }
            return result;
        }
        // Parse a string representation of a number in the specified base to a long value.
        private static long ParseFromBase(string digits, int fromBase)
        {
            digits = digits.Trim().ToUpper();
            // If the digits string starts with a negative sign, throw an exception.
            if (digits.StartsWith('-'))
            {
                throw new ArgumentException("Negative numbers are not supported.");
            }

            long value = 0;
            foreach (char c in digits)
            {
                int digitValue = Digits.IndexOf(c);

                if (digitValue < 0 || digitValue >= fromBase)
                {
                    throw new ArgumentException($"'{c}' is not a valid digit in base {fromBase}.");
                }

                value = checked(value * fromBase + digitValue);
            }
            return value;
        }

        // Groups binary numbers for readability
        public static string GroupDigits(string number)
        {
            if (number.Length < 5) return number;

            string numberGrouped = "";
            for (int i = number.Length - 1; i >= 0; i--)
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
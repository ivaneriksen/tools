namespace BaseConverter.Core
{
    class Program
    {
        static void Main(string[] args)
        {


            string[] tests =
            {
                "2001:db8::1",
                "::1",
                "2001:db8::",
                "::",
                "2001:0db8:0000:0000:0000:0000:0000:0001",
                "2001:db8::1::5",
                "2001:xyz::1",
                "1:2:3",

                // Some tricky cases
                "2001:::1",           // three colons
                "1::2:",              // single colon at the end
                "1:2:3:4:5:6:7:",     // trailing colon, no ::
                "1:2:3:4::5:6:7:8",   // :: plus 8 groups
                "12345::1",           // group too long
            };

            foreach (string test in tests)
            {
                try
                {
                    var result = Ipv6Formatter.Format(test);
                    Console.WriteLine($"{test,-45} < {result.ShortAddress, -36} : {result.FullAddress, 36} >");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"{test,-45} < ERROR: {ex.Message, -71} >");
                }
            }
        }
    }
}
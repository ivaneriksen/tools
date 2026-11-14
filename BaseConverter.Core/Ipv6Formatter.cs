namespace BaseConverter.Core
{
    public readonly record struct Ipv6Result(string FullAddress, string TrimmedAddress, string ShortAddress);

    // Format an IPv6 address string and return the full and short representations, without the use of the System.Net.IPAddress class.
    public static class Ipv6Formatter
    {
        public static Ipv6Result Format(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException("Input cannot be null or whitespace.");
            }
            if (input.Contains('.') || input.Contains('%'))
            {
                throw new ArgumentException("Embedded IPv4 (.) and zone IDs (%) are not supported yet.");
            }
            input = input.Trim().ToLower();

            // check if valid ipv6 address
            ushort[] groups = ParseGroups(input);

            return new Ipv6Result(FullAddress: ToFullForm(groups), TrimmedAddress: ToTrimmedForm(groups), ShortAddress: ToShortForm(groups));
        }

        private static ushort[] ParseGroups(string input)
        {

            string[] parts = input.Split("::");

            if (parts.Length == 1) 
            {
                string[] groups = parts[0].Split(':');
                if (groups.Length != 8)
                {
                    throw new ArgumentException($"Invalid IPv6 address: expected 8 groups, found {groups.Length}.");
                }
                for (int i = 0; i < groups.Length; i++)
                {
                    if (!IsValidGroup(groups[i]))
                    {
                        throw new ArgumentException($"Invalid IPv6 address: invalid group found at index {i}.");
                    }
                }


            }
            else if (parts.Length == 2)
            {
                int groupCount = 0;

                foreach (string part in parts)
                {
                    if (part.Length == 0)
                    {
                        continue; // An entirely empty part (like the left side of "::1") has 0 groups
                    }

                    string[] groups = part.Split(':');
                    groupCount += groups.Length;

                    foreach (string group in groups)
                    {
                        if (!IsValidGroup(group))
                        {
                            throw new ArgumentException($"Invalid IPv6 address: invalid group found in part '{part}'.");
                        }
                    }
                }

                if (groupCount > 7)
                {
                    throw new ArgumentException("Invalid IPv6 address: too many groups for '::'.");
                }
            }
            else 
            {
                throw new ArgumentException("Invalid IPv6 address: more than one '::' found.");
            }

            // Now we can parse the groups into a ushort array
            ushort[] ipv6 = new ushort[8];
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i] == "")
                {
                    continue;
                }
                string[] groups = parts[i].Split(':');
                for (int j = 0; j < groups.Length; j++)
                {
                    if (i == 0)             // Left part
                    {
                        ipv6[j] = Convert.ToUInt16(groups[j], 16);
                    }
                    else                    // Right part
                    {
                        ipv6[8 - groups.Length + j] = Convert.ToUInt16(groups[j], 16);
                    }
                }
            }

            return ipv6;
        }

        // return the shortest possible representation of the IPv6 address, using "::" to compress the longest sequence of zero groups
        private static string ToShortForm(ushort[] groups)
        {
            (int Start, int Length) best = (-1, 0);
            (int Start, int Length) current = (-1, 0);

            for (int i = 0; i < groups.Length; i++)
            {
                if (groups[i] == 0)
                {
                    if (current.Length == 0)
                    {
                        current.Start = i;
                    }
                    current.Length++;

                    if (current.Length > best.Length)
                    {
                        best = current;
                    }
                }
                else
                {
                    current = (-1, 0);
                }
            }

            string[] trimmed = groups.Select(g => g.ToString("x")).ToArray();

            if (best.Length < 2)
            {
                return string.Join(":", trimmed);
            }

            string left = string.Join(":", trimmed.Take(best.Start));
            string right = string.Join(":", trimmed.Skip(best.Start + best.Length));
            return left + "::" + right;
        }

        // return the 8 groups as hex, separated by colons, with no leading zeros
        private static string ToTrimmedForm(ushort[] groups)
        {
            return JoinGroups(groups, "x");
        }

        // return the 8 groups as 4-digit hex, separated by colons
        private static string ToFullForm(ushort[] groups)
        {
            return JoinGroups(groups, "x4");
        }

        private static bool IsValidGroup(string group)
        {
            if (string.IsNullOrEmpty(group) || group.Length > 4)
            {
                return false;
            }
            foreach (char c in group)
            {
                if (!char.IsAsciiHexDigit(c))
                {
                    return false;
                }
            }
            return true;
        }

        private static string JoinGroups(ushort[] groups, string format)
        {
            return string.Join(":", groups.Select(g => g.ToString(format)));
        }
    }
}
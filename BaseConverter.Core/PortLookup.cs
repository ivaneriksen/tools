
namespace BaseConverter.Core
{
    public readonly record struct Port(int Number, string Name, string Protocol, string Description);
    public static class PortLookup
    {
        private static readonly List<Port> Ports = new List<Port>([
            new Port(20, "FTP", "TCP", "File transfer"),
            new Port(21, "FTP", "TCP", "File transfer"),
            new Port(22, "SSH", "TCP", "Secure remote login"),
            new Port(23, "Telnet", "TCP", "Unencrypted remote login"),
            new Port(25, "SMTP", "TCP", "Sending email"),
            new Port(53, "DNS", "TCP/UDP", "Domain name lookup"),
            new Port(67, "DHCP", "UDP", "Assigning IP addresses"),
            new Port(68, "DHCP", "UDP", "Assigning IP addresses"),
            new Port(80, "HTTP", "TCP", "Web traffic"),
            new Port(110, "POP3", "TCP", "Fetching email"),
            new Port(123, "NTP", "UDP", "Time synchronization"),
            new Port(143, "IMAP", "TCP", "Fetching email"),
            new Port(161, "SNMP", "UDP", "Network monitoring"),
            new Port(389, "LDAP", "TCP", "Directory services"),
            new Port(443, "HTTPS", "TCP", "Encrypted web traffic"),
            new Port(445, "SMB", "TCP", "Windows file sharing"),
            new Port(3306, "MySQL", "TCP", "MySQL database"),
            new Port(3389, "RDP", "TCP", "Remote Desktop")
            ]);



        public static List<Port> Search(string query)
        {
            List<Port> foundPorts = [];
            foreach (Port port in Ports)
            {
                if (port.Number.ToString().Contains(query) ||
                    port.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    port.Protocol.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    port.Description.Contains(query, StringComparison.OrdinalIgnoreCase))
                {
                    foundPorts.Add(port);
                }
            }
            return foundPorts;
        }

        public static string GetRange(int port)
        {
            if (port < 0 || port > 65535) throw new ArgumentOutOfRangeException(nameof(port), "Port must be between 0 and 65535.");
            if (port < 1024) return "Well-known";
            if (port < 49152) return "Registered";
            return "Dynamic";
        }
    }
}

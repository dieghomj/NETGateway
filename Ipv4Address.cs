namespace NETGateway
{
    public class Ipv4Address
    {
        public Ipv4Address(string address)
        {
            Address = address;
            if (!ValidateAddress())
                throw new BadHttpRequestException("Invalid Address");
        }

        public string Address { get; set; }

        public bool ValidateAddress()
        {
            var dir = Address.Split(".");
            if (dir.Length != 4)
                return false;
            foreach (var dir2 in dir)
            {
                if (!int.TryParse(dir2, out var val))
                    return false;
                if (val < 0 || val >= 255) return false;
            }
            return true;
        }
    }
}

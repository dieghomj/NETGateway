namespace NETGateway
{
    public class Device
    {
        public required int UID { get; set; }
        public required string Vendor { get; set; }
        public required bool Status { get; set; }
        public string Date { get => DateTime.Now.ToString(); }
    }
}

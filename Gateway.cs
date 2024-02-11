

namespace NETGateway
{
    public class Gateway
    {
        private static List<int> _UIDErrors = new List<int>();
        public Gateway(string serialNumber, string identifier, string address, List<Device>? devices)
        {
            SerialNumber = serialNumber;
            Identifier = identifier;
            Address = new Ipv4Address(address);
            if (devices != null)
                AddDevices(devices);
        }

        public string SerialNumber { get; set; }

        public string Identifier { get; set; }

        public Ipv4Address Address { get; set; }

        public List<Device>? Devices { get; set; }

        public void AddDevices(List<Device> devices)
        {
            _UIDErrors.Clear();
            if (Devices != null)
            {
                foreach(Device d in devices)
                {
                    var uid = d.UID;
                    foreach (Device e in Devices)
                    {
                        if (uid == e.UID)
                        {
                            _UIDErrors.Add(uid);
                            continue;
                        }
                        
                        Devices.Add(d);
                    }
                }

                if (_UIDErrors.Count > 0)
                    throw new BadHttpRequestException($"The devices with this UID can't be added because already exist in this gatay :\n '{_UIDErrors}' ");
            }
            else
                Devices = new List<Device>(devices);

            CheckDevicesStored();
        }

        public void AddDevices(Device device)
        {
            if (Devices != null)
            {
                foreach(Device d in Devices)
                {
                    if(d.UID == device.UID)
                        throw new BadHttpRequestException($"The devices with this UID can't be added because already exist:\n '{device.UID}'");
                }
                Devices.Add(device);
            }

            else
                Devices = new List<Device> { device };

            CheckDevicesStored();
        }

        internal int FindDevice(int uid)
        {
            if(Devices != null)
            for(int i = 0; i < Devices.Count; i++)
                if (Devices[i].UID == uid)
                    return i;
            return -1;
        }

        private void CheckDevicesStored()
        {
            if (Devices != null && Devices.Count > 10)
            {
                throw new BadHttpRequestException("Devices limit exceeded");
            }
        }

        internal void DeleteDevice(int uid)
        {
            var pos = FindDevice(uid);
            if(pos != -1 && Devices != null)
                Devices.RemoveAt(pos);
        }
    }
}

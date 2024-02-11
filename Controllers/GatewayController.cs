using Microsoft.AspNetCore.Mvc;

namespace NETGateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ProducesErrorResponseType(typeof(BadHttpRequestException))]
    public class GatewayController : ControllerBase
    {
        private static Dictionary<string, Gateway> _gatewayDict = new();

        private readonly ILogger<GatewayController> _logger;

        public GatewayController(ILogger<GatewayController> logger)
        {
            _logger = logger;
        }
        [HttpGet(Name = "GetGateway")]
        public IEnumerable<Gateway> GetGateway()
        {
            return _gatewayDict.Values;
        }
        [HttpGet(template: "UniqueGateway", Name = "GetUniqueGateway")]
        public Gateway GetUniqueGateway(string serialNumber)
        {
            _gatewayDict.TryGetValue(serialNumber, out var gateway);
            if (gateway == null)
                throw new BadHttpRequestException($"Gateway with serial number < {serialNumber} > doesnt exist");

            return gateway;
        }

        [HttpPost(Name = "PostGateway")]
        public Gateway PostGateway(string serialNumber, string identifier, string address, List<Device>? devices)
        {
            var gateway = new Gateway(serialNumber, identifier, address, devices);
            if (_gatewayDict.ContainsKey(gateway.SerialNumber))
                throw new BadHttpRequestException($"Gateway with serial number < {gateway.SerialNumber} > already exists");

            _gatewayDict.Add(gateway.SerialNumber, gateway);
            return gateway;
        }

        [HttpPost(template: "Device", Name = "PostDevice")]
        public Gateway PostDevice(string serialNumber, Device device)
        {
            _gatewayDict.TryGetValue(serialNumber, out var gateway);
            if (gateway == null)
                throw new BadHttpRequestException($"Gateway with serial number < {serialNumber} > doesnt exists ");

            gateway.AddDevices(device);
            return gateway;
        }

        [HttpDelete(template: "Device", Name = "DeleteDevice")]
        public Gateway DeleteGateway(string serialNumber, int UID)
        {
            _gatewayDict.TryGetValue(serialNumber,out var gateway);
            if(gateway == null)
                throw new BadHttpRequestException($"Gateway with serial number < {serialNumber} > doesnt exists ");

            gateway.DeleteDevice(UID);
            return gateway;
        }
    }
}

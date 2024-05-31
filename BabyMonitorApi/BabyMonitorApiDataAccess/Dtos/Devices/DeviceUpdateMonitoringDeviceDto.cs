namespace BabyMonitorApiDataAccess.Dtos.Devices;

public class DeviceUpdateMonitoringDeviceDto
{
    public Guid? ApiKeyId { get; set; }
    public string? ApiKeyValue { get; set; }
    public Guid? DeviceId { get; set; }
    public string? StreamId { get; set; }
    public string? LivestreamUrl { get; set; }
}